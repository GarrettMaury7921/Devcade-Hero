using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using Furball.Engine.Engine.Graphics.Video;
using Microsoft.Xna.Framework.Media;
using System.Reflection.Metadata;
using Kettu;

namespace DevcadeGame.States
{
    /* 
    Class IntroState:
        IntroState Constructor
        @ Draw Method
        @ Update Method
        @ PostUpdate Method
    */
    public class IntroState : State
    {
        // Attributes
        public static List<Component> _components;
        private SpriteBatch _spriteBatch;
        public static VideoDecoder VideoDecoder;
        public Texture2D VideoTexture;
        private string State_name;
        private new GraphicsDevice _graphicsDevice;
        private TimeSpan videoDuration;
        private TimeSpan elapsedVideoTime;
        private double increase_scale;
        private Game1 game;
        private Song presentation_intro_music;
        private State menu_state;

        // Close the video when it ends
        protected void OnExiting()
        {
            // Dispose of the video decoder since a video should be playing
            IntroState.VideoDecoder.Dispose();
            Logger.StopLogging();
        }

        // Getter and Setter for graphicsDevice
        private void setGraphicsDevice(GraphicsDevice device)
        {
            _graphicsDevice = device;
        }
        private GraphicsDevice getGraphicsDevice()
        {
            return _graphicsDevice;
        }

        // Play the music for whatever intro video
        // TODO: SWITCH STATEMENT
        private void PlayPresentation()
        {
            MediaPlayer.Play(presentation_intro_music);
        }

        public IntroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {

            // Load the intro music for each video
            presentation_intro_music = _content.Load<Song>("Songs/presentation_intro_music");

            // Attributes
            State_name = _state_name;
            increase_scale = 0.0065;
            menu_state = new MenuState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "MenuState");

            // Set Graphics Device for drawing
            setGraphicsDevice(graphicsDevice);

            // Video Decoder
            VideoDecoder = new VideoDecoder(4);
            _spriteBatch = new SpriteBatch(graphicsDevice);

            // Load in the video, video is in output directory of the project
            // Have ffmpeg.auto .dll's in this directory as well
            VideoDecoder.Load("presentation.mp4", HardwareDecoderType.Any);
            PlayPresentation();

            // Get the duration of the video
            videoDuration = TimeSpan.FromSeconds(VideoDecoder.Length-1);

            Console.WriteLine($"Using hardware codec type of {VideoDecoder.HwCodecType.ToHardwareDecoderType()}!");

            PreferredBackBufferWidth = VideoDecoder.Width;
            PreferredBackBufferHeight = VideoDecoder.Height;
            Game1._graphics.ApplyChanges();

            // Set the video texture
            VideoTexture = new Texture2D(graphicsDevice, VideoDecoder.Width, VideoDecoder.Height, false, SurfaceFormat.Color);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu)
        {
            _graphicsDevice.Clear(Color.CornflowerBlue);

            byte[] frame = VideoDecoder.GetFrame((int)gameTime.TotalGameTime.TotalMilliseconds);

            if (frame != null)
            {
                VideoTexture.SetData(0, new Rectangle(0, 0, VideoTexture.Width, VideoTexture.Height), frame, 0, frame.Length);
            }
            _spriteBatch.Begin();
            // I increase the scale so it fits in the picture (slight offset to the left)
            _spriteBatch.Draw(VideoTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2((float)(((float)_graphicsDevice.Viewport.Height / VideoTexture.Height)+increase_scale)), SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Update the time
            elapsedVideoTime += gameTime.ElapsedGameTime;

            // If the time is longer than the video duration go to the menu!
            if(elapsedVideoTime >= videoDuration)
            {
                // Close the video
                OnExiting();
                // Go to the Menu State
                Game1.ChangeState(menu_state);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

    }
}

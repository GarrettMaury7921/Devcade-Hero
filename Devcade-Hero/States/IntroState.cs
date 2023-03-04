using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;
using Furball.Engine.Engine.Graphics.Video;
using System.Diagnostics;

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

        // Getter and Setter for graphicsDevice
        private void setGraphicsDevice(GraphicsDevice device)
        {
            _graphicsDevice = device;
        }
        private GraphicsDevice getGraphicsDevice()
        {
            return _graphicsDevice;
        }

        public IntroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {
            // Set Graphics Device for drawing
            setGraphicsDevice(graphicsDevice);

            // Attributes
            State_name = _state_name;

            // Video Decoder
            VideoDecoder = new VideoDecoder(4);
            _spriteBatch = new SpriteBatch(graphicsDevice);

            // Load in the video, video is in output directory of the project
            // Have ffmpeg.auto .dll's in this directory as well
            VideoDecoder.Load("main_intro.mp4", HardwareDecoderType.Any);

            // Get the duration of the video
            videoDuration = TimeSpan.FromSeconds(VideoDecoder.Length);

            Console.WriteLine($"Using hardware codec type of {VideoDecoder.HwCodecType.ToHardwareDecoderType()}!");

            PreferredBackBufferWidth = VideoDecoder.Width;
            PreferredBackBufferHeight = VideoDecoder.Height;
            Game1._graphics.ApplyChanges();

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
            _spriteBatch.Draw(VideoTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2((float)_graphicsDevice.Viewport.Height / VideoTexture.Height), SpriteEffects.None, 0);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            // Update the time
            elapsedVideoTime += gameTime.ElapsedGameTime;

            // If the time is longer than the video duration go to the menu!
            if(elapsedVideoTime >= videoDuration)
            {
                Debug.WriteLine("DONE!!!!!!");
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

    }
}

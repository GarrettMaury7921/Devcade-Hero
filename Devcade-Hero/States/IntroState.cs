using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Furball.Engine.Engine.Graphics.Video;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Devcade;
using System;
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
        private new GraphicsDevice _graphicsDevice;
        public static List<Component> _components;
        private readonly SpriteBatch _spriteBatch;
        public static VideoDecoder VideoDecoder;
        private readonly TimeSpan videoDuration;
        private readonly Song presentation_intro_music;
        private readonly Song welcome_to_the_jungle;
        private readonly Song objection_intro;
        private TimeSpan elapsedVideoTime;
        public Texture2D VideoTexture;
        private State menu_state;
        private double increase_scale;
        private string State_name;

        // For picking the video and music for the intro
        private Song songName;
        private string videoName;
        private int randomValue;
        private double cutoff_seconds;
        private MenuState menu_state2;


        // Close the video when it ends
        protected static void OnExiting()
        {
            // Dispose of the video decoder since a video should be playing
            IntroState.VideoDecoder.Dispose();
            Logger.StopLogging();
        }

        // Getter and Setter for graphicsDevice
        private void SetGraphicsDevice(GraphicsDevice device)
        {
            _graphicsDevice = device;
        }
        private GraphicsDevice GetGraphicsDevice()
        {
            return _graphicsDevice;
        }

        // Play the music for whatever intro video
        private void PickVideoAndMusic()
        {
            // THERE ARE CURRENTLY 3 INTROS
            Random random = new();
            randomValue = random.Next(1, 3); // generates a random value between x and y-1 (inclusive)
            switch (randomValue)
            {
                // SET ALL PARAMETERS FOR EACH INTRO

                // Main Intro
                case 1:
                    videoName = "main_intro.mp4";
                    songName = welcome_to_the_jungle;
                    increase_scale = 0.0075;
                    State_name = "MenuState_beat_drop";
                    cutoff_seconds = 0;
                    break;
               
                // Ace Attorney Intro
                case 2:
                    videoName = "objection_intro.mp4";
                    songName = objection_intro;
                    increase_scale = 0.006;
                    State_name = "MenuState1";
                    cutoff_seconds = 0;
                    break;

                // Mega mind Intro
                case 3:
                    videoName = "presentation.mp4";
                    songName = presentation_intro_music;
                    increase_scale = 0.0065;
                    State_name = "MenuState_beat_drop";
                    cutoff_seconds = 0; //1.25 works
                    break;

                // Default Main Intro
                default:
                    videoName = "main_intro.mp4";
                    songName = welcome_to_the_jungle;
                    increase_scale = 0.0075;
                    State_name = "MenuState_beat_drop";
                    break;

            } // switch

        } // pick video and music

        public IntroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {

            // Load the intro music for each video
            presentation_intro_music = _content.Load<Song>("Songs/presentation_intro_music");
            welcome_to_the_jungle = _content.Load<Song>("Songs/welcome_to_the_jungle_PCM");
            objection_intro = _content.Load<Song>("Songs/objection_intro");

            // Attributes
            State_name = _state_name;
            increase_scale = 0;
            menu_state = new MenuState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "MenuState");

            // Set Graphics Device for drawing
            SetGraphicsDevice(graphicsDevice);

            // Video Decoder
            VideoDecoder = new VideoDecoder(4);
            _spriteBatch = new SpriteBatch(graphicsDevice);

            // Pick the video and music randomly
            // Load in the video, video is in output directory of the project
            // Have ffmpeg.auto .dll's in this directory as well
            PickVideoAndMusic();
            VideoDecoder.Load(videoName, HardwareDecoderType.Any);

            // Play the selected song
            MediaPlayer.Play(songName);

            // Get the duration of the video, end the video early if needed
            videoDuration = TimeSpan.FromSeconds(VideoDecoder.Length-cutoff_seconds);

            // Print hardware codec and set width and height
            Console.WriteLine($"Using hardware codec type of {VideoDecoder.HwCodecType.ToHardwareDecoderType()}!");;
            Game1._graphics.ApplyChanges();

            // Set the video texture
            VideoTexture = new Texture2D(graphicsDevice, VideoDecoder.Width, VideoDecoder.Height, false, SurfaceFormat.Color);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu)
        {
            // DRAW THE VIDEO frame by frame
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

            // If the time is longer than the video duration, go to the menu!
            // OR if any key on the keyboard is being pressed, change to the menu
            if(elapsedVideoTime >= videoDuration || Keyboard.GetState().GetPressedKeys().Length > 0 || DevcadeButtonCheck() == true)
            {
                // If the user pressed a button to skip
                if (elapsedVideoTime < videoDuration)
                {
                    menu_state = new MenuState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "MenuState1");
                    // Go to the Menu State
                    Game1.ChangeState(menu_state);
                    // Close the video
                    OnExiting();
                    // Stop the media player
                    MediaPlayer.Stop();
                }
                else
                {
                    menu_state2 = new MenuState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, State_name);
                    // Go to the Menu State
                    Game1.ChangeState(menu_state2);
                    // Close the video
                    OnExiting();
                    // Stop the media player
                    MediaPlayer.Stop();
                }
            } // if statement
        } // Update method

        public override void PostUpdate(GameTime gameTime)
        {

        }

        // OTHER METHODS
        public static bool DevcadeButtonCheck()
        {
            if (Input.GetButtonDown(1, Input.ArcadeButtons.A1) || Input.GetButtonDown(1, Input.ArcadeButtons.A2)
                || Input.GetButtonDown(1, Input.ArcadeButtons.A3) || Input.GetButtonDown(1, Input.ArcadeButtons.A4)
                || Input.GetButtonDown(1, Input.ArcadeButtons.B1) || Input.GetButtonDown(1, Input.ArcadeButtons.B2)
                || Input.GetButtonDown(1, Input.ArcadeButtons.B3) || Input.GetButtonDown(1, Input.ArcadeButtons.B4)
                || Input.GetButtonDown(1, Input.ArcadeButtons.Menu) || Input.GetButtonDown(2, Input.ArcadeButtons.A1) 
                || Input.GetButtonDown(2, Input.ArcadeButtons.A2)
                || Input.GetButtonDown(2, Input.ArcadeButtons.A3) || Input.GetButtonDown(2, Input.ArcadeButtons.A4)
                || Input.GetButtonDown(2, Input.ArcadeButtons.B1) || Input.GetButtonDown(2, Input.ArcadeButtons.B2)
                || Input.GetButtonDown(2, Input.ArcadeButtons.B3) || Input.GetButtonDown(2, Input.ArcadeButtons.B4)
                || Input.GetButtonDown(2, Input.ArcadeButtons.Menu))
            {
                return true;
            }
            return false;
        }

    } // intro state class

} // name space

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DevcadeGame.States;
using DevcadeGame.Sounds;
using Devcade;
using System;

namespace DevcadeGame
{

    /* 
    Main Class Game1:
		ChangeState Constructor
		Game1 Constructor
		@ Initialize Function - Initialization setup
		@ Load Content		  - Loads the content after initialization
		@ Update Method		  - Updates every frame
        @ Draw Method		  - Contains the main draw method to draw things on the screen
    */

    public class Game1 : Game
	{
		// Attributes
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private State _currentState;
		private State _nextState;

		Texture2D main_menu;

        Song welcome_to_the_jungle;

        /// <summary>
        /// Changes State
        /// </summary>
        /// <param name="state"></param>
        public void ChangeState(State state)
		{
			_nextState = state;
		}

        /// <summary>
		/// Event handler from when a song ends
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            MediaPlayer.Play(welcome_to_the_jungle);
        }

        /// <summary>
        /// Game constructor
        /// </summary>
        public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}



		/// <summary>
		/// Does any setup prior to the first frame that doesn't need loaded content.
		/// </summary>
		protected override void Initialize()
		{
			Input.Initialize(); // Sets up the input library
			MenuSounds menuSounds = new MenuSounds(); // Sets up the starting sound volume

            // Set window size if running debug (in release it will be full screen)
            #region
#if DEBUG
            _graphics.PreferredBackBufferWidth = 420;
            _graphics.PreferredBackBufferHeight = 980;
            _graphics.ApplyChanges();
#else
			_graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
			_graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
			_graphics.ApplyChanges();
#endif
			#endregion

			base.Initialize();
		}



		/// <summary>
		/// Does any setup prior to the first frame that needs loaded content.
		/// </summary>
		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // ex.
            // texture = Content.Load<Texture2D>("fileNameWithoutExtention");

            main_menu = Content.Load<Texture2D>("Menu_Assets/vertical background");


            // Load the Current Menu State
            _currentState = new IntroState(this, _graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, Content);

            // MENU STATE
            //_currentState = new MenuState(this, _graphics.GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, Content);
            // Load and Play Songs
            //welcome_to_the_jungle = Content.Load<Song>("Songs/welcome_to_the_jungle_PCM");
            //MediaPlayer.Play(welcome_to_the_jungle);
            //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        } // End of LoadContent



		/// <summary>
		/// Your main update loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Update(GameTime gameTime)
		{
            Devcade.Input.Update(); // Updates the state of the input library

			// Exit when both menu buttons are pressed (or escape for keyboard debugging)
			// You can change this but it is suggested to keep the key bind of both menu
			// buttons at once for graceful exit.
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
				(Input.GetButton(1, Input.ArcadeButtons.Menu) &&
				Input.GetButton(2, Input.ArcadeButtons.Menu)))
			{
				Exit();
			}

            // Update the state if it is updated
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }
            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
		} // End of Update


        /// <summary>
        /// Your main draw loop. This runs once every frame, over and over.
        /// </summary>
        /// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
        protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Sprite Batch Begin
			_spriteBatch.Begin();

            // Draw the menu items and each state
            _currentState.Draw(gameTime, _spriteBatch, main_menu);
            foreach (var component in IntroState._components)
            {
                component.Draw(gameTime, _spriteBatch);
            }
            /*foreach (var component in MenuState._components)
            {
                component.Draw(gameTime, _spriteBatch);
            }*/

            // Sprite Batch End
            _spriteBatch.End();

            base.Draw(gameTime);
		} // Draw Method

	} // End of Game1

} // End of name space
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Devcade;
using System.Diagnostics;
using DevcadeGame.States;

namespace DevcadeGame
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private State _currentState;
		private State _nextState;
		Texture2D main_menu;

		// To see if sprite batch is currently drawing from other classes
        public static bool isDrawing;

        public void ChangeState(State state)
		{
			_nextState = state;
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

            // Set window size if running debug (in release it will be full screen)
            #region
#if DEBUG
            //_graphics.PreferredBackBufferWidth = 420;
            //_graphics.PreferredBackBufferHeight = 980;
            _graphics.PreferredBackBufferWidth = 1920;
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
			_currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
		}

		/// <summary>
		/// Your main update loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Update(GameTime gameTime)
		{
			Input.Update(); // Updates the state of the input library

			// Exit when both menu buttons are pressed (or escape for keyboard debugging)
			// You can change this but it is suggested to keep the key bind of both menu
			// buttons at once for graceful exit.
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) ||
				(Input.GetButton(1, Input.ArcadeButtons.Menu) &&
				Input.GetButton(2, Input.ArcadeButtons.Menu)))
			{
				Exit();
			}

			if (_nextState != null) 
			{
				_currentState = _nextState;
				_nextState = null;
			}
			_currentState.Update(gameTime);
			_currentState.PostUpdate(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// Your main draw loop. This runs once every frame, over and over.
		/// </summary>
		/// <param name="gameTime">This is the gameTime object you can use to get the time since last frame.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Sprite Batch Begin
			_spriteBatch.Begin();
			isDrawing = true;
			// Draw the main menu
			//_spriteBatch.Draw(main_menu, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
			//	new Rectangle(0, 0, 1080, 2560), Color.White);

			_currentState.Draw(gameTime, _spriteBatch);
            foreach (var component in MenuState._components)
            {
                component.Draw(gameTime, _spriteBatch);
            }
            _spriteBatch.End();
            // Sprite Batch End

			isDrawing = false;

            base.Draw(gameTime);
		}
	}
}
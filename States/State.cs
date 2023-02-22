using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
// MODIFIED VERSION OF Oyyou's MonoGame_Tutorials #13. All credit goes to Oyyou for the original code.
// https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial013


namespace DevcadeGame.States
{
    /* 
    Abstract Class State:
        State Constructor
        @ Draw Method
        @ Update Method
        @ PostUpdate Method
    */
    public abstract class State
    {
        #region fields
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;
        protected int _preferredBackBufferWidth;
        protected int _preferredBackBufferHeight;
        #endregion

        public State(Game1 game, GraphicsDevice graphicsDevice, int preferredBackBufferWidth, int preferredBackBufferHeight, ContentManager content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _preferredBackBufferWidth = preferredBackBufferWidth;
            _preferredBackBufferHeight = preferredBackBufferHeight;
            _content = content;
        }

        #region Methods
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu);
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);
        #endregion
    }
}

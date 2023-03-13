using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using DevcadeGame.GameManager;

namespace DevcadeGame.States
{
    /* 
    Class DevcadeHeroState:
        DevcadeHeroState Constructor
        @ Draw Method
        @ Update Method
        @ PostUpdate Method
    */
    public class DevcadeHeroState : State
    {
        // Attributes
        ChartReader chartReader;


        public DevcadeHeroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {
            // Put the name into the ChartReader
            chartReader = new ChartReader(_state_name);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu)
        {
        }

        public override void Update(GameTime gameTime)
        {
           
        } // Update method

        public override void PostUpdate(GameTime gameTime)
        {

        }

    } // Devcade Hero state class

} // name space

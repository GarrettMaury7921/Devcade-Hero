using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

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
        //public VideoDecoder VideoDecoder;
        public Texture2D VideoTexture;

        public IntroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content)
        {

            _components = new List<Component>()
            {

            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu)
        {
            // Draw any other game objects or components
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

    }
}

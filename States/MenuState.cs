using DevcadeGame.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
// MODIFIED VERSION OF Oyyou's MonoGame_Tutorials #13. All credit goes to Oyyou for the original code.
// https://github.com/Oyyou/MonoGame_Tutorials/tree/master/MonoGame_Tutorials/Tutorial013


namespace DevcadeGame.States
{
    /* 
    Class MenuState:
        MenuState Constructor: Loads Button Icon with fronts for the text on said button,
            - contains each button by making a new button class
        @ Draw Method
        @ Update Method
        @ PostUpdate Method
        ********** IMPLEMENT NEW BUTTON METHODS HERE ********
    */
    public class MenuState : State
    {
        public static List<Component> _components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // Load the assets for the buttons for the menu
            var buttonTexture = _content.Load<Texture2D>("Menu_Assets/button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            // ***** ALL BUTTONS ARE DEFINED BELOW *****
            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 700),
                Text = "           Single Player",
            };
            newGameButton.Click += SinglePlayerButton_Click;


            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 780),
                Text = "           Multi Player",
            };
            loadGameButton.Click += MultiPlayerButton_Click;


            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 860),
                Text = "         Quit",
            };
            quitGameButton.Click += QuitGameButton_Click;

            // Put them in _components
            _components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton,
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

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

        // Methods for when you click on each button
        private void SinglePlayerButton_Click(object sender, EventArgs e)
        {
            //_game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            Debug.WriteLine("New Game");
        }

        private void MultiPlayerButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Load Game!");
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Exit Game!");
            _game.Exit();
        }
    }
}

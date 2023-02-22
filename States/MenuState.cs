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
            var careerGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 720),
                Text = "           Career Mode",
            };
            careerGameButton.Click += CareerButton_Click;


            var casualGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 780),
                Text = "           Casual Mode",
            };
            casualGameButton.Click += CasualButton_Click;

            var settingsButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 840),
                Text = "         Settings",
            };
            settingsButton.Click += SettingsButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 900),
                Text = "         Quit",
            };
            quitGameButton.Click += QuitGameButton_Click;

            // Put them in _components
            _components = new List<Component>()
            {
                careerGameButton,
                casualGameButton,
                settingsButton,
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
            // remove the sprites if no longer needed
        }

        // Methods for when you click on each button
        private void CareerButton_Click(object sender, EventArgs e)
        {
            //_game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            Debug.WriteLine("Career");
        }

        private void CasualButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Casual");
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            _components = new List<Component>(){};
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Exit Game!");
            _game.Exit();
        }
    }
}

﻿using DevcadeGame.Controls;
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
        public static List<Component> _main_menu_components;
        public static List<Component> _settings_components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content) : 
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content)
        {

            // Load the assets for the buttons for the menu
            var buttonTexture = _content.Load<Texture2D>("Menu_Assets/button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            // ***** ALL STARTING BUTTONS ARE DEFINED BELOW *****
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

            var BackButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 900),
                Text = "         Back",
            };
            BackButton.Click += BackButton_Click;

            // ***** TYPES OF COMPONENTS *****
            _main_menu_components = new List<Component>()
            {
                careerGameButton,
                casualGameButton,
                settingsButton,
                quitGameButton,
            };
            _settings_components = new List<Component>()
            {

                BackButton,
            };

            // Put them in _components
            _components = _main_menu_components;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu)
        {
            // Draw the main menu background
            spriteBatch.Draw(main_menu, new Rectangle(0, 0, _preferredBackBufferWidth, _preferredBackBufferHeight),
                new Rectangle(0, 0, 1080, 2560), Color.White);
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
            _components = _settings_components;
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Exit Game!");
            _game.Exit();
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            _components = _main_menu_components;
        }
    }
}

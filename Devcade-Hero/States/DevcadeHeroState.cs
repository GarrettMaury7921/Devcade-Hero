using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using DevcadeGame.GameManager;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;

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
        private ChartReader chartReader;
        private GameBackgroundManager backgroundManager;
        private List<String> notes;
        private Texture2D background;
        private Song song;
        private String videoName;


        public DevcadeHeroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {
            // Put the chart into the ChartReader
            chartReader = new ChartReader(_state_name);
            notes = chartReader.GetNotes();

            // Get the background/video/song for the selected song
            backgroundManager = new GameBackgroundManager(_state_name);
            background = backgroundManager.BackgroundChooser(_content, _state_name);
            song = backgroundManager.SongChooser(_content, _state_name);
            videoName = backgroundManager.VideoChooser(_state_name);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu)
        {
            // Draw the main menu background
            if (background != null)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, _preferredBackBufferWidth, _preferredBackBufferHeight),
                new Rectangle(0, 0, 1080, 2560), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
           
        } // Update method

        public override void PostUpdate(GameTime gameTime)
        {

        }

    } // Devcade Hero state class

} // name space

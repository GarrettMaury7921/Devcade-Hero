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
        @ Initialize Method
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
        private int count;
        private int highway_width;
        private int highway_height;
        private int highwayX;
        private int highwayY;
        private int highway_offset;
        private Texture2D highway;
        private Texture2D fred_board;
        private int fred_board_width;
        private int fred_board_height;
        private int fred_boardX;
        private int fred_boardY;
        private int fred_board_offset;
        private Texture2D fred_line;
        private int fred_line_width;
        private int fred_line_height;
        private int fred_lineX;
        private int fred_lineY;
        private int fred_line_offsetX;

        public void Initialize()
        {
            #region
#if DEBUG
            highway_width = 310;
            highway_height = 735;
            fred_board_height = 21;
#else
			highway_width = 922;
            highway_height = 2000;
            fred_board_height = 64;
#endif
            #endregion

            // Attributes                                                                       // COMMENTS BELOW
            highway_offset = 10;
            highwayX = (_preferredBackBufferWidth - highway_width) / 2;
            highwayY = _preferredBackBufferHeight - highway_height - highway_offset;
            
            // Fred Board parameters
            fred_board_offset = -55;                                                            // adjust as needed
            fred_board_width = highway_width;
            fred_boardX = highwayX;
            fred_boardY = highwayY + highway_height + fred_board_offset;
            
            // Fred Board Lines
            fred_line_offsetX = 30;
            fred_line_width = 3;
            fred_line_height = highway_height + fred_board_offset + (fred_board_height - 5);    // Make the lines appear in the fred board
            fred_lineX = fred_boardX + fred_line_offsetX;                                       // apply offset to X
            fred_lineY = highwayY;

            // Load Assets
            highway = _content.Load<Texture2D>("Game_Assets/922Highway");
            fred_board = _content.Load<Texture2D>("Game_Assets/fred_board_gap");
            fred_line = _content.Load<Texture2D>("Game_Assets/note_line");

        } // Initialize Method

        public DevcadeHeroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {
            // Initialize all the variables and import in all the content
            Initialize();

            // Put the chart into the ChartReader
            chartReader = new ChartReader(_state_name);
            notes = chartReader.GetNotes();
            /*for (int i = 0; i < notes.Count; i++)
            {
                Debug.WriteLine(notes[i]);
            }*/

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

            // Draw the highway
            spriteBatch.Draw(highway, new Rectangle(highwayX, highwayY, highway_width, highway_height), Color.White);

            // Draw Fret Lines
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 34, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 68, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 103, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 145, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 179, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 213, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 247, fred_lineY, fred_line_width, fred_line_height), Color.White);

            // Draw the fred board
            spriteBatch.Draw(fred_board, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
           
        } // Update method

        public override void PostUpdate(GameTime gameTime)
        {

        }

    } // Devcade Hero state class

} // name space

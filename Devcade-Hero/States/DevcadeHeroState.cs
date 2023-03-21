using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using DevcadeGame.GameManager;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;
using Devcade;
using Microsoft.Xna.Framework.Input;

namespace DevcadeGame.States
{
    /* 
    Class DevcadeHeroState:
        DevcadeHeroState Constructor
        @ Initialize Method
        @ Draw Method
        @ Update Method
        @ PostUpdate Method
        @ DrawFretLines Method
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
        private KeyboardState previousKeyboardState;
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
        private int fred_line2;
        private int fred_line3;
        private int fred_line4;
        private int fred_line5;
        private int fred_line6;
        private int fred_line7;
        private int fred_line8;
        // Keys
        private Texture2D blue1down_pic;
        private bool blue1down;
        private Texture2D blue2down_pic;
        private bool blue2down;
        private Texture2D blue3down_pic;
        private bool blue3down;
        private Texture2D blue4down_pic;
        private bool blue4down;
        private Texture2D reddown_pic;
        private bool reddown;
        private Texture2D blue5down_pic;
        private bool blue5down;
        private Texture2D greendown_pic;
        private bool greendown;
        private Texture2D whitedown_pic;
        private bool whitedown;

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
            blue1down = false;
            blue2down = false;
            blue3down = false;
            blue4down = false;
            reddown = false;
            blue5down = false;
            greendown = false;
            whitedown = false;

            highway_offset = 10;
            highwayX = (_preferredBackBufferWidth - highway_width) / 2;
            highwayY = _preferredBackBufferHeight - highway_height - highway_offset;

            previousKeyboardState = Keyboard.GetState();
            
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
            fred_line2 = fred_lineX + 34;
            fred_line3 = fred_lineX + 68;
            fred_line4 = fred_lineX + 103;
            fred_line5 = fred_lineX + 145;
            fred_line6 = fred_lineX + 179;
            fred_line7 = fred_lineX + 213;
            fred_line8 = fred_lineX + 247;

            // Load Assets
            highway = _content.Load<Texture2D>("Game_Assets/922Highway");
            fred_board = _content.Load<Texture2D>("Game_Assets/fred_board_gap");
            fred_line = _content.Load<Texture2D>("Game_Assets/note_line");
            blue1down_pic = _content.Load<Texture2D>("Game_Assets/blue1down");
            blue2down_pic = _content.Load<Texture2D>("Game_Assets/blue2down");
            blue3down_pic = _content.Load<Texture2D>("Game_Assets/blue3down");
            blue4down_pic = _content.Load<Texture2D>("Game_Assets/blue4down");
            reddown_pic = _content.Load<Texture2D>("Game_Assets/reddown");
            blue5down_pic = _content.Load<Texture2D>("Game_Assets/blue5down");
            greendown_pic = _content.Load<Texture2D>("Game_Assets/greendown");
            whitedown_pic = _content.Load<Texture2D>("Game_Assets/whitedown");

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
            DrawFredLines(spriteBatch);

            // Draw the fred board
            spriteBatch.Draw(fred_board, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);

            // Draw held freds when pressed down
            if (blue1down)
            {
                spriteBatch.Draw(blue1down_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }
            if (blue2down)
            {
                spriteBatch.Draw(blue2down_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }
            if (blue3down)
            {
                spriteBatch.Draw(blue3down_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }
            if (blue4down)
            {
                spriteBatch.Draw(blue4down_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }

            if (reddown)
            {
                spriteBatch.Draw(reddown_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }
            if (blue5down)
            {
                spriteBatch.Draw(blue5down_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }
            if (greendown)
            {
                spriteBatch.Draw(greendown_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }
            if (whitedown)
            {
                spriteBatch.Draw(whitedown_pic, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);
            }
        } // Draw Method

        // GAME CONTROLS:
        
        // KEYBOARD (Player 1):
        //          TOP ROW: Q W E R
        //       BOTTOM ROW: Z X C V
        //
        // KEYBOARD (Player 2):
        //          TOP ROW: 7 8 9 0
        //       BOTTOM ROW: H J K L
        public override void Update(GameTime gameTime)
        {
            // Make keyboard state
            KeyboardState currentKeyboardState = Keyboard.GetState();

            CheckP1Buttons(currentKeyboardState);

            previousKeyboardState = currentKeyboardState;

        } // Update method

        public override void PostUpdate(GameTime gameTime)
        {

        }

        // OTHER METHODS
        public void DrawFredLines(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_line2, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_line3, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_line4, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_line5, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_line6, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_line7, fred_lineY, fred_line_width, fred_line_height), Color.White);
            spriteBatch.Draw(fred_line, new Rectangle(fred_line8, fred_lineY, fred_line_width, fred_line_height), Color.White);
        }

        public void CheckP1Buttons(KeyboardState currentKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Z) || Input.GetButton(1, Input.ArcadeButtons.B1))
            {
                blue1down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.Z) || Input.GetButtonUp(1, Input.ArcadeButtons.B1))
            {
                blue1down = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.X) || Input.GetButton(1, Input.ArcadeButtons.B2))
            {
                blue2down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.X) || Input.GetButtonUp(1, Input.ArcadeButtons.B2))
            {
                blue2down = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.C) || Input.GetButton(1, Input.ArcadeButtons.B3))
            {
                blue3down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.C) || Input.GetButtonUp(1, Input.ArcadeButtons.B3))
            {
                blue3down = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.V) || Input.GetButton(1, Input.ArcadeButtons.B4))
            {
                blue4down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.V) || Input.GetButtonUp(1, Input.ArcadeButtons.B4))
            {
                blue4down = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.Q) || Input.GetButton(1, Input.ArcadeButtons.A1))
            {
                reddown = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.Q) || Input.GetButtonUp(1, Input.ArcadeButtons.A1))
            {
                reddown = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.W) || Input.GetButton(1, Input.ArcadeButtons.A2))
            {
                blue5down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.W) || Input.GetButtonUp(1, Input.ArcadeButtons.A2))
            {
                blue5down = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.E) || Input.GetButton(1, Input.ArcadeButtons.A3))
            {
                greendown = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.E) || Input.GetButtonUp(1, Input.ArcadeButtons.A3))
            {
                greendown = false;
            }

            if (currentKeyboardState.IsKeyDown(Keys.R) || Input.GetButton(1, Input.ArcadeButtons.A4))
            {
                whitedown = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.R) || Input.GetButtonUp(1, Input.ArcadeButtons.A4))
            {
                whitedown = false;
            }
        } // CheckP1Buttons Method

    } // Devcade Hero state class

} // name space

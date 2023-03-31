using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using DevcadeGame.GameManager;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;
using Devcade;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

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
        private ChartTranslator chartTranslator;
        private GameBackgroundManager backgroundManager;
        private List<String> notes;
        private Texture2D background;
        private Song song;
        private String videoName;
        private KeyboardState previousKeyboardState;

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
        private int fred_line_offsetY;
        private float fred_line_left_rotationAngle;
        private float fred_line_left_rotationAngle2;
        private float fred_line_left_rotationAngle3;
        private float fred_line_left_rotationAngle4;
        private float fred_line_right_rotationAngle;
        private float fred_line_right_rotationAngle2;
        private float fred_line_right_rotationAngle3;
        private float fred_line_right_rotationAngle4;
        private Vector2 fred_line_rotationOrigin;

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

        private Texture2D note_blue;
        private Texture2D note_green;
        private Texture2D note_red;
        private Texture2D note_white;
        private int note_width;
        private int note_height;
        private int note_y;

        private Model highway3D;
        private Texture2D highway_3Dtexture;
        private Vector3 highwayPosition;

        // Set up camera
        private Matrix view;
        private Matrix projection;
        private Matrix world;
        private ModelMesh mesh;
        private float timer;

        // Note information
        private List<int> bpms;
        private List<int> bpm_time;
        private List<int> note_ticks;
        private List<int> note_color;
        private List<int> note_length;
        private List<double> time_between_notes;

        // Timing of Notes
        private float songTime;

        // Song and Sounds
        private int drum_stick_counter;
        private SoundEffect notes_ripple;
        private SoundEffect drum_sticks;

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
            timer = 0;
            drum_stick_counter = 0;
            MenuState.inGame = true;

            highway_offset = 10;
            highwayX = (_preferredBackBufferWidth - highway_width) / 2;
            highwayY = _preferredBackBufferHeight - highway_height - highway_offset;

            previousKeyboardState = Keyboard.GetState();

            // Fred Board parameters
            fred_board_offset = -35;
            fred_board_width = highway_width + 45;
            fred_boardX = highwayX - 22;
            fred_boardY = highwayY + highway_height + fred_board_offset;

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
            // Sound Assets
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged; // subscribe so no bug?
            notes_ripple = _content.Load<SoundEffect>("Sound_Effects/notes_ripple_up");
            drum_sticks = _content.Load<SoundEffect>("Sound_Effects/drum_sticks");

            // Fred Lines
            fred_line_width = 1;
            fred_line_height = 350;
            fred_line_offsetX = 54;
            fred_line_offsetY = 11;

            // FRED LINE LEFT (X, Y, Rotations)
            fred_lineX = fred_boardX + fred_line_offsetX;
            fred_lineY = fred_boardY - (fred_line_height / 2) + fred_line_offsetY;
            fred_line_left_rotationAngle = MathHelper.ToRadians(6);                     // rotate x degrees
            fred_line_left_rotationAngle2 = MathHelper.ToRadians(5);                    // rotate x degrees
            fred_line_left_rotationAngle3 = MathHelper.ToRadians(4);                    // rotate x degrees
            fred_line_left_rotationAngle4 = MathHelper.ToRadians(2);                    // rotate x degrees
            fred_line_right_rotationAngle = MathHelper.ToRadians(-1.5f);                // rotate x degrees
            fred_line_right_rotationAngle2 = MathHelper.ToRadians(-4);                  // rotate x degrees
            fred_line_right_rotationAngle3 = MathHelper.ToRadians(-5);                  // rotate x degrees
            fred_line_right_rotationAngle4 = MathHelper.ToRadians(-6);                  // rotate x degrees
            fred_line_rotationOrigin = new Vector2(fred_line.Width / 2, fred_line.Height / 2); // center of the texture

            note_blue = _content.Load<Texture2D>("Game_Assets/note_blue");
            note_green = _content.Load<Texture2D>("Game_Assets/note_green");
            note_red = _content.Load<Texture2D>("Game_Assets/note_red");
            note_white = _content.Load<Texture2D>("Game_Assets/note_white");
            note_width = 34;
            note_height = 17;

            // 3D Highway
            highway3D = _content.Load<Model>("Models/highway_obj");
            highway_3Dtexture = _content.Load<Texture2D>("Models/highway");
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _graphicsDevice.Viewport.AspectRatio, 0.0001f, 100000.0f);


            // Get the texture data
            Color[] data = new Color[highway_3Dtexture.Width * highway_3Dtexture.Height];
            highway_3Dtexture.GetData(data);

            // Flip the texture vertically
            Color[] newData = new Color[data.Length];
            for (int y = 0; y < highway_3Dtexture.Height; y++)
            {
                for (int x = 0; x < highway_3Dtexture.Width; x++)
                {
                    newData[x + y * highway_3Dtexture.Width] = data[x + (highway_3Dtexture.Height - y - 1)
                        * highway_3Dtexture.Width];
                }
            }

            // Set the new texture data
            highway_3Dtexture.SetData(newData);

            // Get the position of the highway
            Vector3 minExtents = new Vector3(float.MaxValue);
            Vector3 maxExtents = new Vector3(float.MinValue);

            foreach (ModelMesh mesh in highway3D.Meshes)
            {
                BoundingSphere meshSphere = mesh.BoundingSphere;
                BoundingBox meshBox = new BoundingBox();
                BoundingBox.CreateFromSphere(meshSphere);

                minExtents = Vector3.Min(minExtents, meshBox.Min);
                maxExtents = Vector3.Max(maxExtents, meshBox.Max);
            }

            highwayPosition = (minExtents + maxExtents) / 2f;

            // Change camera positions and targets
            view = Matrix.CreateLookAt(new Vector3(0, 6.2f, 17),
                new Vector3(0, 5.2f, 10.6f), new Vector3(0, 1, 0));

            world = Matrix.CreateTranslation(Vector3.Zero);
            _graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;


        } // Initialize Method

        public DevcadeHeroState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {
            // Initialize all the variables and import in all the content
            Initialize();

            // Put the chart into the ChartReader and then the translator
            chartReader = new ChartReader(_state_name);
            notes = chartReader.GetNotes();
            chartTranslator = new ChartTranslator(notes);

            // Get Important Chart information from the translator
            bpm_time = chartTranslator.GetBPMTickTime();
            bpms = chartTranslator.GetBPM();
            note_ticks = chartTranslator.GetNoteTickTime();
            note_color = chartTranslator.GetNoteColor();
            note_length = chartTranslator.GetNoteLength();
            time_between_notes = chartTranslator.TimeBetweenNotes();

            // Get the background/video/song for the selected song
            // _state_name is the song name
            backgroundManager = new GameBackgroundManager(_state_name);
            background = backgroundManager.BackgroundChooser(_content, _state_name);
            song = backgroundManager.SongChooser(_content, _state_name);
            videoName = backgroundManager.VideoChooser(_state_name);

            // Play the sound effect
            notes_ripple.Play();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D main_menu)
        {
            _graphicsDevice.Clear(Color.Black);

            // Draw the main menu background
            if (background != null)
            {
                spriteBatch.End();
                spriteBatch.Begin();
                spriteBatch.Draw(background, new Rectangle(0, 0, _preferredBackBufferWidth, _preferredBackBufferHeight),
                new Rectangle(0, 0, 1080, 2560), Color.White);
                spriteBatch.End();
            }

            // 3D Highway
            Matrix[] transforms = new Matrix[highway3D.Bones.Count];
            highway3D.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in highway3D.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.Texture = highway_3Dtexture;
                    effect.TextureEnabled = true;
                    effect.EnableDefaultLighting();

                }
                mesh.Draw();
            }

            spriteBatch.Begin();

            // DRAW NOTES

            // Draw Fret Lines
            DrawFredLines(spriteBatch);

            // Draw the fred board
            spriteBatch.Draw(fred_board, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);

            // Draw held freds when pressed down
            DrawHeldFredLines(spriteBatch);

            // DRAW NOTES

        }


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

            if (timer < 0.5)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 500;
                world = Matrix.CreateRotationX(timer) * Matrix.CreateTranslation(Vector3.Zero);
            }
            else
            {
                if (timer > 0.5 && timer < 1.5 && drum_stick_counter == 0)
                {
                    timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 500;
                    drum_sticks.Play();
                    drum_stick_counter++;

                }
                else
                {
                    songTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    // Debug.WriteLine(songTime);
                    if (songTime >= 4.6f && drum_stick_counter == 1)
                    {
                        songTime -= 4.6f;
                        drum_stick_counter++;
                        MediaPlayer.Play(song);
                    }
                }

            }

        } // Update method

        public override void PostUpdate(GameTime gameTime)
        {

        }

        // OTHER METHODS
        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            MenuState.inGame = true;
        } // MediaPlayer_MediaStateChanged method
        public void DrawFredLines(SpriteBatch spriteBatch)
        {
            // Left
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX, fred_lineY, fred_line_width, fred_line_height), 
                null, Color.White, fred_line_left_rotationAngle, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 35, fred_lineY, fred_line_width, fred_line_height),
                null, Color.White, fred_line_left_rotationAngle2, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 72, fred_lineY, fred_line_width, fred_line_height),
                null, Color.White, fred_line_left_rotationAngle3, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 105, fred_lineY, fred_line_width, fred_line_height),
                null, Color.White, fred_line_left_rotationAngle4, fred_line_rotationOrigin, SpriteEffects.None,
                0f);

            // Right
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 143, fred_lineY, fred_line_width, fred_line_height),
                null, Color.White, fred_line_right_rotationAngle, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 174, fred_lineY, fred_line_width, fred_line_height),
                null, Color.White, fred_line_right_rotationAngle2, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 209, fred_lineY, fred_line_width, fred_line_height),
                null, Color.White, fred_line_right_rotationAngle3, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, new Rectangle(fred_lineX + 243, fred_lineY, fred_line_width, fred_line_height),
                null, Color.White, fred_line_right_rotationAngle4, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
        }

        public void DrawHeldFredLines(SpriteBatch spriteBatch)
        {
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
        } // DrawHeldSpriteLines Method

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
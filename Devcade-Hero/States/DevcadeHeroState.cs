using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using DevcadeHero.GameManager;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;
using Devcade;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using System.Diagnostics;

namespace DevcadeHero.States
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
        private readonly ChartReader chartReader;
        private readonly ChartTranslator chartTranslator;
        private readonly GameBackgroundManager backgroundManager;
        private readonly List<String> chart_lines;
        private readonly Texture2D background;
        private readonly Song song;
        private String videoName;
        private KeyboardState previousKeyboardState;

        private int highway_width;
        private int highway_height;
        private int highwayX;
        private int highwayY;
        private int highway_offset;

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
        private Rectangle fred_line_left_rectangle;
        private Rectangle fred_line_left_rectangle2;
        private Rectangle fred_line_left_rectangle3;
        private Rectangle fred_line_left_rectangle4;
        private Rectangle fred_line_right_rectangle;
        private Rectangle fred_line_right_rectangle2;
        private Rectangle fred_line_right_rectangle3;
        private Rectangle fred_line_right_rectangle4;

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
        private int note_x;
        private readonly int note_count;

        // Make Note Rectangle and Fred Note down Rectangles for hit box purposes
        private Rectangle blue1_down_rect;
        private Rectangle blue2_down_rect;
        private Rectangle blue3_down_rect;
        private Rectangle blue4_down_rect;
        private Rectangle red_down_rect;
        private Rectangle blue5_down_rect;
        private Rectangle green_down_rect;
        private Rectangle white_down_rect;

        private Model highway3D;
        private Texture2D highway_3Dtexture;

        // Set up camera
        private Matrix view;
        private Matrix projection;
        private Matrix world;
        private float timer;

        // Note information
        private readonly List<int> bpms;
        private readonly List<int> bpm_time;
        private readonly List<int> note_ticks;
        private readonly List<int> note_color;
        private readonly List<int> note_length;
        private readonly List<double> time_between_notes;
        private List<Note> notes;
        private List<int> multi_notes;
        private bool songPlaying;
        private bool songPlayed;

        // Timing of Notes
        private float songTime;

        // Song and Sounds
        private int drum_stick_counter;
        private SoundEffect notes_ripple;
        private SoundEffect drum_sticks;
        private SoundEffect bad_note_hit;
        private SoundEffect bad_note_hit2;
        private SoundEffect bad_note_hit3;
        private SoundEffect bad_note_hit4;
        private SoundEffect bad_note_hit5;

        // Note timing
        private System.Timers.Timer buttonTimer;
        private System.Timers.Timer multiButtonTimer;
        private bool canPressButton;
        private bool multiCanPressButton;

        // Score calculations
        private int noteHits;
        private int noteLateMiss;

        // Debug flag
        private readonly bool debug = false;
        private readonly bool debug_note_detection = false;


        public void Initialize()
        {
            #region
#if DEBUG
            highway_width = 922;
            highway_height = 2000;
            fred_board_height = 64;
#else
			highway_width = 922;
            highway_height = 2000;
            fred_board_height = 64;
#endif
            #endregion
            /*
            highway_width = 310;
            highway_height = 735;
            fred_board_height = 21;
            */


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
            songPlaying = false;
            songPlayed = false;

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
            note_blue = _content.Load<Texture2D>("Game_Assets/note_blue");
            note_green = _content.Load<Texture2D>("Game_Assets/note_green");
            note_red = _content.Load<Texture2D>("Game_Assets/note_red");
            note_white = _content.Load<Texture2D>("Game_Assets/note_white");

            // Sound Assets
            notes_ripple = _content.Load<SoundEffect>("Sound_Effects/notes_ripple_up");
            drum_sticks = _content.Load<SoundEffect>("Sound_Effects/drum_sticks");
            bad_note_hit = _content.Load<SoundEffect>("Sound_Effects/bad_note1");
            bad_note_hit2 = _content.Load<SoundEffect>("Sound_Effects/bad_note2");
            bad_note_hit3 = _content.Load<SoundEffect>("Sound_Effects/bad_note3");
            bad_note_hit4 = _content.Load<SoundEffect>("Sound_Effects/bad_note4");
            bad_note_hit5 = _content.Load<SoundEffect>("Sound_Effects/bad_note5");

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

            fred_line_left_rectangle = new Rectangle(fred_lineX, fred_lineY, fred_line_width, fred_line_height);
            fred_line_left_rectangle2 = new Rectangle(fred_lineX + 35, fred_lineY, fred_line_width, fred_line_height);
            fred_line_left_rectangle3 = new Rectangle(fred_lineX + 72, fred_lineY, fred_line_width, fred_line_height);
            fred_line_left_rectangle4 = new Rectangle(fred_lineX + 105, fred_lineY, fred_line_width, fred_line_height);
            fred_line_right_rectangle = new Rectangle(fred_lineX + 143, fred_lineY, fred_line_width, fred_line_height);
            fred_line_right_rectangle2 = new Rectangle(fred_lineX + 174, fred_lineY, fred_line_width, fred_line_height);
            fred_line_right_rectangle3 = new Rectangle(fred_lineX + 209, fred_lineY, fred_line_width, fred_line_height);
            fred_line_right_rectangle4 = new Rectangle(fred_lineX + 243, fred_lineY, fred_line_width, fred_line_height);

            note_blue = _content.Load<Texture2D>("Game_Assets/note_blue");
            note_green = _content.Load<Texture2D>("Game_Assets/note_green");
            note_red = _content.Load<Texture2D>("Game_Assets/note_red");
            note_white = _content.Load<Texture2D>("Game_Assets/note_white");
            note_width = 34;
            note_height = 17;
            note_x = 100;
            note_y = 450;
            notes = new List<Note>();

            // MODIFYING ASSETS FOR BIGGER SCREEN
            // Arcade Machine
            if (_preferredBackBufferHeight >= 2560)
            {
                // Fred Board parameters
                fred_board_offset = -55;
                fred_board_width = highway_width + 0;
                fred_boardX = highwayX - 0;
                fred_boardY = highwayY + highway_height + fred_board_offset;

                // Fred Lines
                fred_line_width *= 3;
                fred_line_height *= 3;
                fred_line_offsetX *= 3;
                fred_line_offsetY *= 3;

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

                fred_line_left_rectangle = new Rectangle(fred_lineX - 13, fred_lineY, fred_line_width, fred_line_height);
                fred_line_left_rectangle2 = new Rectangle(fred_lineX + 78, fred_lineY, fred_line_width, fred_line_height);
                fred_line_left_rectangle3 = new Rectangle(fred_lineX + 170, fred_lineY, fred_line_width, fred_line_height);
                fred_line_left_rectangle4 = new Rectangle(fred_lineX + 254, fred_lineY, fred_line_width, fred_line_height);

                fred_line_right_rectangle = new Rectangle(fred_lineX + 348, fred_lineY, fred_line_width, fred_line_height);
                fred_line_right_rectangle2 = new Rectangle(fred_lineX + 423, fred_lineY, fred_line_width, fred_line_height);
                fred_line_right_rectangle3 = new Rectangle(fred_lineX + 518, fred_lineY, fred_line_width, fred_line_height);
                fred_line_right_rectangle4 = new Rectangle(fred_lineX + 612, fred_lineY, fred_line_width, fred_line_height);

                note_width *= 3;
                note_height *= 3;
                note_x = 100;
                note_y = 450;
            }

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
            Vector3 minExtents = new(float.MaxValue);
            Vector3 maxExtents = new(float.MinValue);

            foreach (ModelMesh mesh in highway3D.Meshes)
            {
                BoundingSphere meshSphere = mesh.BoundingSphere;
                BoundingBox meshBox = new();
                BoundingBox.CreateFromSphere(meshSphere);

                minExtents = Vector3.Min(minExtents, meshBox.Min);
                maxExtents = Vector3.Max(maxExtents, meshBox.Max);
            }

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

            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged; // subscribe so no bug

            // Put the chart into the ChartReader and then the translator
            chartReader = new ChartReader(_state_name);
            chart_lines = chartReader.GetNotes();
            chartTranslator = new ChartTranslator(chart_lines);

            // Get Important Chart information from the translator
            bpm_time = chartTranslator.GetBPMTickTime();
            bpms = chartTranslator.GetBPM();
            note_ticks = chartTranslator.GetNoteTickTime();
            note_color = chartTranslator.GetNoteColor();
            note_length = chartTranslator.GetNoteLength();
            time_between_notes = chartTranslator.TimeBetweenNotes();
            note_count = chartTranslator.GetNoteCount();
            multi_notes = chartTranslator.FindMultiNotes();

            // Make the notes so we can draw them later
            MakeNotes(note_ticks, note_color, note_length, multi_notes);
            SetupMultiNoteDetection(notes.ToList());

            // Make the note rectangles
            blue1_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);
            blue2_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);
            blue3_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);
            blue4_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);
            red_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);
            blue5_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);
            green_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);
            white_down_rect = new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height);

            // Get the background/video/song for the selected song
            // _state_name is the song name
            backgroundManager = new GameBackgroundManager(_state_name);
            background = backgroundManager.BackgroundChooser(_content, _state_name);
            song = backgroundManager.SongChooser(_content, _state_name);
            videoName = backgroundManager.VideoChooser(_state_name);

            // Play the sound effect
            notes_ripple.Play();

            // Note Timing
            buttonTimer = new System.Timers.Timer
            {
                Interval = 500000
            };
            multiButtonTimer = new System.Timers.Timer
            {
                Interval = 500000
            };
            canPressButton = true;
            multiCanPressButton = true;
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
                for (int i = 0; i < mesh.Effects.Count; i++)
                {
                    BasicEffect effect = (BasicEffect)mesh.Effects[i];
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

            // Draw Fret Lines
            DrawFredLines(spriteBatch);

            // Draw the fred board
            spriteBatch.Draw(fred_board, new Rectangle(fred_boardX, fred_boardY, fred_board_width, fred_board_height), Color.White);

            // Draw held freds when pressed down
            DrawHeldFredNotes(spriteBatch);

            // DRAW NOTES WHEN READY AFTER DRUMSTICKS
            if (songPlayed)
            {
                // Filter the notes that are supposed to appear at or before the current song time
                var visibleNotes = notes.Where(n => n.NoteTime - time_between_notes.First() <= songTime);

                // Draw the visible notes
                foreach (Note note in visibleNotes)
                {
                    note.CalculatePosition(note.Position, note.fretLineRotationAngle, note_width, note_height);
                    spriteBatch.Draw(note.Texture, note.Position, Color.White);
                    note.isVisible = true;
                }

            } // drum stick if statement

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

            // Check if user is pressing the correct note while the texture is over it
            NoteHitDetection(blue1_down_rect, blue2_down_rect, blue3_down_rect, blue4_down_rect, red_down_rect,
                blue5_down_rect, green_down_rect, white_down_rect);

            previousKeyboardState = currentKeyboardState;

            // Game and song setup
            // Contains logic where sound effects are played at the beginning of the song
            // The game also goes back to the main menu after a song
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
                    // Increment the song time
                    songTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    // SONG TIME
                    // Debug.WriteLine(songTime);

                    if (songTime >= 4.6f && drum_stick_counter == 1)
                    {
                        drum_stick_counter++;
                        songPlaying = true;
                    }

                    if (songTime >= 5.1f && drum_stick_counter == 2)
                    {
                        songTime -= 5.1f;
                        if (!songPlayed)
                        {
                            MediaPlayer.Play(song);
                            drum_stick_counter++;
                        }

                    }
                    // Implement the delay for the notes
                    if (songTime > backgroundManager.delay && drum_stick_counter == 3)
                    {
                        songPlayed = true;
                        songTime -= backgroundManager.delay;
                        drum_stick_counter++;
                    }

                    // If the song is over
                    if (songTime > MediaPlayer.Queue.ActiveSong.Duration.TotalSeconds)
                    {
                        // Stop everything
                        MediaPlayer.Stop();

                        // Reset all Variables
                        Initialize();

                        MediaPlayer.MediaStateChanged -= MediaPlayer_MediaStateChanged;
                        MenuState.inGame = false;
                        _graphicsDevice.Reset();

                        // Change back to menu for now
                        MenuState menu_state = new(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "HeroToMenu");
                        // Go to the Menu State
                        Game1.ChangeState(menu_state);
                    }

                } // else

            } // else

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
            spriteBatch.Draw(fred_line, fred_line_left_rectangle,
                null, Color.White, fred_line_left_rotationAngle, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, fred_line_left_rectangle2,
                null, Color.White, fred_line_left_rotationAngle2, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, fred_line_left_rectangle3,
                null, Color.White, fred_line_left_rotationAngle3, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, fred_line_left_rectangle4,
                null, Color.White, fred_line_left_rotationAngle4, fred_line_rotationOrigin, SpriteEffects.None,
                0f);

            // Right
            spriteBatch.Draw(fred_line, fred_line_right_rectangle,
                null, Color.White, fred_line_right_rotationAngle, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, fred_line_right_rectangle2,
                null, Color.White, fred_line_right_rotationAngle2, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, fred_line_right_rectangle3,
                null, Color.White, fred_line_right_rotationAngle3, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
            spriteBatch.Draw(fred_line, fred_line_right_rectangle4,
                null, Color.White, fred_line_right_rotationAngle4, fred_line_rotationOrigin, SpriteEffects.None,
                0f);
        }

        public void DrawHeldFredNotes(SpriteBatch spriteBatch)
        {
            // Draw held freds when pressed down
            if (blue1down)
            {
                spriteBatch.Draw(blue1down_pic, blue1_down_rect, Color.White);
            }
            if (blue2down)
            {
                spriteBatch.Draw(blue2down_pic, blue2_down_rect, Color.White);
            }
            if (blue3down)
            {
                spriteBatch.Draw(blue3down_pic, blue3_down_rect, Color.White);
            }
            if (blue4down)
            {
                spriteBatch.Draw(blue4down_pic, blue4_down_rect, Color.White);
            }

            if (reddown)
            {
                spriteBatch.Draw(reddown_pic, red_down_rect, Color.White);
            }
            if (blue5down)
            {
                spriteBatch.Draw(blue5down_pic, blue5_down_rect, Color.White);
            }
            if (greendown)
            {
                spriteBatch.Draw(greendown_pic, green_down_rect, Color.White);
            }
            if (whitedown)
            {
                spriteBatch.Draw(whitedown_pic, white_down_rect, Color.White);
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

                // Set "canPressButton" to true again
                canPressButton = true;
                // Stop the timer
                buttonTimer.Stop();
            }

            if (currentKeyboardState.IsKeyDown(Keys.X) || Input.GetButton(1, Input.ArcadeButtons.B2))
            {
                blue2down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.X) || Input.GetButtonUp(1, Input.ArcadeButtons.B2))
            {
                blue2down = false;

                canPressButton = true;
                buttonTimer.Stop();
            }

            if (currentKeyboardState.IsKeyDown(Keys.C) || Input.GetButton(1, Input.ArcadeButtons.B3))
            {
                blue3down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.C) || Input.GetButtonUp(1, Input.ArcadeButtons.B3))
            {
                blue3down = false;

                canPressButton = true;
                buttonTimer.Stop();
            }

            if (currentKeyboardState.IsKeyDown(Keys.V) || Input.GetButton(1, Input.ArcadeButtons.B4))
            {
                blue4down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.V) || Input.GetButtonUp(1, Input.ArcadeButtons.B4))
            {
                blue4down = false;

                canPressButton = true;
                buttonTimer.Stop();
            }

            if (currentKeyboardState.IsKeyDown(Keys.Q) || Input.GetButton(1, Input.ArcadeButtons.A1))
            {
                reddown = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.Q) || Input.GetButtonUp(1, Input.ArcadeButtons.A1))
            {
                reddown = false;

                canPressButton = true;
                buttonTimer.Stop();
            }

            if (currentKeyboardState.IsKeyDown(Keys.W) || Input.GetButton(1, Input.ArcadeButtons.A2))
            {
                blue5down = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.W) || Input.GetButtonUp(1, Input.ArcadeButtons.A2))
            {
                blue5down = false;

                canPressButton = true;
                buttonTimer.Stop();
            }

            if (currentKeyboardState.IsKeyDown(Keys.E) || Input.GetButton(1, Input.ArcadeButtons.A3))
            {
                greendown = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.E) || Input.GetButtonUp(1, Input.ArcadeButtons.A3))
            {
                greendown = false;

                canPressButton = true;
                buttonTimer.Stop();
            }

            if (currentKeyboardState.IsKeyDown(Keys.R) || Input.GetButton(1, Input.ArcadeButtons.A4))
            {
                whitedown = true;
            }
            else if (previousKeyboardState.IsKeyDown(Keys.R) || Input.GetButtonUp(1, Input.ArcadeButtons.A4))
            {
                whitedown = false;

                canPressButton = true;
                buttonTimer.Stop();
            }
        } // CheckP1Buttons Method

        public void NoteHitDetection(Rectangle blue1_down_rect, Rectangle blue2_down_rect, Rectangle blue3_down_rect,
            Rectangle blue4_down_rect, Rectangle red_down_rect, Rectangle blue5_down_rect, Rectangle green_down_rect,
            Rectangle white_down_rect)
        {
            // Go through each note
            bool lane1 = false;
            bool lane2 = false;
            bool lane3 = false;
            bool lane4 = false;
            bool lane5 = false;
            bool lane6 = false;
            bool lane7 = false;
            bool lane8 = false;
            int amountOfNotes = 0;

            foreach (Note note in notes.ToList())
            {
                // Check for skipping notes because of multi notes
                if (amountOfNotes > 0)
                {
                    amountOfNotes--;
                    continue;
                }
                
                // Is the note is even visible
                if (note.isVisible)
                {
                    // SINGLE NOTES - HIT DETECTION
                    if (!note.isMultiNote && note.multiNoteLanes == null)
                    {
                        // Depending on what lane the note is in
                        switch (note.Lane)
                        {
                            case 4:
                                if (blue1down && note.Position.Intersects(new Rectangle(
                                    blue1_down_rect.X,
                                    blue1_down_rect.Y - 8,
                                    blue1_down_rect.Width,
                                    blue1_down_rect.Height + 8)) && canPressButton)
                                {
                                    // If the if statement condition is true, set "canPressButton" to false and start a new timer
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue1 Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (blue1down && !note.Position.Intersects(new Rectangle(
                                    blue1_down_rect.X,
                                    blue1_down_rect.Y - 8,
                                    blue1_down_rect.Width,
                                    blue1_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue1 Missed!!");
                                    PlayBadNote();
                                }
                                lane1 = true;
                                break;
                            case 5:
                                if (blue2down && note.Position.Intersects(new Rectangle(
                                    blue2_down_rect.X,
                                    blue2_down_rect.Y - 8,
                                    blue2_down_rect.Width,
                                    blue2_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue2 Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (blue2down && !note.Position.Intersects(new Rectangle(
                                    blue2_down_rect.X,
                                    blue2_down_rect.Y - 8,
                                    blue2_down_rect.Width,
                                    blue2_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue2 Missed!!");
                                    PlayBadNote();
                                }
                                lane2 = true;
                                break;
                            case 6:
                                if (blue3down && note.Position.Intersects(new Rectangle(
                                    blue3_down_rect.X,
                                    blue3_down_rect.Y - 8,
                                    blue3_down_rect.Width,
                                    blue3_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue3 Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (blue3down && !note.Position.Intersects(new Rectangle(
                                    blue3_down_rect.X,
                                    blue3_down_rect.Y - 8,
                                    blue3_down_rect.Width,
                                    blue3_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue3 Missed!!");
                                    PlayBadNote();
                                }
                                lane3 = true;
                                break;
                            case 7:
                                if (blue4down && note.Position.Intersects(new Rectangle(
                                    blue4_down_rect.X,
                                    blue4_down_rect.Y - 8,
                                    blue4_down_rect.Width,
                                    blue4_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue4 Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (blue4down && !note.Position.Intersects(new Rectangle(
                                    blue4_down_rect.X,
                                    blue4_down_rect.Y - 8,
                                    blue4_down_rect.Width,
                                    blue4_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue4 Missed!!");
                                    PlayBadNote();
                                }
                                lane4 = true;
                                break;
                            case 0:
                                if (reddown && note.Position.Intersects(new Rectangle(
                                    red_down_rect.X,
                                    red_down_rect.Y - 8,
                                    red_down_rect.Width,
                                    red_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("red Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (reddown && !note.Position.Intersects(new Rectangle(
                                    red_down_rect.X,
                                    red_down_rect.Y - 8,
                                    red_down_rect.Width,
                                    red_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("red Missed!!");
                                    PlayBadNote();
                                }
                                lane5 = true;
                                break;
                            case 1:
                                if (blue5down && note.Position.Intersects(new Rectangle(
                                    blue5_down_rect.X,
                                    blue5_down_rect.Y - 8,
                                    blue5_down_rect.Width,
                                    blue5_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue5 Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (blue5down && !note.Position.Intersects(new Rectangle(
                                    blue5_down_rect.X,
                                    blue5_down_rect.Y - 8,
                                    blue5_down_rect.Width,
                                    blue5_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("Blue5 Missed!!");
                                    PlayBadNote();
                                }
                                lane6 = true;
                                break;
                            case 2:
                                if (greendown && note.Position.Intersects(new Rectangle(
                                    green_down_rect.X,
                                    green_down_rect.Y - 8,
                                    green_down_rect.Width,
                                    green_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("green Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (greendown && !note.Position.Intersects(new Rectangle(
                                    green_down_rect.X,
                                    green_down_rect.Y - 8,
                                    green_down_rect.Width,
                                    green_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("green Missed!!");
                                    PlayBadNote();
                                }
                                lane7 = true;
                                break;
                            case 3:
                                if (whitedown && note.Position.Intersects(new Rectangle(
                                    white_down_rect.X,
                                    white_down_rect.Y - 8,
                                    white_down_rect.Width,
                                    white_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("white Hit!!");
                                    notes.Remove(note);
                                    note.isVisible = false;
                                }
                                else if (whitedown && !note.Position.Intersects(new Rectangle(
                                    white_down_rect.X,
                                    white_down_rect.Y - 8,
                                    white_down_rect.Width,
                                    white_down_rect.Height + 8)) && canPressButton)
                                {
                                    canPressButton = false;
                                    buttonTimer.Start();

                                    Debug.WriteLine("white Missed!!");
                                    PlayBadNote();
                                }
                                lane8 = true;
                                break;

                        } // switch statement   

                    } // if statement

                    // Check whether the note is a double note or a single note
                    else if (note.isMultiNote && note.multiNoteLanes != null)
                    {

                        // Keep track of the button states
                        Dictionary<int, bool> buttonStates = new()
                        {
                            { 0, reddown },
                            { 1, blue5down },
                            { 2, greendown },
                            { 3, whitedown },
                            { 4, blue1down },
                            { 5, blue2down },
                            { 6, blue3down },
                            { 7, blue4down }
                        };

                        
                        /*foreach (var kvp in buttonStates)
                        {
                            Debug.WriteLine($"Button {kvp.Key}: {kvp.Value}");
                        }*/
                        

                        // If there is collision of the multi note
                        if ((note.Position.Intersects(new Rectangle(
                                            red_down_rect.X,
                                            red_down_rect.Y - 8,
                                            red_down_rect.Width,
                                            red_down_rect.Height + 8)) && canPressButton)
                           || (note.Position.Intersects(new Rectangle(
                                            blue5_down_rect.X,
                                            blue5_down_rect.Y - 8,
                                            blue5_down_rect.Width,
                                            blue5_down_rect.Height + 8)) && canPressButton)
                           || (note.Position.Intersects(new Rectangle(
                                            green_down_rect.X,
                                            green_down_rect.Y - 8,
                                            green_down_rect.Width,
                                            green_down_rect.Height + 8)) && canPressButton)
                           || (note.Position.Intersects(new Rectangle(
                                            white_down_rect.X,
                                            white_down_rect.Y - 8,
                                            white_down_rect.Width,
                                            white_down_rect.Height + 8)) && canPressButton)
                           || (note.Position.Intersects(new Rectangle(
                                            blue1_down_rect.X,
                                            blue1_down_rect.Y - 8,
                                            blue1_down_rect.Width,
                                            blue1_down_rect.Height + 8)) && canPressButton)
                           || (note.Position.Intersects(new Rectangle(
                                            blue2_down_rect.X,
                                            blue2_down_rect.Y - 8,
                                            blue2_down_rect.Width,
                                            blue2_down_rect.Height + 8)) && canPressButton)
                           || (note.Position.Intersects(new Rectangle(
                                            blue3_down_rect.X,
                                            blue3_down_rect.Y - 8,
                                            blue3_down_rect.Width,
                                            blue3_down_rect.Height + 8)) && canPressButton)
                           || (note.Position.Intersects(new Rectangle(
                                            blue4_down_rect.X,
                                            blue4_down_rect.Y - 8,
                                            blue4_down_rect.Width,
                                            blue4_down_rect.Height + 8)) && canPressButton))
                        {
                            // If there is a collision with the multi-note
                            bool allButtonsPressed = true;

                            foreach (int requiredLane in note.multiNoteLanes)
                            {
                                if (!buttonStates.ContainsKey(requiredLane) || !buttonStates[requiredLane])
                                {
                                    // If any of the required buttons is not pressed, set allButtonsPressed to false
                                    allButtonsPressed = false;
                                    break;
                                }
                            }

                            // If they hit the multi note
                            if (allButtonsPressed)
                            {
                                Debug.WriteLine("MULTI-NOTE HITTTTTTT!!!!!!!!!!!!!!!!!!!");

                                // Collect notes associated with the multi-note
                                List<Note> notesToRemove = new List<Note>();
                                int count = 0;

                                // Find how many buttons need to be pressed
                                foreach (int requiredLane in note.multiNoteLanes)
                                {
                                    count++;
                                }

                                for (int i = 0; i < count; i++)
                                {
                                    // Add this note, then add the others
                                    if (i == 0) {
                                        notesToRemove.Add(note);
                                    }
                                    else
                                    {
                                        Note nextNote = notes[i];
                                        notesToRemove.Add(nextNote);
                                    }
                                }

                                // Remove the collected notes
                                foreach (Note noteToRemove in notesToRemove)
                                {
                                    notes.Remove(noteToRemove);
                                    noteToRemove.isVisible = false;
                                }

                                canPressButton = false;
                                buttonTimer.Start();

                                // Start the multi hit timer to stop the 'missed, nothing in lane'
                                multiCanPressButton = false;
                                multiButtonTimer.Start();

                                // Amount of times to skip in the loop
                                amountOfNotes = note.howManyMulti;
                                
                            }

                        } // if it intersects

                    } // multi note hit detection

                } // if the note is visible


                // Check if note is off screen when missed
                if ((note.Position.Y > _preferredBackBufferHeight) && songPlayed)
                {
                    //Debug.WriteLine("NOTE OFF SCREEN!");
                    notes.Remove(note);
                    note.isVisible = false;
                    PlayBadNote();
                }

            } // for each

            // ELAPSED EVENT HANDLER FOR THE TIMER
            buttonTimer.Elapsed += (sender, e) => {
                // Set "canPressButton" to true again
                canPressButton = true;
                // Stop the timer
                buttonTimer.Stop();
            };

            // ELAPSED EVENT HANDLER FOR THE MULTI TIMER
            multiButtonTimer.Elapsed += (sender, e) => {
                // Set "canPressButton" to true again
                multiCanPressButton = true;
                // Stop the timer
                multiButtonTimer.Stop();
            };

            if (((blue1down && !lane1) || (blue2down && !lane2) || (blue3down && !lane3) || (blue4down && !lane4) ||
                (reddown && !lane5) || (blue5down && !lane6) || (greendown && !lane7) || (whitedown && !lane8)) && canPressButton && multiCanPressButton)
            {
                // If the if statement condition is true, set "canPressButton" to false and start a new timer
                canPressButton = false;

                buttonTimer.Start();

                Debug.WriteLine("Missed, NOTHING IN LANE!");
                PlayBadNote();
            }

        } // note hit detection method

        public void MakeNotes(List<int> ticks, List<int> color, List<int> length, List<int> multi_note)
        {
            // Upper Variables
            int multiCount = 0;
            int multiCount2 = 0;

            // Go through each note
            for (int i = 0; i < note_count; i++)
            {
                // Attributes
                Texture2D texture = null;
                int lane = color[i];
                float angle = 0f;
                Rectangle fredline_rect = new();
                bool isMulti = false;

                // See if this is a multi note from the last one
                if (multiCount2 >= 2)
                {
                    isMulti = true;
                    multiCount2--;

                    // Already one number above what it should be
                    if (multiCount2 == 1)
                    {
                        multiCount2 = 0;
                    }
                }

                // Figure out if it is a double note
                if (multi_note[i] == 1)
                {
                    isMulti = true;

                    // Check if there are more than 2 notes
                    multiCount = 2;
                    bool loop = true;
                    int x = 1;
                    while (loop)
                    {
                        // Make sure the multi note does not go out of bounds
                        if (i + x < multi_note.Count && multi_note[i + x] == 1)
                        {
                            x++;
                            multiCount++;
                        }
                        else
                        {
                            loop = false;
                        }
                    }

                    multiCount2 = multiCount;

                    // Get the next amount of notes to be multi depending on the multiCount
                    if (debug)
                    {
                        Debug.WriteLine("MULTI ON " + (i + 1) + " With " + multiCount + " Notes! multiCount2: " + multiCount2);
                    }
                }

                // Associate a texture with the color
                // AND FRET LINE, ROTATION ANGLE
                //  N 4 = blue1
                //  N 5 = blue2
                //  N 6 = blue3
                //  N 7 = blue4
                //  N 0 = red
                //  N 1 = blue5
                //  N 2 = green
                //  N 3 = white

                switch (color[i])
                {
                    case 0:
                        texture = note_red;
                        angle = fred_line_right_rotationAngle;
                        fredline_rect = fred_line_right_rectangle;
                        break;
                    case 1:
                        texture = note_blue;
                        angle = fred_line_right_rotationAngle2;
                        fredline_rect = fred_line_right_rectangle2;
                        break;
                    case 2:
                        texture = note_green;
                        angle = fred_line_right_rotationAngle3;
                        fredline_rect = fred_line_right_rectangle3;
                        break;
                    case 3:
                        texture = note_white;
                        angle = fred_line_right_rotationAngle4;
                        fredline_rect = fred_line_right_rectangle4;
                        break;
                    case 4:
                        texture = note_blue;
                        angle = fred_line_left_rotationAngle;
                        fredline_rect = fred_line_left_rectangle;
                        break;
                    case 5:
                        texture = note_blue;
                        angle = fred_line_left_rotationAngle2;
                        fredline_rect = fred_line_left_rectangle2;
                        break;
                    case 6:
                        texture = note_blue;
                        angle = fred_line_left_rotationAngle3;
                        fredline_rect = fred_line_left_rectangle3;
                        break;
                    case 7:
                        texture = note_blue;
                        angle = fred_line_left_rotationAngle4;
                        fredline_rect = fred_line_left_rectangle4;
                        break;
                    default:
                        break;
                } // switch statement

                // Fix position
                fredline_rect.Y -= 350;
                if (color[i] == 7)
                {
                    fredline_rect.X -= 10;
                }
                if (color[i] >= 0 && color[i] <= 3)
                {
                    fredline_rect.X -= 20;
                }

                // Fix Position for Arcade Machine
                if (_preferredBackBufferHeight >= 2560)
                {
                    fredline_rect.Y -= 700;
                    if (color[i] == 4)
                    {
                        fredline_rect.X += 5;
                    }
                    if (color[i] == 5)
                    {
                        fredline_rect.X -= 8;
                    }
                    if (color[i] == 6)
                    {
                        fredline_rect.X -= 10;
                    }
                    if (color[i] == 7)
                    {
                        fredline_rect.X -= 19;
                    }
                }

                // Make a note for each note and add it to the note list
                Note note = new(texture, ticks[i], length[i], lane, _preferredBackBufferWidth, _preferredBackBufferHeight,
                    note_width, note_height, time_between_notes[i], isMulti, false, multiCount, null)
                {
                    Position = fredline_rect,
                    fretLineRotationAngle = angle
                };
                notes.Add(note);

                if (note.isMultiNote && debug)
                {
                    Debug.WriteLine("NOTE " + (i + 1) + " IS MULTI");
                }

            } // for loop

        } // Make Notes method

        public void SetupMultiNoteDetection(List<Note> notes)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];

                if (note.isMultiNote)
                {
                    List<int> lanes = new List<int> { note.Lane };

                    int remainingMulti = note.howManyMulti;

                    for (int j = i + 1; j < notes.Count; j++)
                    {
                        Note nextNote = notes[j];

                        if (nextNote.isMultiNote && nextNote.Tick == note.Tick && remainingMulti > 0)
                        {
                            lanes.Add(nextNote.Lane);
                            remainingMulti--;
                            i++; // Skip subsequent notes in the multi-note
                        }

                        if (remainingMulti == 0)
                            break;
                    }

                    note.multiNoteLanes = lanes.ToArray();
                }
            }

            // DEBUGGING
            if (debug_note_detection)
            {
                int x = 1;
                foreach (Note note in notes)
                {
                    if (note.isMultiNote && note.multiNoteLanes != null)
                    {
                        Debug.WriteLine("NOTE: " + x);
                        foreach (int element in note.multiNoteLanes)
                        {
                            Debug.WriteLine("DEBUGGING SETUPNOTE DETECTION: " + element);
                        }
                    }
                    x++;
                }
            }
        }

        public void PlayBadNote()
        {
            Random random = new();
            int value = random.Next(1, 6); // generates a random value between x and y-1 (inclusive)
            switch (value)
            {
                case 1:
                    bad_note_hit.Play();
                    break;
                case 2:
                    bad_note_hit2.Play();
                    break;
                case 3:
                    bad_note_hit3.Play();
                    break;
                case 4:
                    bad_note_hit4.Play();
                    break;
                case 5:
                    bad_note_hit5.Play();
                    break;
                default:
                    bad_note_hit.Play();
                    break;
            } // switch statement

        } // Play Bad Note Method

    } // Devcade Hero state class

} // name space
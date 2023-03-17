﻿using DevcadeGame.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;
using Devcade;
using System.Diagnostics;
using System.Linq;
// HEAVILY MODIFIED VERSION OF Oyyou's MonoGame_Tutorials #13. All credit goes to Oyyou for the original code.
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
        // Attributes
        public static List<Component> _components;
        public List<Component> _main_menu_components;
        public List<Component> _settings_components;
        public List<Component> _empty_components;
        public List<Component> _difficulty_components;
        public List<Component> _player_select_components;
        public List<Component> _setlist_components;
        private readonly SoundEffect selectSound;
        private readonly SoundEffect selectSound2;
        private readonly SoundEffect selectSound3;
        private readonly SoundEffect backSound;
        private readonly SoundEffect backSound2;
        private readonly SoundEffect sliderUpSound;
        private readonly SoundEffect sliderDownSound;
        private readonly string musicType;
        private readonly string soundEffectType;
        private readonly string state_name; // Could be MenuState or MenuState1 (skipped cut-scene)
        private readonly Texture2D setlist_background;
        private readonly Texture2D main_menu_background;
        private readonly Song welcome_to_the_jungle;
        private readonly Song beat_drop_after_jungle; // Song that plays after mega mind intro (Welcome to the Jungle)
        private bool playWelcomeToTheJungle;
        public static int _gameID;
        public static int _difficultyID;
        private State DevcadeHero_State;
        private bool mediaPlayerKillSwitch;
        private bool keyPressed;
        private int currentButton;
        private int randomValue;

        // *************************************
        // ***** SETTER AND GETTER METHODS *****
        // *************************************

        // Variable Methods for Game ID and Difficulty ID
        public static void SetGameID(int gameID)
        {
            _gameID = gameID;
        }
        public static int GetGameID()
        {
            return _gameID;
        }
        public static void SetDifficultyID(int difficultyID)
        {
            _difficultyID = difficultyID;
        }
        public static int GetDifficultyID()
        {
            return _difficultyID;
        }

        // MenuState Constructor
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {
            // Attributes
            musicType = "music";
            soundEffectType = "effect";
            playWelcomeToTheJungle = false;
            mediaPlayerKillSwitch = false;
            state_name = _state_name;
            currentButton = 1;

            // ***** LOAD ASSETS *****
            // Load another background for the set list
            main_menu_background = _content.Load<Texture2D>("Menu_Assets/vertical background");
            setlist_background = _content.Load<Texture2D>("Menu_Assets/setlist_background");

            // Load Music for the menu
            welcome_to_the_jungle = _content.Load<Song>("Songs/welcome_to_the_jungle_PCM");
            beat_drop_after_jungle = _content.Load<Song>("Songs/megamind_after_jungle");

            // Load the Sound effects for the menu
            selectSound = _content.Load<SoundEffect>("Sound_Effects/UIConfirm");
            selectSound2 = _content.Load<SoundEffect>("Sound_Effects/UIConfirm2");
            selectSound3 = _content.Load<SoundEffect>("Sound_Effects/UIConfirm3");
            backSound = _content.Load<SoundEffect>("Sound_Effects/UICancel");
            backSound2 = _content.Load<SoundEffect>("Sound_Effects/UICancel2");
            sliderUpSound = _content.Load<SoundEffect>("Sound_Effects/VolumeUp");
            sliderDownSound = _content.Load<SoundEffect>("Sound_Effects/VolumeDown");

            // Subscribe to MediaStateChange Event, so things can happen when the song ends
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            // Load the buttons for the menu
            var buttonTexture = _content.Load<Texture2D>("Menu_Assets/button");
            var devcade_ButtonTexture = _content.Load<Texture2D>("Menu_Assets/devcade_button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var devcadeButtonFont = _content.Load<SpriteFont>("Fonts/SetList_Font");

            // Load the sliders for the menu
            var sliderTexture = _content.Load<Texture2D>("Menu_Assets/slider");
            var sliderThumbTexture = _content.Load<Texture2D>("Menu_Assets/slider_ball");

            // ***** ALL STARTING BUTTONS ARE DEFINED BELOW *****
            // These are all the things that the user can select in the menu
            // Each button has an on-click event
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

            // Player Buttons
            var SinglePlayerButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 780),
                Text = "         Single Player",
            };
            SinglePlayerButton.Click += SinglePlayerButton_Click;

            var MultiPlayerButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 840),
                Text = "         Multi Player",
            };
            MultiPlayerButton.Click += MultiPlayerButton_Click;

            // Difficulty Buttons
            var ExpertButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 840),
                Text = "         Expert",
            };
            ExpertButton.Click += ExpertButton_Click;
            var HardButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 780),
                Text = "         Hard",
            };
            HardButton.Click += HardButton_Click;
            var MediumButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 720),
                Text = "         Medium",
            };
            MediumButton.Click += MediumButton_Click;
            var EasyButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 660),
                Text = "         Easy",
            };
            EasyButton.Click += EasyButton_Click;

            // Set list buttons
            var Setlist_test = new Button(devcade_ButtonTexture, devcadeButtonFont)
            {
                Position = new Vector2(10, 100),
                Text = "Test Song",
                // Make the text go to the left
                textOffset = new Vector2(-50, 0),
                PenColour = Color.Yellow,
            };
            Setlist_test.Click += Setlist_TestButton_Click;
            var Setlist_BackButton = new Button(devcade_ButtonTexture, devcadeButtonFont)
            {
                Position = new Vector2(10, 900),
                Text = "Back",
                // Make the text go to the left
                textOffset = new Vector2(-50, 0),
                PenColour = Color.Yellow,
            };
            Setlist_BackButton.Click += SetListBackButton_Click;

            // ***** ALL SLIDERS DEFINED BELOW *****
            // MUSIC VOLUME SLIDER
            var MusicVolumeSliderButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 760),
                // Setting the text to the correct place above the slider
                textOffset = new Vector2(40, -40),
                Text = "  Music",
            };
            MusicVolumeSliderButton.Click += MusicVolumeSliderButton_Click;
            var MusicVolumeSlider = new Slider(sliderTexture, sliderThumbTexture, musicType)
            {
                Position = new Vector2(135, 785),
                BarColor = Color.White,
            };

            // SOUND EFFECT SLIDER
            var EffectVolumeSliderButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(70, 840),
                // Setting the text to the correct place above the slider
                textOffset = new Vector2(44, -40),
                Text = "Sound Effects",
            };
            EffectVolumeSliderButton.Click += EffectVolumeSliderButton_Click;
            var EffectVolumeSlider = new Slider(sliderTexture, sliderThumbTexture, soundEffectType)
            {
                Position = new Vector2(135, 865),
                BarColor = Color.White,
            };


            // ***** TYPES OF COMPONENTS *****
            // Change to these components to change the menu
            _empty_components = new List<Component>()
            {

            };
            _main_menu_components = new List<Component>()
            {
                careerGameButton,
                casualGameButton,
                settingsButton,
                quitGameButton,
            };
            _settings_components = new List<Component>()
            {
                MusicVolumeSliderButton,
                MusicVolumeSlider,
                EffectVolumeSliderButton,
                EffectVolumeSlider,
                BackButton,
            };
            _player_select_components = new List<Component>()
            {
                SinglePlayerButton,
                MultiPlayerButton,
                BackButton,
            };
            _difficulty_components = new List<Component>()
            {
                EasyButton,
                MediumButton,
                HardButton,
                ExpertButton,
                BackButton,
            };
            _setlist_components = new List<Component>()
            {
                Setlist_test,
                Setlist_BackButton,
            };

            // Using starting main menu component
            _components = _main_menu_components;

            // Put the mouse in the correct spot
            Mouse.SetPosition((int)careerGameButton.Position.X + 150, (int)careerGameButton.Position.Y + 20);

        } // MenuState Constructor

        // **********************************
        // ***** BUTTON ON-CLICK EVENTS *****
        // **********************************
        private void CareerButton_Click(object sender, EventArgs e)
        {
            // Career Mode ID = 0
            SetGameID(0);
            _components = _player_select_components;
            SelectSound().Play();
            //Game1.ChangeState(new GameState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "MenuState"));
        
        }

        private void CasualButton_Click(object sender, EventArgs e)
        {
            // Casual Mode ID = 1
            SetGameID(1);
            _components = _player_select_components;
            SelectSound().Play();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            _components = _settings_components;
            SelectSound().Play();
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _components = _main_menu_components;
            BackSound().Play();
        }

        private void Setlist_TestButton_Click(object sender, EventArgs e)
        {
            _components = _empty_components;
            SelectSound().Play();

            // Make the Devcade Hero State, the state name is the name of the song/chart file
            DevcadeHero_State = new DevcadeHeroState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "tester");

            // Stop the media player
            mediaPlayerKillSwitch = true;
            MediaPlayer.Stop();

            // Change State 
            Game1.ChangeState(DevcadeHero_State);
        }

        private void SetListBackButton_Click(object sender, EventArgs e)
        {
            // Set the main menu back to normal
            Game1.main_menu = main_menu_background;
            _components = _main_menu_components;
            BackSound().Play();
        }

        private void SinglePlayerButton_Click(object sender, EventArgs e)
        {
            // Enter the game differently depending on what game mode they chose before
            switch (_gameID)
            {
                // Career Mode
                case 0:
                    break;

                // Casual Mode
                case 1:
                    _components = _difficulty_components;
                    break;

                default: break;

            }
            SelectSound().Play();
        }
        private void MultiPlayerButton_Click(object sender, EventArgs e)
        {
            SelectSound().Play();
        }

        private void ExpertButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(3);

            SelectSound().Play();
        }

        private void HardButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(2);

            SelectSound().Play();
        }

        private void MediumButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(1);

            SelectSound().Play();
        }

        private void EasyButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(0);

            // Casual Mode
            if (_gameID == 1)
            {
                ChangeMenuBackground(setlist_background);
                _components = _setlist_components;
            }
            SelectSound().Play();
        }

        // *******************
        // ***** SLIDERS *****
        // *******************

        private void MusicVolumeSliderButton_Click(object sender, EventArgs e)
        {
            // Plays no sound since music is playing in the background
        }
        private void EffectVolumeSliderButton_Click(object sender, EventArgs e)
        {
            sliderUpSound.Play();
        }

        // *************************************
        // ***** Game1.cs Override Methods *****
        // *************************************
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            // Draw the main menu background
            spriteBatch.Draw(background, new Rectangle(0, 0, _preferredBackBufferWidth, _preferredBackBufferHeight),
                new Rectangle(0, 0, 1080, 2560), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            // Put the menu items on the screen
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }

            // Menu Controls
            DoMenuControls();

        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove the sprites if no longer needed
        }

        // OTHER METHODS

        private SoundEffect SelectSound()
        {
            Random random = new();
            randomValue = random.Next(1, 4); // generates a random value between x and y-1 (inclusive)
            switch (randomValue)
            {
                case 1:
                    return selectSound;
                case 2:
                    return selectSound2;
                case 3:
                    return selectSound3;
                default: 
                    return selectSound;
            }
        }

        private SoundEffect BackSound()
        {
            Random random = new();
            randomValue = random.Next(1, 3); // generates a random value between x and y-1 (inclusive)
            switch (randomValue)
            {
                case 1:
                    return backSound;
                case 2:
                    return backSound2;
                default:
                    return backSound;
            }
        }
        
        private static void ChangeMenuBackground(Texture2D background)
        {
            Game1.main_menu = background;
        }

        // This is called after the intro state and when a MenuState object is made
        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            // Kill switch for this when we don't want stuff playing anymore
            if (mediaPlayerKillSwitch)
            {
                playWelcomeToTheJungle = false;
                MediaPlayer.IsRepeating = false;
                MediaPlayer.Stop();
            }
            else
            {
                // If the person skipped the cut-scene
                if ((MediaPlayer.State == MediaState.Stopped) && state_name.Equals("MenuState1") && playWelcomeToTheJungle == false)
                {
                    MediaPlayer.Play(welcome_to_the_jungle);
                    playWelcomeToTheJungle = true;
                }

                // If they watched the entire cut-scene
                else
                {
                    // Then after that play the regular one
                    if ((MediaPlayer.State == MediaState.Stopped) && playWelcomeToTheJungle == true)
                    {
                        MediaPlayer.Play(welcome_to_the_jungle);
                    }
                    // First 'welcome to the jungle' when beat drops
                    if (state_name.Equals("MenuState_beat_drop") && playWelcomeToTheJungle == false)
                    {
                        MediaPlayer.Play(beat_drop_after_jungle);
                    }

                } // Else statement

            } // Kill switch else statement

        } // MediaPlayer_MediaStateChanged method

        // Keyboard and Arcade controls for the menu screen
        private void DoMenuControls()
        {
            // Make keyboard state so cursor only moves once when pressed
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.GetPressedKeys().Length > 0 || IntroState.DevcadeButtonCheck() == true)
            {
                // Get number of elements so we can move the cursor up and down and know the limits
                int numOfElements = _components.Count;

                // For moving the cursor up and down
                // For current button the highest button is 1, when you move down the screen the number goes up
                if ((currentKeyboardState.IsKeyDown(Keys.Up) || Input.GetButton(1, Input.ArcadeButtons.StickUp) 
                    || Input.GetButton(2, Input.ArcadeButtons.StickUp)) && !keyPressed && currentButton > 1)
                {
                    // Get the next button pos (up) and set mouse to it
                    currentButton -= 1;
                    int count = 0;
                    foreach (Button btn in _components.Cast<Button>())
                    {
                        if (count == currentButton - 1)
                        {
                            string cords = btn.Position.ToString();
                            string[] parts = cords.Replace("{", "").Replace("}", "").Split(' ');

                            int x = int.Parse(parts[0].Split(':')[1]);
                            int y = int.Parse(parts[1].Split(':')[1]);

                            Mouse.SetPosition(x, y);
                            break;
                        }
                        count++;
                    } // foreach loop

                    keyPressed = true;
                }
                if ((currentKeyboardState.IsKeyDown(Keys.Down) || Input.GetButton(1, Input.ArcadeButtons.StickDown)
                    || Input.GetButton(2, Input.ArcadeButtons.StickDown)) && !keyPressed && currentButton < numOfElements)
                {
                    // Get the next button pos (below) and set mouse to it
                    currentButton += 1;
                    int count = 0;
                    foreach (Button btn in _components.Cast<Button>())
                    {
                        if (count == currentButton - 1)
                        {
                            string cords = btn.Position.ToString();
                            string[] parts = cords.Replace("{", "").Replace("}", "").Split(' ');

                            int x = int.Parse(parts[0].Split(':')[1]);
                            int y = int.Parse(parts[1].Split(':')[1]);

                            Mouse.SetPosition(x, y);
                            break;
                        }
                        count++;
                    } // foreach loop
                    keyPressed = true;
                }

                // Mouse click implemented in Button.cs
                if ((currentKeyboardState.IsKeyDown(Keys.Enter) || Input.GetButton(1, Input.ArcadeButtons.A1)
                    || Input.GetButton(2, Input.ArcadeButtons.A1)) && !keyPressed)
                {
                    // Go through each button and click the one we are on
                    int count = 0;
                    foreach (Button btn in _components.Cast<Button>())
                    {
                        if (count == currentButton - 1)
                        {
                            btn.EnterButtonHit();
                            break;
                        }
                        count++;
                    } // foreach loop

                    // Extract the cords from the button position and put the cursor on the first button in the components
                    if (_components.FirstOrDefault(x => x is Button) is Button btn2)
                    {
                        string cords = btn2.Position.ToString();

                        string[] parts = cords.Replace("{", "").Replace("}", "").Split(' ');

                        int x = int.Parse(parts[0].Split(':')[1]);
                        int y = int.Parse(parts[1].Split(':')[1]);

                        Mouse.SetPosition(x, y);

                        // Reset the button with the new menu components
                        currentButton = 1;
                    }

                    keyPressed = true;

                } // If Statement

                // For going back to the original menu

            }
            // Set to false if not being pressed
            else if (keyPressed)
            {
                keyPressed = false;
            }

        } // Do Menu Controls

    } // Public class MenuState end
} // Name space end

using DevcadeGame.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
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
        private readonly SoundEffect backSound;
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

        // This is called after the intro state
        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            
            // Kill switch for this when we don't want stuff playing anymore
            if (mediaPlayerKillSwitch)
            {
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
                    // First 'welcome to the jungle' when beat drops
                    if ((MediaPlayer.State == MediaState.Stopped) && state_name.Equals("MenuState_beat_drop") && playWelcomeToTheJungle == false)
                    {
                        MediaPlayer.Play(beat_drop_after_jungle);
                        playWelcomeToTheJungle = true;
                    }
                    // Then after that play the regular one
                    if ((MediaPlayer.State == MediaState.Stopped) && playWelcomeToTheJungle == true)
                    {
                        MediaPlayer.Play(welcome_to_the_jungle);
                    }

                } // Else statement

            } // Kill switch else statement

        } // MediaPlayer_MediaStateChanged method

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

            // ***** LOAD ASSETS *****
            // Load another background for the set list
            main_menu_background = _content.Load<Texture2D>("Menu_Assets/vertical background");
            setlist_background = _content.Load<Texture2D>("Menu_Assets/setlist_background");

            // Load Music for the menu
            welcome_to_the_jungle = _content.Load<Song>("Songs/welcome_to_the_jungle_PCM");
            beat_drop_after_jungle = _content.Load<Song>("Songs/megamind_after_jungle");

            // Load the Sound effects for the menu
            selectSound = _content.Load<SoundEffect>("Sound_Effects/UIConfirm");
            backSound = _content.Load<SoundEffect>("Sound_Effects/UICancel");
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

        } // MenuState Constructor


        // Game1.cs Override Methods
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Texture2D background)
        {
            // Draw the main menu background
            spriteBatch.Draw(background, new Rectangle(0, 0, _preferredBackBufferWidth, _preferredBackBufferHeight),
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



        // ***** BUTTON ON-CLICK EVENTS *****
        private void CareerButton_Click(object sender, EventArgs e)
        {
            // Career Mode ID = 0
            SetGameID(0);
            _components = _player_select_components;
            selectSound.Play();
            //Game1.ChangeState(new GameState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "MenuState"));
        
        }

        private void CasualButton_Click(object sender, EventArgs e)
        {
            // Casual Mode ID = 1
            SetGameID(1);
            _components = _player_select_components;
            selectSound.Play();
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            _components = _settings_components;
            selectSound.Play();
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _components = _main_menu_components;
            backSound.Play();
        }

        private void Setlist_TestButton_Click(object sender, EventArgs e)
        {
            _components = _empty_components;
            selectSound.Play();

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
            backSound.Play();
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
            selectSound.Play();
        }
        private void MultiPlayerButton_Click(object sender, EventArgs e)
        {
            selectSound.Play();
        }

        private void ExpertButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(3);

            selectSound.Play();
        }

        private void HardButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(2);

            selectSound.Play();
        }

        private void MediumButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(1);

            selectSound.Play();
        }

        private void EasyButton_Click(object sender, EventArgs e)
        {
            // Set difficulty
            SetDifficultyID(0);

            // Casual Mode
            if (_gameID == 1)
            {
                ChangeMenuBackground();
                _components = _setlist_components;
            }
            selectSound.Play();
        }

        private void MusicVolumeSliderButton_Click(object sender, EventArgs e)
        {
            // Plays no sound since music is playing in the background
        }
        private void EffectVolumeSliderButton_Click(object sender, EventArgs e)
        {
            sliderUpSound.Play();
        }

        private void ChangeMenuBackground()
        {
            Game1.main_menu = setlist_background;
        }

    } // Public class MenuState end
} // Name space end

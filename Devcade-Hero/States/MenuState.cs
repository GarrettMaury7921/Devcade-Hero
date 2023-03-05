using DevcadeGame.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;
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
        private readonly SoundEffect selectSound;
        private readonly SoundEffect backSound;
        private readonly SoundEffect sliderUpSound;
        private readonly SoundEffect sliderDownSound;
        private readonly string musicType;
        private readonly string soundEffectType;
        private readonly string state_name;
        private int gameID;
        private Song welcome_to_the_jungle;
        // Song that plays after mega mind intro (Welcome to the Jungle)
        private Song megamind_after_jungle;
        private bool playWelcomeToTheJungle;

        private void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            // First play mega mind 'welcome to the jungle'
            if (MediaPlayer.State == MediaState.Stopped)
            {
                if (!playWelcomeToTheJungle)
                {
                    MediaPlayer.Play(megamind_after_jungle);
                    playWelcomeToTheJungle = true;
                }
            }
            // Then after that play the regular one
            if ((MediaPlayer.State == MediaState.Stopped) && playWelcomeToTheJungle == true)
            {
                MediaPlayer.Play(welcome_to_the_jungle);
            }
        }

        // Variable Methods for Game ID
        public void setGameID(int gameID)
        {
            this.gameID = gameID;
        }
        public int getGameID()
        {
            return gameID;
        }

        // MenuState Constructor
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, int PreferredBackBufferWidth, int PreferredBackBufferHeight, ContentManager content, string _state_name) :
            base(game, graphicsDevice, PreferredBackBufferWidth, PreferredBackBufferHeight, content, _state_name)
        {
            // Attributes
            musicType = "music";
            soundEffectType = "effect";
            state_name = _state_name;
            playWelcomeToTheJungle = false;

            // ***** LOAD ASSETS *****
            // Load Music for the menu
            welcome_to_the_jungle = _content.Load<Song>("Songs/welcome_to_the_jungle_PCM");
            megamind_after_jungle = _content.Load<Song>("Songs/megamind_after_jungle");

            // Load the Sound effects for the menu
            selectSound = _content.Load<SoundEffect>("Sound_Effects/UIConfirm");
            backSound = _content.Load<SoundEffect>("Sound_Effects/UICancel");
            sliderUpSound = _content.Load<SoundEffect>("Sound_Effects/VolumeUp");
            sliderDownSound = _content.Load<SoundEffect>("Sound_Effects/VolumeDown");

            // Subscribe to MediaStateChange Event, so things can happen when the song ends
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            // Load the buttons for the menu
            var buttonTexture = _content.Load<Texture2D>("Menu_Assets/button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

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
            setGameID(0);
            _components = _player_select_components;
            selectSound.Play();
            //Game1.ChangeState(new GameState(_game, _graphicsDevice, _preferredBackBufferWidth, _preferredBackBufferHeight, _content, "MenuState"));
        
        }

        private void CasualButton_Click(object sender, EventArgs e)
        {
            // Casual Mode ID = 1
            setGameID(1);
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

        private void SinglePlayerButton_Click(object sender, EventArgs e)
        {
            // Enter the game differently depending on what game mode they chose before
            switch (gameID)
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
            selectSound.Play();
        }

        private void HardButton_Click(object sender, EventArgs e)
        {
            selectSound.Play();
        }

        private void MediumButton_Click(object sender, EventArgs e)
        {
            selectSound.Play();
        }

        private void EasyButton_Click(object sender, EventArgs e)
        {
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

    } // Public class MenuState end
} // Name space end

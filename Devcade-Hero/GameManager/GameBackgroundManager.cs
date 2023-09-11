using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

namespace DevcadeHero.GameManager
{
    /* 
    Class GameBackgroundManager:
        GameBackgroundManager Constructor
        @ VideoChooser Method
        @ SongChooser Method
        @ BackgroundChooser Method
    */
    public class GameBackgroundManager
    {

        // Attributes
        private Texture2D background;
        private readonly string song_name;
        private string videoName;
        private Song song;
        public float delay;

        public GameBackgroundManager(string songName)
        {
            song_name = songName;
        }

        public string VideoChooser(string songName)
        {
            // Find the correct background video
            switch (songName)
            {
                case "Linux_Startup_Song":
                    videoName = null;
                    break;

                case "Kalimba (Ninja Tuna)":
                    videoName = null;
                    break;

                case "Wash Your Dishes":
                    videoName = null;
                    break;

                default:
                    videoName = null;
                    break;
            }
            return videoName;
        } // VideoChooser Method
        public Song SongChooser(ContentManager content, string songName)
        {
            // Find the correct song

            // Delay: 1.52 seconds subtracted from first note

            switch (songName)
            {
                case "Linux_Startup_Song":
                    song = content.Load<Song>("Songs/Linux_Startup_Song");
                    delay = 2.23f;
                    break;

                case "Kalimba (Ninja Tuna)":
                    song = content.Load<Song>("Songs/Kalimba (Ninja Tuna)");
                    delay = 2.1f;
                    break;

                case "wash_your_dishes":
                    song = content.Load<Song>("Songs/clean_your_dishes");
                    delay = 0.35f;
                    break;

                // default is not supported
                default:
                    Debug.WriteLine("A SONG WAS NOT FOUND FOR THE SELECTED SONG!!!");
                    Environment.Exit(0);
                    break;
            }

            return song;
        } // Song chooser method
        public Texture2D BackgroundChooser(ContentManager content, string songName)
        {
            // Select a background for whatever song there is
            switch (songName)
            {
                case "Linux_Startup_Song":
                    background = content.Load<Texture2D>("Game_Assets/linux_desktop");
                    break;

                case "Kalimba (Ninja Tuna)":
                    background = content.Load<Texture2D>("Game_Assets/ninja_tuna");
                    break;

                // default is supported
                default:
                    background = content.Load<Texture2D>("Game_Assets/outside");
                    break;
            }

            return background;
        } // BackgroundChooser Method

    } // GameBackgroundManager class

} // name space

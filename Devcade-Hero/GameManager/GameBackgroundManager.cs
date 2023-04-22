using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

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
                case "tester":
                    videoName = null;
                    break;
                case "Kalimba (Ninja Tuna)":
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
            switch (songName)
            {
                case "tester":
                    song = content.Load<Song>("Songs/tester");
                    delay = 0;
                    break;

                case "Kalimba (Ninja Tuna)":
                    song = content.Load<Song>("Songs/Kalimba (Ninja Tuna)");
                    delay = 1.7f;
                    break;

                default:
                    song = null;
                    break;
            }

            return song;
        } // Song chooser method
        public Texture2D BackgroundChooser(ContentManager content, string songName)
        {
            // Select a background for whatever song there is
            switch (songName)
            {
                case "tester":
                    background = content.Load<Texture2D>("Game_Assets/outside");
                    break;
                case "Kalimba (Ninja Tuna)":
                    background = content.Load<Texture2D>("Game_Assets/outside");
                    break;
                // default is null
                default:
                    background = null;
                    break;
            }

            return background;
        } // BackgroundChooser Method

    } // GameBackgroundManager class

} // name space

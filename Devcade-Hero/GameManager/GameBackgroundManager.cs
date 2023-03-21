using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace DevcadeGame.GameManager
{
    public class GameBackgroundManager
    {

        // Attributes
        private Texture2D background;
        private readonly string song_name;
        private string videoName;
        private Song song;

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
                    background = null;
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

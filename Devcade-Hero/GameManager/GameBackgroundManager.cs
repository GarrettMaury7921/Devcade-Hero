using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DevcadeGame.GameManager
{
    public class GameBackgroundManager
    {

        // Attributes
        private Texture2D background;
        private string song_name;

        public GameBackgroundManager(string songName)
        {
            song_name = songName;

        }

        public Texture2D BackgroundChooser(ContentManager content, string songName)
        {
            // Select a background for whatever song there is
            switch (songName)
            {
                case "tester":
                    background = content.Load<Texture2D>("Menu_Assets/vertical background");
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

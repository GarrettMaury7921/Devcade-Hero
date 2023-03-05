using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DevcadeGame.Sounds
{

    /* 
    Class MenuSounds:
        MenuSounds Constructor: Initializes the volume for the game
            - this is called from the initialization stage in game1.cs
        @ InitializeSound Method
    */

    public class MenuSounds
    {

        public MenuSounds() {
            InitializeSound();
        }
        
        public void InitializeSound()
        {
            // Starting Volume
            MediaPlayer.Volume = 0.40f;
            SoundEffect.MasterVolume = 0.09f;

        }

        public void setMediaVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public void setSoundEffectVolume(float volume) 
        { 
            SoundEffect.MasterVolume = volume;
        }

    }
}

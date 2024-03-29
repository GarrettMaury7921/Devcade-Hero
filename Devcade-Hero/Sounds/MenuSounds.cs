﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DevcadeHero.Sounds
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
            MediaPlayer.Volume = 0.60f;
            SoundEffect.MasterVolume = 0.1f;

        }

        public static void setMediaVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public static void setSoundEffectVolume(float volume) 
        { 
            SoundEffect.MasterVolume = volume;
        }

    }
}

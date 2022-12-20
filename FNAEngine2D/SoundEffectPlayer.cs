using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Player for sound effect
    /// </summary>
    public static class SoundEffectPlayer
    {
        /// <summary>
        /// Play a sfx
        /// </summary>
        public static void Play(string assetName)
        {

            GameHost.GetContent<SoundEffect>(assetName).Data.Play();

        }

        /// <summary>
        /// Play a sfx
        /// </summary>
        public static void Play(string assetName, float volume)
        {

            GameHost.GetContent<SoundEffect>(assetName).Data.Play(volume, 0.0f, 0.0f);

        }

    }
}

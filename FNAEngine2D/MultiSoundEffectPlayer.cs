using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SharpFont.Cache;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Multi sound effect player render from a enum
    /// </summary>
    public class MultiSoundEffectPlayer<T> : GameObject where T : System.Enum
    {
        /// <summary>
        /// Textures
        /// </summary>
        private Dictionary<T, Content<SoundEffect>> _sounds = new Dictionary<T, Content<SoundEffect>>();


        /// <summary>
        /// Sfx folder
        /// </summary>
        public string SfxFolder { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public float Volume { get; set; } = 1f;

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiSoundEffectPlayer(string sfxFolder)
        {
            this.SfxFolder = sfxFolder;

        }

        /// <summary>
        /// Play a sound
        /// </summary>
        public void Play(T sound)
        {
            _sounds[sound].Data.Play(this.Volume, 0f, 0f);
        }

        /// <summary>
        /// Load
        /// </summary>
        protected override void Load()
        {
            LoadSounds();
            
        }

        /// <summary>
        /// Load textures
        /// </summary>
        private void LoadSounds()
        {
            foreach (var soundValue in Enum.GetValues(typeof(T)))
            {
                T soundEnum = (T)soundValue;

                string assetName = Path.Combine(this.SfxFolder, soundEnum.ToString());

                Content<SoundEffect> sfx = GetContent<SoundEffect>(assetName);

                _sounds[soundEnum] = sfx;
            }

        }

    }
}

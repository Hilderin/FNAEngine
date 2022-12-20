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
    public class SoundEffectPlayer: GameObject
    {
        /// <summary>
        /// Currentply play...
        /// </summary>
        private Content<SoundEffect> _currentlyPlaying = null;

        /// <summary>
        /// Currently playing
        /// </summary>
        private SoundEffectInstance _currentSfxInstance = null;

        /// <summary>
        /// Time between the last start
        /// </summary>
        private float _elapedStartSeconds = 0;

        /// <summary>
        /// Volume
        /// </summary>
        public float Volume { get; set; }

        /// <summary>
        /// Minimum rate for playing the sound
        /// </summary>
        public float MinimumRateSeconds = 0f;

        /// <summary>
        /// IsPlaying
        /// </summary>
        public bool IsPlaying
        {
            get { return _currentSfxInstance != null && _currentSfxInstance.State == SoundState.Playing; }
        }

        /// <summary>
        /// Get a sfx content
        /// </summary>
        public Content<SoundEffect> GetContent(string assetName)
        {
            return GameHost.GetContent<SoundEffect>(assetName);
        }


        /// <summary>
        /// Play a sfx
        /// </summary>
        public void Play(Content<SoundEffect> sfx)
        {
            //No sound to play?
            if (sfx == null)
                return;


            if (_currentSfxInstance != null && _currentlyPlaying == sfx && _currentSfxInstance.State == SoundState.Playing)
                //Already playing...
                return;


            if (_elapedStartSeconds < this.MinimumRateSeconds)
                //Still not the time...
                return;


            if (_currentSfxInstance != null)
            {
                //Stopping...
                _currentSfxInstance.Dispose();
            }

            //Start playing...
            _currentlyPlaying = sfx;
            _currentSfxInstance = _currentlyPlaying.Data.CreateInstance();
            _currentSfxInstance.Volume = this.Volume;
            _currentSfxInstance.Play();

            _elapedStartSeconds = 0f;

        }

        /// <summary>
        /// Play a sfx
        /// </summary>
        public void Stop()
        {
            if (_currentlyPlaying == null)
                return;

            if (_currentSfxInstance != null)
            {
                //Stopping...
                _currentSfxInstance.Dispose();
                _currentSfxInstance = null;
            }

            _currentlyPlaying = null;
        }


        /// <summary>
        /// Update each frame
        /// </summary>
        public override void Update()
        {
            _elapedStartSeconds += GameHost.ElapsedGameTimeSeconds;
        }

        /// <summary>
        /// Play 
        /// </summary>
        public static void PlayStatic(string assetName)
        {
            GameHost.GetContent<SoundEffect>(assetName).Data.Play();
        }

        /// <summary>
        /// Play 
        /// </summary>
        public static void PlayStatic(Content<SoundEffect> sfx)
        {
            sfx.Data.Play();
        }

        /// <summary>
        /// Play 
        /// </summary>
        public static void PlayStatic(string assetName, float volume)
        {
            GameHost.GetContent<SoundEffect>(assetName).Data.Play(volume, 0f, 0f);
        }

    }
}

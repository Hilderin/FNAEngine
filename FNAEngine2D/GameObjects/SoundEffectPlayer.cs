using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D.GameObjects
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
        public float Volume { get; set; } = 1f;

        /// <summary>
        /// Minimum rate for playing the sound
        /// </summary>
        public float MinimumRateSeconds { get; set; } = 0f;

        /// <summary>
        /// Allow multiple sfx at the same time
        /// </summary>
        public bool AllowMultiple { get; set; } = false;

        /// <summary>
        /// IsPlaying
        /// </summary>
        public bool IsPlaying
        {
            get { return _currentSfxInstance != null && _currentSfxInstance.State == SoundState.Playing; }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public SoundEffectPlayer()
        {

        }


        /// <summary>
        /// Update each frame
        /// </summary>
        protected override void Update()
        {
            if (!AllowMultiple)
                _elapedStartSeconds += this.ElapsedGameTimeSeconds;
        }


        /// <summary>
        /// Get a sfx content
        /// </summary>
        public Content<SoundEffect> GetContent(string assetName)
        {
            return GetContent<SoundEffect>(assetName);
        }


        /// <summary>
        /// Play a sfx
        /// </summary>
        public void Play(Content<SoundEffect> sfx)
        {
            //No sound to play?
            if (sfx == null)
                return;

            if (AllowMultiple)
            {
                //Allowing multiple at the same time?
                sfx.Data.Play(this.Volume, 0f, 0f);
            }
            else
            {
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

        ///// <summary>
        ///// Play 
        ///// </summary>
        //public static void PlayStatic(string assetName)
        //{
        //    GetContent<SoundEffect>(assetName).Data.Play();
        //}

        ///// <summary>
        ///// Play 
        ///// </summary>
        //public static void PlayStatic(Content<SoundEffect> sfx)
        //{
        //    sfx.Data.Play();
        //}

        ///// <summary>
        ///// Play 
        ///// </summary>
        //public static void PlayStatic(string assetName, float volume)
        //{
        //    GetContent<SoundEffect>(assetName).Data.Play(volume, 0f, 0f);
        //}

    }
}

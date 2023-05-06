using UnityEngine;
using System;

namespace UniversalAudioSystem
{
    [Serializable]
    public class AudioTrack
    {
        #region Fields

        [SerializeField]
        private AudioLabel label;
        [SerializeField]
        private SoundElement audioElement;

        #endregion

        #region Propeties

        public AudioLabel Label
        {
            get => label;
            private set => label = value;
        }

        public SoundElement AudioElement
        {
            get => audioElement;
            private set => audioElement = value;
        }

        #endregion

        #region Methods

        public AudioTrack(SoundElement audio, AudioLabel label)
        {
            AudioElement = audio;
            Label = label;
        }

        public AudioTrack(SoundElement audio)
        {
            AudioElement = audio;
        }

        public bool IsTrackEqual(AudioLabel Label)
        {
            if (Label == label)
            {
                return true;
            }

            return false;
        }

        public void ResetAudio()
        {
            StopAudio();
            PlayAudio();
        }

        public void StopAudio()
        {
            AudioElement.StopAudio();
        }

        public void PlayAudio()
        {
            AudioElement.PlayOneShotAudio();
        }

        public void DestroyAudio()
        {
            AudioElement.DestroyAudio();
        }

        #endregion
    }
}


using UnityEngine;
using System;

namespace UniversalAudioSystem
{
    [Serializable]
    public class AudioAmbientTrack
    {
        #region Fields

        [SerializeField]
        private int sceneId;
        [SerializeField]
        private SoundElement audioElement;

        #endregion

        #region Propeties

        public SoundElement AudioElement
        {
            get => audioElement;
            private set => audioElement = value;
        }

        public int SceneId
        {
            get => sceneId;
            private set => sceneId = value;
        }

        #endregion

        #region Methods

        public AudioAmbientTrack(SoundElement audio, int sceneId)
        {
            AudioElement = audio;
            SceneId = sceneId;
        }

        public bool IsTrackEqual(int sceneId)
        {
            if (SceneId == sceneId)
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

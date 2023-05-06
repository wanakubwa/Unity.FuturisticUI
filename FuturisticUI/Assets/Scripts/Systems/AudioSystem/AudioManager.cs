using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Game.Scripts.Generic;

namespace UniversalAudioSystem
{
    public class AudioManager : SingletonBase<AudioManager>
    {
        #region Fields

        [NonSerialized]
        private AudioTrack currentSoundTrack = null;
        [NonSerialized]
        private AudioAmbientTrack currentAmbientTrack = null;

        [Space(10)]
        [SerializeField]
        private float volume = 0;
        [SerializeField]
        private float maxVolume = 1f;
        [SerializeField]
        private bool isAudioMute = false;

        #endregion

        #region Propeties

        public event Action<float> OnVolumeChanged = delegate { };

        public AudioTrack CurrentSoundTrack
        {
            get => currentSoundTrack;
            private set => currentSoundTrack = value;
        }

        public AudioAmbientTrack CurrentAmbientTrack
        {
            get => currentAmbientTrack;
            private set => currentAmbientTrack = value;
        }

        public float Volume
        {
            get => volume;
            private set => volume = value;
        }

        public float MaxVolume
        {
            get => maxVolume;
            private set => maxVolume = value;
        }

        public bool IsAudioMute
        {
            get => isAudioMute;
            private set => isAudioMute = value;
        }

        // Variables.
        private Dictionary<AudioLabel, AudioTrack> CurrentSounds { get; set; } = new Dictionary<AudioLabel, AudioTrack>();
        private AudioContainerSettings AudioContainer { get; set; }

        #endregion

        #region Methods

        protected void Awake()
        {
            AudioContainer = AudioContainerSettings.Instance;
        }

        public void SetAudioMute(bool isMuteAudio)
        {
            IsAudioMute = isMuteAudio;
            if (IsAudioMute == true)
            {
                SetVolume(0f);
            }
            else
            {
                SetVolume(MaxVolume);
            }
        }

        public void SetVolume(float value)
        {
            Volume = Mathf.Clamp(value, 0f, 1f);
            OnVolumeChanged(Volume);
        }

        public void ToggleIsAudioMute()
        {
            SetAudioMute(!IsAudioMute);
        }

        public void PlayAudioSoundByLabel(AudioLabel label)
        {
            SoundElement audioElement = AudioContainer.GetAudioElementByLabel(label);
            if (audioElement != null)
            {
                if (audioElement.IsExclusive == false)
                {
                    PlaySoundCommon(audioElement, label);
                }
                else
                {
                    PlaySoundExclusive(audioElement, label);
                }
            }
        }

        public void PlayAmbientSoundBySceneId(int sceneId)
        {
            SoundElement audioElement = AudioContainer.GetAudioElementBySceneId(sceneId);
            if (audioElement != null)
            {
                PlayAmbientSoundTrack(audioElement, sceneId);
            }
        }

        protected void OnEnable()
        {
            PlayAmbientSoundBySceneId(SceneManager.GetActiveScene().buildIndex);
        }

        private void PlayAmbientSoundTrack(SoundElement audio, int sceneId)
        {
            if (CurrentAmbientTrack != null)
            {
                if (CurrentAmbientTrack.IsTrackEqual(sceneId) == true)
                {
                    return;
                }

                CurrentAmbientTrack.DestroyAudio();
            }

            SoundElement audioElement = Instantiate(audio, transform);
            CurrentAmbientTrack = new AudioAmbientTrack(audioElement, sceneId);
        }

        private void PlaySoundExclusive(SoundElement audioElement, AudioLabel label)
        {
            if (CurrentSounds.TryGetValue(label, out AudioTrack track) == true)
            {
                if (track.IsTrackEqual(label) == true)
                {
                    track.ResetAudio();
                    return;
                }

                track.DestroyAudio();
                CurrentSounds.Remove(label);
            }

            SoundElement newAudioElement = SpawnSoundElement(audioElement, label.ToString());
            AudioTrack audioTrack = new AudioTrack(newAudioElement, label);
            CurrentSounds.Add(label, audioTrack);
        }

        private void PlaySoundCommon(SoundElement audioElement, AudioLabel label)
        {
            if (CurrentSoundTrack != null)
            {
                if (CurrentSoundTrack.IsTrackEqual(label) == true)
                {
                    CurrentSoundTrack.ResetAudio();
                    return;
                }

                CurrentSoundTrack.DestroyAudio();
            }

            SoundElement newAudioElement = SpawnSoundElement(audioElement);
            CurrentSoundTrack = new AudioTrack(newAudioElement, label);
        }

        private SoundElement SpawnSoundElement(SoundElement audioElement, string prefix = null)
        {
            SoundElement newAudioElement = Instantiate(audioElement, transform);
            newAudioElement.name = (prefix + audioElement.name);

            return newAudioElement;
        }

        #endregion
    }
}
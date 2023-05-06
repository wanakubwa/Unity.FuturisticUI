using UnityEngine;

namespace UniversalAudioSystem
{
    public class UIAudioTrigger : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AudioLabel label;

        #endregion

        #region Methods

        public void PlayAudio()
        {
            AudioManager.Instance?.PlayAudioSoundByLabel(label);
        }

        #endregion
    }
}
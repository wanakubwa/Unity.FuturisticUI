using UnityEngine;

namespace UniversalAudioSystem
{
    public class UITwoStateAudioTrigger : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AudioLabel onLabel;
        [SerializeField]
        private AudioLabel offLabel;

        #endregion

        #region Methods

        public void PlayAudio(bool isOn)
        {
            AudioLabel label = isOn == true ? onLabel : offLabel;
            AudioManager.Instance?.PlayAudioSoundByLabel(label);
        }

        #endregion
    }
}
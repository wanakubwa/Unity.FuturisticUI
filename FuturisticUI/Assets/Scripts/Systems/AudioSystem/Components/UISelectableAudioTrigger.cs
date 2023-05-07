using UnityEngine;
using UnityEngine.UI;

namespace UniversalAudioSystem
{
    public class UISelectableAudioTrigger : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AudioLabel interactableAudiolabel;
        [SerializeField]
        private AudioLabel noInteractableAudiolabel;

        #endregion

        #region Methods

        public void PlayAudio(Selectable source)
        {
            AudioLabel label = source.interactable == true ? interactableAudiolabel : noInteractableAudiolabel;
            AudioManager.Instance?.PlayAudioSoundByLabel(label);
        }

        #endregion
    }
}
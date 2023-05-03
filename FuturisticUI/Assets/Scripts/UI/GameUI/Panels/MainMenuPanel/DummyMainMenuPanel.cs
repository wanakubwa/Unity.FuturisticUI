using UnityEngine;
using UniversalUISystem;

namespace Game.UI
{
    public class DummyMainMenuPanel : MonoBehaviour
    {
        #region Methods

        public void OnSettingsSelected()
        {
            PopUpManager.Instance.ShowSettingsPopUp();
        }

        public void OnDummyButtonSelected(UISelectable selectable)
        {
            Debug.Log("OnPressedButton: " + selectable.name);
        }

        #endregion
    }
}
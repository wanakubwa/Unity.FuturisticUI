using System.Linq;
using UnityEngine;

namespace UniversalUISystem
{
    public class UIInputSubController : MonoBehaviour
    {
        #region Propeties

        private UIInputController RootController { get; set; }
        private UISelectable[] PanelSelectables { get; set; }

        // Defaults.
        private UISelectable DefaultSelectable { get; set; } = null;

        #endregion

        #region Methods

        public void SetFocus()
        {
            RootController.FocusButton(DefaultSelectable);
        }

        private void Awake()
        {
            // Moze zostac zastapione DI lub singletonem.
            RootController = FindObjectOfType<UIInputController>();

            PanelSelectables = GetComponentsInChildren<UISelectable>();
            DefaultSelectable = PanelSelectables.FirstOrDefault();
        }

        private void Start()
        {
            RootController.RegisterSubController(this);
        }

        private void OnDestroy()
        {
            RootController.UnregisterSubController(this);
        }

        #endregion
    }
}

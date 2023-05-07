using System.Linq;
using UnityEngine;

namespace UniversalUISystem
{
    public class UIInputSubController : MonoBehaviour
    {
        #region Propeties

        private UIInputController RootController { get; set; }
        public UISelectable[] PanelSelectables { get; private set; }

        // Defaults.
        public UISelectable DefaultSelectable { get; private set; } = null;

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
            for (int i = 0; i < PanelSelectables.Length; i++)
            {
                PanelSelectables[i].Initialize(RootController);
            }
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

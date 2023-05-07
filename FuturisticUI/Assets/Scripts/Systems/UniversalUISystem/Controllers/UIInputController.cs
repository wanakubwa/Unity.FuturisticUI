using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniversalUISystem
{
    public class UIInputController : MonoBehaviour
    {
        #region Propeties

        private UISelectable[] SelectableElements { get; set; }
        private EventSystem CurrentEventSystem { get; set; }
        
        // Subcontrollers.
        private UIInputSubController CurrentSubcontroller { get; set; } = null;

        // Variables.
        private UISelectable DefaultSelectableElement { get; set; }
        private UISelectable LastSelectedElement { get; set; } = null;

        // Accessors.
        private UISelectable DefaultSelectable { get => CurrentSubcontroller != null ? CurrentSubcontroller.DefaultSelectable : DefaultSelectableElement; }

        #endregion

        #region Methods

        public void FocusButton(Selectable target, BaseEventData eventData = null, bool isDelay = false)
        {
            if(target == null) { return; }

            if(isDelay == false)
            {
                target.Select();
                target.OnSelect(eventData);
            }
            else
            {
                FocusButtonWithDelay(target, eventData);
            }
        }

        public void RegisterSubController(UIInputSubController subController)
        {
            CurrentSubcontroller = subController;
            LastSelectedElement = CurrentEventSystem.currentSelectedGameObject?.GetComponent<UISelectable>();
            SetFocus(DefaultSelectable);
        }

        public void UnregisterSubController(UIInputSubController subController)
        {
            CurrentSubcontroller = null;

            // Przywrocenie ostatnio wybranego elementu przed przejsciem na nowy panel.
            SetFocus(LastSelectedElement);
        }

        private void Awake()
        {
            CurrentEventSystem = EventSystem.current;
            SelectableElements = GetComponentsInChildren<UISelectable>();
            foreach (UISelectable selectable in SelectableElements)
            {
                selectable.Initialize(this);
            }

            DefaultSelectableElement = Array.Find(SelectableElements, x => x.IsDefaultSelected);

            SetFocus();
        }

        private void Update()
        {
            // == true.
            // README: Mozna wykorzystac np. Rewired albo utworzyc modul przechowujacy zmapowane przyciski w grze.
            // Konfiguracja na "sztywno" pod pada DS4.
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton2))
            {
                GameObject selectedObject = CurrentEventSystem.currentSelectedGameObject;
                if (selectedObject != null)
                {
                    HandleSubmitButtonPress(selectedObject);
                }
            }

            if(CurrentEventSystem.currentSelectedGameObject == null)
            {
                SetFocus(DefaultSelectable);
            }
        }

        private void HandleSubmitButtonPress(GameObject selectedObject)
        {
            UISelectable[] selectables = CurrentSubcontroller != null ? CurrentSubcontroller.PanelSelectables : SelectableElements;
            UISelectable selectedButton = Array.Find(selectables, x => x.gameObject == selectedObject);
            if (selectedButton != null)
            {
                selectedButton.PressElement();
            }
        }

        private void SetFocus(UISelectable selectable = null)
        { 
            FocusButton(selectable);
        }

        // Internal.
        private void FocusButtonWithDelay(Selectable target, BaseEventData eventData = null)
        {
            StartCoroutine(_WaitAndFocus(target, eventData));
        }

        // Coroutines.
        private IEnumerator _WaitAndFocus(Selectable target, BaseEventData eventData)
        {
            yield return new WaitForEndOfFrame();
            FocusButton(target, eventData);
        }

        #endregion
    }
}

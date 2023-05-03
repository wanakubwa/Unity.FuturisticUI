using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniversalUISystem
{
    public class UISelectable : Selectable
    {
        #region Fields

        [Header("Flags Settings")]
        [SerializeField]
        private bool isDefaultSelected = false;
        [SerializeField]
        private bool isDisabled = false;

        [Header("Element Graphics")]
        [SerializeField]
        private TextMeshProUGUI buttonText;
        [SerializeField]
        private Image additionalGraphic;
        [SerializeField]
        private Image elementFrameGraphic;

        [Header("State Colors")]
        [SerializeField]
        private Color nonSelectAdditionalElementsColor;
        [SerializeField]
        private Color selectedAdditionalElementsColor;
        [SerializeField]
        private Color nonInteractableAdditionalElementsColor;
        [SerializeField]
        private Color defaultColor = Color.white;

        [Header("Events")]
        [SerializeField]
        private UISelectableEvent onPressed = new UISelectableEvent();
        [SerializeField]
        private UnityEvent onSelected = new UnityEvent();
        [SerializeField]
        private UnityEvent onDeselected = new UnityEvent();

        #endregion

        #region Propeties

        public bool IsDefaultSelected { get => isDefaultSelected; }

        /// <summary>
        /// Czy element moze zostac uzyty.
        /// </summary>
        private bool CanPress { get => interactable == true && isDisabled == false; }

        private UIInputController RootController { get; set; } = null;

        #endregion

        #region Methods

        public void Initialize(UIInputController inputController)
        {
            RootController = inputController;
        }

        public void SetIsDisabled(bool isDisabled)
        {
            this.isDisabled = isDisabled;
        }

        public void PressElement()
        {
            if (CanPress == true)
            {
                onPressed?.Invoke(this);
            }
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            if (isDisabled == false && interactable == false)
            {
                // Przycisk wlaczony ale nie aktywny. - text zamienia sie na kolor z tla wybranego guzika.
                SetNonInteractableState();
            }

            onDeselected?.Invoke();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            if (eventData is AxisEventData axisEvent)
            {
                OnSelectByKey(axisEvent);
            }

            // Przycisk nie jest wylaczony wiec mozna go zaznaczyc.
            if(isDisabled == false && interactable == false)
            {
                // Przycisk wlaczony ale nie aktywny. - text zamienia sie na kolor z tla wybranego guzika.
                SetSelectedNonInteractableState();
            }

            onSelected?.Invoke();
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            // Wcisniecie elementu myszka.
            if (state == SelectionState.Pressed)
            {
                PressElement();
            }

            switch (state)
            {
                case SelectionState.Normal:
                    SetNormalState();
                    break;

                case SelectionState.Selected:
                    SetSelectedInteractableState();
                    break;

                case SelectionState.Disabled:
                    SetNonInteractableState();
                    break;
            }

            if(isDisabled == true)
            {
                SetDisabledState();
            }
        }

        private void OnSelectByKey(AxisEventData axisEvent)
        {
            if (isDisabled == true)
            {
                Selectable target = FindNextElementToSelect(axisEvent.moveDir);

                // Nastepny element w lancuchu tez moze byc wylaczony.
                RootController?.FocusButton(target, axisEvent, true);
            }
        }

        // README: Stany przygotowane w celu otwrcia mozliwosci wydzielenia klasy pochodnej np do toggli lub sliderow.
        #region Element_States

        private void SetSelectedInteractableState()
        {
            SetTargetGraphicColor(additionalGraphic, selectedAdditionalElementsColor);
            SetTargetGraphicColor(buttonText, selectedAdditionalElementsColor);
            SetTargetGraphicColor(targetGraphic, colors.selectedColor);
            SetTargetGraphicColor(elementFrameGraphic, defaultColor);
        }

        private void SetSelectedNonInteractableState()
        {
            SetTargetGraphicColor(elementFrameGraphic, colors.selectedColor);
        }

        private void SetNonInteractableState()
        {
            SetTargetGraphicColor(additionalGraphic, nonInteractableAdditionalElementsColor);
            SetTargetGraphicColor(buttonText, nonInteractableAdditionalElementsColor);
            SetTargetGraphicColor(targetGraphic, colors.disabledColor);
            SetTargetGraphicColor(elementFrameGraphic, defaultColor);
        }

        private void SetDisabledState()
        {
            SetTargetGraphicColor(elementFrameGraphic, nonInteractableAdditionalElementsColor);
            SetTargetGraphicColor(buttonText, nonInteractableAdditionalElementsColor);
        }

        private void SetNormalState()
        {
            SetTargetGraphicColor(additionalGraphic, nonSelectAdditionalElementsColor);
            SetTargetGraphicColor(buttonText, nonSelectAdditionalElementsColor);
            SetTargetGraphicColor(targetGraphic, colors.normalColor);
            SetTargetGraphicColor(elementFrameGraphic, defaultColor);
            SetTargetGraphicColor(elementFrameGraphic, defaultColor);
        }

        #endregion

        private Selectable FindNextElementToSelect(MoveDirection direction)
        {
            // Jezeli nie ma nastepnego elementu w kierunku selekcji, powrot do poprzedniego elementu.
            Selectable target = null;
            switch (direction)
            {
                case MoveDirection.Left:
                    target = FindSelectableOnLeft();
                    if(target == null)
                        target = FindSelectableOnRight();
                    break;

                case MoveDirection.Up:
                    target = FindSelectableOnUp();
                    if (target == null)
                        target = FindSelectableOnDown();
                    break;

                case MoveDirection.Right:
                    target = FindSelectableOnRight();
                    if (target == null)
                        target = FindSelectableOnLeft();
                    break;

                case MoveDirection.Down:
                    target = FindSelectableOnDown();
                    if (target == null)
                        target = FindSelectableOnUp();
                    break;
            }

            return target;
        }

        private void SetTargetGraphicColor(Graphic target, Color color)
        {
            if (target != null)
            {
                target.color = color;
            }
        }

        #endregion

        [Serializable]
        private class UISelectableEvent : UnityEvent<UISelectable>
        {
            // None.
        }
    }
}


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniversalUISystem
{
    [RequireComponent(typeof(ScrollRect))]
    public class AutoScrollRect : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private float scrollSpeed = 10f;
        [SerializeField]
        private ScrollRect targetScrollRect;
        [SerializeField]
        private RectTransform viewportTransform;
        [SerializeField]
        private RectTransform contentRectTransform;

        #endregion

        #region Properties

        private RectTransform SelectedRectTransform { get; set; } = null;

        #endregion

        #region Methods

        void Update()
        {
            UpdateScrollToSelected(targetScrollRect, contentRectTransform, viewportTransform);
        }

        void UpdateScrollToSelected(ScrollRect scrollRect, RectTransform contentRectTransform, RectTransform viewportRectTransform)
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;

            if (selected == null)
            {
                return;
            }
            if (selected.transform.parent != contentRectTransform.transform)
            {
                return;
            }

            SelectedRectTransform = selected.GetComponent<RectTransform>();

            Vector3 selectedDifference = viewportRectTransform.localPosition - SelectedRectTransform.localPosition;
            float contentHeightDifference = (contentRectTransform.rect.height - viewportRectTransform.rect.height);

            float selectedPosition = (contentRectTransform.rect.height - selectedDifference.y);
            float currentScrollRectPosition = scrollRect.normalizedPosition.y * contentHeightDifference;
            float above = currentScrollRectPosition - (SelectedRectTransform.rect.height / 2) + viewportRectTransform.rect.height;
            float below = currentScrollRectPosition + (SelectedRectTransform.rect.height / 2);

            if (selectedPosition > above)
            {
                float step = selectedPosition - above;
                float newY = currentScrollRectPosition + step;
                float newNormalizedY = newY / contentHeightDifference;
                scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            }
            else if (selectedPosition < below)
            {
                float step = selectedPosition - below;
                float newY = currentScrollRectPosition + step;
                float newNormalizedY = newY / contentHeightDifference;
                scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(0, newNormalizedY), scrollSpeed * Time.deltaTime);
            }
        }

        #endregion
    }
}
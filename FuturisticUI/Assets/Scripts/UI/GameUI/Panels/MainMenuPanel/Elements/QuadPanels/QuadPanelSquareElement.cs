using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class QuadPanelSquareElement : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private RectTransform rect;
        [SerializeField]
        private Image targetImage;

        #endregion

        #region Propeties

        public RectTransform Rect { get => rect; }
        public Image TargetImage { get => targetImage; }

        #endregion

        #region

        private void Reset()
        {
            rect = GetComponent<RectTransform>();
        }

        #endregion
    }
}
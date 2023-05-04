using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class AnimatedProgressBar : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Image targetImage;
        [SerializeField]
        private float fillDurationS = 3f;
        [SerializeField]
        private Ease easeType = Ease.InOutSine;

        #endregion

        #region Methods

        private void Start()
        {
            RunAnimation();
        }

        private void RunAnimation()
        {
            // Stan default.
            targetImage.fillAmount = 0;

            targetImage.DOFillAmount(1, fillDurationS)
                .SetEase(easeType)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            // Hot "plug and play".
            if(Application.isPlaying == true)
            {
                targetImage.DOKill();
                RunAnimation();
            }
        }

#endif

        #endregion
    }
}
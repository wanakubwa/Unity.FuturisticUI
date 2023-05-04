using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class AnimatedCircle : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Image targetLinesImage;
        [SerializeField]
        private float rotateDurationS = 3f;
        [SerializeField]
        private Vector3 rotationAngles = new Vector3(0f, 0f, 180f);
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
            targetLinesImage.rectTransform.localRotation = Quaternion.identity;

            targetLinesImage.rectTransform.DOLocalRotate(rotationAngles, rotateDurationS, RotateMode.FastBeyond360)
                .SetEase(easeType)
                .SetLoops(-1, LoopType.Incremental)
                .SetLink(gameObject);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            // Hot "plug and play".
            if(Application.isPlaying == true)
            {
                targetLinesImage.DOKill();
                RunAnimation();
            }
        }

#endif

        #endregion
    }
}
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Game.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class AnimatedQuadPanel : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private int squaresToDisplay;
        [SerializeField]
        private Vector2 animationDurationMinMaxS = Vector2.one;
        [SerializeField]
        private Ease easeIn = Ease.InSine;
        [SerializeField]
        private Ease easeOut = Ease.OutSine;
        [SerializeField]
        private Vector2Int quadSize;
        [SerializeField]
        private Color[] colorsToRoll;

        [Space]
        [SerializeField]
        private RectTransform squaresParent;
        [SerializeField]
        private QuadPanelSquareElement squareElementPrefab;

        #endregion

        #region Propeties

        private Vector2 SquareSize { get; set; } = Vector2.zero;

        #endregion

        #region Methods

        private void Start()
        {
            CalculateQuadParameters();
            InitializeSquares();
        }

        private void InitializeSquares()
        {
            for (int i = 0; i < squaresToDisplay; i++)
            {
                QuadPanelSquareElement squareElement = GetNewSquareElement();
                OnStartSquareSequence(squareElement, true);
            }
        }

        private void OnStartSquareSequence(QuadPanelSquareElement element, bool isFirstRun = false)
        {
            int xIndex = Random.Range(0, quadSize.x);
            int yIndex = Random.Range(0, quadSize.y);
            SetSquareToPosition(element, xIndex, yIndex);

            // Wybor losowego koloru z dostepnej puli - mozna dodac ten generyczny extension do wyboru losowego elementu z kolekcji.
            // Mozliwosc dodania wartosci prawdopodobienstwa wybieranego koloru - np. zolty bedzie pokazywany z 30% sznasa na wylosowanie itp.
            Color color = colorsToRoll[Random.Range(0, colorsToRoll.Length)];

            // Pierwsze wejscie po starcie kolor alfa ustawiony bedzie jako 1.
            // Kolejne wejscie z lancucha kolor alfa bedzie ustawiony jako 0.
            color.a = isFirstRun == true ? 1f : 0f;
            element.TargetImage.color = color;

            // Kazdy kwadrat ma inny czas animacji dzieki czemu nie popuja w tym samym momencie - mysle, ze jest ciekawszy efekt.
            float animationDurationS = Random.Range(animationDurationMinMaxS.x, animationDurationMinMaxS.y);

            // Tworzenie lancucha za pomoca dotween.
            element.TargetImage.DOFade(1f, animationDurationS)
                .OnComplete(() => element.TargetImage.DOFade(0f, animationDurationS / 2f).SetEase(easeIn).SetLink(gameObject) // Zakonczenie sekwencji wejscia.
                .OnComplete(() => OnStartSquareSequence(element)).SetEase(easeOut).SetLink(gameObject)); // Zakonczenie sekwencji wyjscia -> powrot do sekwencji wejscia.
        }

        private void SetSquareToPosition(QuadPanelSquareElement element, int xIndex, int yIndex)
        {
            element.Rect.anchoredPosition = new Vector2(xIndex * SquareSize.x, yIndex * SquareSize.y);
        }

        private void CalculateQuadParameters()
        {
            RectTransform quadRect = GetComponent<RectTransform>();

            float squareWidth = quadRect.rect.width / quadSize.x;
            float squareHeight = quadRect.rect.height / quadSize.y;

            SquareSize = new Vector2(squareWidth, squareHeight);
        }

        private QuadPanelSquareElement GetNewSquareElement()
        {
            QuadPanelSquareElement squareElement = Instantiate(squareElementPrefab, squaresParent);
            squareElement.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, SquareSize.x);
            squareElement.Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, SquareSize.y);
            squareElement.Rect.anchoredPosition = Vector2.zero;
            squareElement.Rect.anchoredPosition = Vector2.zero;

            return squareElement;
        }

        #endregion
    }
}
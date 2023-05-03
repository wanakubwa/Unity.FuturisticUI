using System;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    #region Fields

    [Space]
    [SerializeField]
    PopUpManager.PopUpPrority prority;

    [Space]
    [SerializeField]
    private PopUpModel model;
    [SerializeField]
    private PopUpView view;

    [Space(5)]
    [SerializeField]
    private Canvas popUpCanvas;
    [SerializeField]
    private CanvasGroup popUpCanvasGroup;

    #endregion

    #region Propeties

    public event Action OnPopUpClose = delegate { };

    internal PopUpModel Model
    {
        get => model;
    }

    internal PopUpView View
    {
        get => view;
    }

    public Canvas PopUpCanvas
    {
        get => popUpCanvas;
        private set => popUpCanvas = value;
    }

    public CanvasGroup PopUpCanvasGroup { get => popUpCanvasGroup; }

    public PopUpManager.PopUpPrority Prority
    {
        get => prority;
    }

    #endregion

    #region Methods

    public virtual void Initialize()
    {
        gameObject.SetActive(true);

        Model.Initialize();
        View.Initialize();
        InitializeCanvasCamera(Camera.main);
    }

    public void ClosePopUp()
    {
        PopUpManager.Instance.RequestClosePopUp(this);

        OnPopUpClose();

        SetPopUpInteractable(false);

        Model.ClosePopUp();
        View.ClosePopUp();

        OutAnimation inAnimation = GetComponent<OutAnimation>();
        if (inAnimation != null)
        {
            inAnimation.PlayAnimation(() => Destroy(gameObject));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TogglePopUp()
    {
        if (gameObject.activeInHierarchy == true)
        {
            Model.HidePopUp();
        }
        else
        {
            Model.ShowPopUp();
        }
    }

    public void SetPopUpInteractable(bool isInteractable)
    {
        if(PopUpCanvasGroup != null)
        {
            PopUpCanvasGroup.interactable = isInteractable;
        }
    }

    protected T GetModel<T>() where T : PopUpModel
    {
        T model = GetComponent<PopUpModel>() as T;
        return model;
    }

    protected T GetView<T>() where T : PopUpView
    {
        T model = GetComponent<PopUpView>() as T;
        return model;
    }

    protected void OnDisable()
    {
        DettachEvents();
    }

    protected void OnEnable()
    {
        TryPlayEnterAnimation();

        AttachEvents();
    }

    protected void Start()
    {
        View.CustomStart();
    }

    protected virtual void AttachEvents()
    {
        Model.AttachEvents();
        View.AttachEvents();
    }

    protected virtual void DettachEvents()
    {
        Model.DettachEvents();
        View.DettachEvents();
    }

    protected virtual void Update()
    {
        // Mozna wykorzystac np. Rewired albo utworzyc modul przechowujacy zmapowane przyciski w grze.
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            ClosePopUp();
        }
    }

    private void InitializeCanvasCamera(Camera mainCamera)
    {
        if (PopUpCanvas != null)
        {
            PopUpCanvas.worldCamera = mainCamera;
        }
    }

    private void TryPlayEnterAnimation()
    {
        InAnimation inAnimation = GetComponent<InAnimation>();
        if (inAnimation != null)
        {
            inAnimation.PlayAnimation();
        }
    }

    private void Reset()
    {
        model = GetComponent<PopUpModel>();
        view = GetComponent<PopUpView>();
    }

    #endregion
}

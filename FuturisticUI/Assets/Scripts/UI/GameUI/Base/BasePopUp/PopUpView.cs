using UnityEngine;

public class PopUpView : MonoBehaviour
{
    #region Fields



    #endregion

    #region Propeties

    #endregion

    #region Methods

    public virtual void ClosePopUp()
    {

    }

    public virtual void Initialize()
    {

    }

    public virtual void AttachEvents()
    {

    }

    public virtual void DettachEvents()
    {

    }

    public virtual void CustomStart()
    {

    }

    public T GetModel<T>() where T : PopUpModel
    {
        T model = GetComponent<PopUpModel>() as T;
        return model;
    }

    public T GetController<T>() where T : PopUpController
    {
        T model = GetComponent<PopUpController>() as T;
        return model;
    }


    #endregion

    #region Handlers



    #endregion
}

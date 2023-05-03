using System.Collections;
using UnityEngine;


public class PopUpModel : MonoBehaviour
{
    #region Fields

    #endregion

    #region Propeties

    #endregion

    #region Methods

    public virtual void Initialize()
    {

    }

    public virtual void AttachEvents()
    {

    }

    public virtual void DettachEvents()
    {

    }

    public virtual void ClosePopUp()
    {

    }

    public virtual void HidePopUp()
    {
        gameObject.SetActive(false);
    }

    public virtual void ShowPopUp()
    {
        gameObject.SetActive(true);
    }

    public T GetView<T>() where T : PopUpView
    {
        T view = GetComponent<PopUpView>() as T;
        return view;
    }

    public T GetController<T>() where T : PopUpController
    {
        T controller = GetComponent<PopUpController>() as T;
        return controller;
    }

    public IEnumerator WaitAndClose(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        PopUpController controller = GetController<PopUpController>();
        controller.ClosePopUp();

        yield return null;
    }

    #endregion

    #region Handlers



    #endregion
}

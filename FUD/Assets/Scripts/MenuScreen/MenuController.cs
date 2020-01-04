using System.Collections.Generic;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    public List<BaseView> baseViews;

    [HideInInspector]
    public BaseView currentBaseView;


    void Start()
    {
        OnButtonAction(baseViews[0]);
    }


    public void OnButtonAction(BaseView baseView)
    {
        if (currentBaseView != baseView)
        {
            currentBaseView?.OnExitScreen();

            currentBaseView = baseView;

            currentBaseView.ShowScreen();
        }
    }
}

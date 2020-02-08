using System.Collections.Generic;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    public List<BaseView> baseViews;

    [HideInInspector]
    public BaseView currentBaseView;

    private int currentViewIndex = -1;
    void Start()
    {
        OnButtonAction(0);
    }


    public void OnButtonAction(int index)
    {
        if (currentViewIndex != index)
        {
            currentViewIndex = index;

            currentBaseView?.OnExitScreen();

            currentBaseView = baseViews[currentViewIndex];

            currentBaseView.ShowScreen();
        }
    }
}

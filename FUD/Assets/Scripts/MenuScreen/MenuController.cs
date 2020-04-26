using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    public List<BaseView> baseViews;
    public List<Toggle> toggles;

    [HideInInspector]
    public BaseView currentBaseView;

    private int currentViewIndex = -1;
    void Start()
    {
        //toggles[0].isOn = true;
        OnButtonAction(0);
    }


    public void OnButtonAction(int index)
    {
        if (toggles[index].isOn)
        {
            currentBaseView?.OnExitScreen();

            currentBaseView = baseViews[index];

            currentBaseView.ShowScreen();
        }
    }
}

public enum EFeatureType
{ 
    StoryCreation = 1,
    StoryVersionCreation,
    ShareStoryVersion,
    PortfolioCreation,
    PortfolioAlbums,
    ProjectCreation,
    AuditionCreation,
    AuditionJoining,
}

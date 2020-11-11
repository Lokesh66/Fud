using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class MenuController : MonoBehaviour
{
    public List<BaseView> baseViews;
    public List<Toggle> toggles;


    public GameObject oneObject;

    public GameObject twoObject;


    private BaseView currentBaseView;


    void Start()
    {
        //toggles[0].isOn = true;
        OnButtonAction(0);
    }


    void UpdateMenuView()
    {
        int roleId = DataManager.Instance.userInfo.role_id;
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

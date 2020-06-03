﻿using UnityEngine;
using TMPro;

public class MyStoriesController : BaseView
{
    public Transform parentTrans;

    public StoryActivitiesView activitiesView;

    public MyStoriesView alteredView;

    public MyStoriesView storiesView;

    public TextMeshProUGUI[] buttonsList;

    public GameObject addObject;


    public GameObject storyCreateCache;

    public GameObject detailsCache;


    public Color selectedColor;

    public Color disabledColor;


    private GameObject currentObject;

    private ETabType currentTab = ETabType.Offers;


    protected override void EnableView()
    {
        base.EnableView();

        UpdateCurrentView();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    public override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();

        ResetScreenView();
    }

    public void Init()
    { 
    
    }

    void ShowOffersScreen()
    {
        activitiesView.EnableView();
    }

    void ShowAlteredScreen()
    { 
    
    }

    #region Button Actions

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (ETabType)tabIndex)
        {
            buttonsList[(int)currentTab].color = disabledColor;

            buttonsList[tabIndex].color = selectedColor;

            currentTab = (ETabType)tabIndex;

            addObject.SetActive(currentTab == ETabType.Created);

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    #endregion

    void UpdateCurrentView()
    {
        switch (currentTab)
        {
            case ETabType.Offers:
                currentObject = activitiesView.gameObject;
                activitiesView.EnableView();
                break;

            case ETabType.Altered:
                currentObject = alteredView.gameObject;
                alteredView.EnableView(this);
                break;

            case ETabType.Created:
                currentObject = storiesView.gameObject;
                storiesView.EnableView(this);
                break;
        }
    }

    void ResetScreenView()
    {
        currentTab = ETabType.Offers;

        currentObject?.SetActive(false);

        storiesView.ClearData();

        currentObject = null;
    }

    public void CreateSubView(GameObject createObject)
    {
        OnAddSubView(createObject);
    }

    public void OnAddButtonAction()
    {
        if (currentTab == ETabType.Created)
        {
            ShowCreateStoryScreen();
        }
        else { 
            
        }
    }

    void ShowCreateStoryScreen()
    {
        /*GameObject createObject = Instantiate(storyCreateCache, parentTrans);

        OnAddSubView(createObject);*/

        Debug.LogError("CanLoadScreen : " + DataManager.Instance.CanLoadScreen(EFeatureType.StoryCreation));

        if (DataManager.Instance.CanLoadScreen(EFeatureType.StoryCreation))
        {
            StoryCreationView.Instance.Load(OnStoryCreationCloseAction);
        }
        else {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.StoryCreation);
        }
    }

    void OnStoryCreationCloseAction()
    {
        /*        OnRemoveLastSubView();*/
        UpdateCurrentView();      
    }
}

public enum ETabType
{
    Offers,
    Altered,
    Created,
}

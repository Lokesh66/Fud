using UnityEngine;
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

    private EMyStoriesTab currentTab = EMyStoriesTab.Offers;


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
        if (currentTab != (EMyStoriesTab)tabIndex)
        {
            buttonsList[(int)currentTab].color = disabledColor;

            buttonsList[tabIndex].color = selectedColor;

            currentTab = (EMyStoriesTab)tabIndex;

            addObject.SetActive(currentTab == EMyStoriesTab.MyStories);

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    #endregion

    void UpdateCurrentView()
    {
        switch (currentTab)
        {
            case EMyStoriesTab.Offers:
                currentObject = activitiesView.gameObject;
                activitiesView.EnableView();
                break;

            case EMyStoriesTab.Altered:
                currentObject = alteredView.gameObject;
                alteredView.EnableView(this);
                break;

            case EMyStoriesTab.MyStories:
                currentObject = storiesView.gameObject;
                storiesView.EnableView(this);
                break;
        }
    }

    void ResetScreenView()
    {
        currentTab = EMyStoriesTab.MyStories;

        storiesView.ClearData();
    }

    public void CreateSubView(GameObject createObject)
    {
        OnAddSubView(createObject);
    }

    public void OnAddButtonAction()
    {
        if (currentTab == EMyStoriesTab.MyStories)
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

public enum EMyStoriesTab
{
    Offers,
    Altered,
    MyStories,
}

using UnityEngine;
using TMPro;


public class MyStoriesController : BaseView
{
    public Transform parentTrans;

    public StoryActivitiesView activitiesView;

    public StoriesAlteredView alteredView;

    public MyStoriesView storiesView;

    public MyStoriesBrowseView browserView;

    public TextMeshProUGUI[] buttonsList;

    public GameObject addObject;

    public GameObject filterObject;


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

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    #endregion

    void UpdateCurrentView()
    {
        addObject.SetActive(currentTab == ETabType.Created);

        filterObject.SetActive(currentTab != ETabType.Created);

        switch (currentTab)
        {
            case ETabType.Offers:
                currentObject = activitiesView.gameObject;
                activitiesView.EnableView();
                break;

            case ETabType.Altered:
                currentObject = alteredView.gameObject;
                alteredView.EnableView();
                break;

            case ETabType.Created:
                currentObject = storiesView.gameObject;
                storiesView.EnableView(this);
                break;

            case ETabType.Browser:
                currentObject = browserView.gameObject;
                browserView.EnableView(this);
                break;
        }
    }

    void ResetScreenView()
    {
        buttonsList[(int)currentTab].color = disabledColor;

        currentTab = ETabType.Offers;

        buttonsList[0].color = selectedColor;

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

    public void OnFilterButtonAction()
    {
        if (currentTab == ETabType.Offers)
        {
            activitiesView.OnFilterButtonAction();
        }
        else if(currentTab == ETabType.Altered)
        {
            alteredView.OnFilterButtonAction();
        }
        else 
        {
            browserView.OnFilterButtonAction();
        }
    }

    void ShowCreateStoryScreen()
    {
        Debug.LogError("CanLoadScreen : " + DataManager.Instance.CanLoadScreen(EFeatureType.StoryCreation));

        if (DataManager.Instance.CanLoadScreen(EFeatureType.StoryCreation))
        {
            StoryCreationView.Instance.Load(OnStoryCreationCloseAction);
        }
        else {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.StoryCreation);
        }
    }

    void OnStoryCreationCloseAction(StoryCreatedModel createModel)
    {
        UpdateCurrentView();      
    }
}

public enum ETabType
{
    Offers,
    Altered,
    Created,
    Browser,
}

public enum EStatusType
{
    Waiting = 0,
    Accepted = 3,
    ShortListed = 5,
    Rejected = 8,
}

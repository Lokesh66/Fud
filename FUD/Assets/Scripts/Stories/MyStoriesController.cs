using UnityEngine;

public class MyStoriesController : BaseView
{
    public Transform parentTrans;

    public EMyStoriesTab currentTab;

    public MyStoriesView storiesView;

    public StoryActivitiesView activitiesView;


    public GameObject storyCreateCache;

    public GameObject detailsCache;

    public enum EMyStoriesTab
    {
        Stories,
        Activities
    }

    protected override void EnableView()
    {
        base.EnableView();

        OnMyStoriesTabAction();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    public override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();

        if (currentTab == EMyStoriesTab.Stories)
        {
            OnMyStoriesTabAction();
        }
        else {
            ShowActivitiesScreen();
        }
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();

        ResetScreenView();
    }

    public void Init()
    { 
    
    }

    void ShowActivitiesScreen()
    {
        activitiesView.EnableView();
    }

    #region Button Actions

    public void OnMyStoriesTabAction()
    {
        currentTab = EMyStoriesTab.Stories;

        Debug.Log("storiesView = " + storiesView);

        storiesView.EnableView(this);
    }

    public void OnActivitiesTabAction()
    {
        currentTab = EMyStoriesTab.Activities;
    }

    #endregion

    void ResetScreenView()
    {
        currentTab = EMyStoriesTab.Stories;

        storiesView.ClearData();
    }

    public void OnStoryButtonAction(string response)
    {
        StoryDetailsResponseModel responseModel = JsonUtility.FromJson<StoryDetailsResponseModel>(response);

        GameObject detailsObject = Instantiate(detailsCache, parentTrans);

        OnAddSubView(detailsObject);

        if (responseModel.data.Count > 0)
        {
            detailsObject.GetComponent<StoryDetailsController>().Load(responseModel.data[0], this);
        }
    }

    public void CreateSubView(GameObject createObject)
    {
        OnAddSubView(createObject);
    }

    public void OnAddButtonAction()
    {
        if (currentTab == EMyStoriesTab.Stories)
        {
            ShowCreateStoryScreen();
        }
        else { 
            
        }
    }

    void ShowCreateStoryScreen()
    {
        GameObject createObject = Instantiate(storyCreateCache, parentTrans);

        OnAddSubView(createObject);

        createObject.GetComponent<StoryCreationView>().Load(this);
    }
}

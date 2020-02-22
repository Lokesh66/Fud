using UnityEngine;
using TMPro;

public class MyStoriesController : BaseView
{
    public Transform parentTrans;

    public EMyStoriesTab currentTab;

    public MyStoriesView storiesView;

    public StoryActivitiesView activitiesView;

    public TextMeshProUGUI[] buttonsList;


    public GameObject storyCreateCache;

    public GameObject detailsCache;


    public Color selectedColor;

    public Color disabledColor;


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

        activitiesView.gameObject.SetActive(false);

        storiesView.EnableView(this);

        buttonsList[0].color = selectedColor;

        buttonsList[1].color = disabledColor;
    }

    public void OnActivitiesTabAction()
    {
        buttonsList[0].color = disabledColor;

        buttonsList[1].color = selectedColor;

        currentTab = EMyStoriesTab.Activities;

        storiesView.gameObject.SetActive(false);

        ShowActivitiesScreen();


    }

    #endregion

    void ResetScreenView()
    {
        currentTab = EMyStoriesTab.Stories;

        storiesView.ClearData();
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
        /*GameObject createObject = Instantiate(storyCreateCache, parentTrans);

        OnAddSubView(createObject);*/

        StoryCreationView.Instance.Load(OnStoryCreationCloseAction);
    }

    void OnStoryCreationCloseAction()
    {
/*        OnRemoveLastSubView();*/
    }
}

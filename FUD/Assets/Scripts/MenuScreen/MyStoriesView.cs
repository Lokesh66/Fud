using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStoriesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;

    public GameObject detailsPanel;

    public EMyStoriesTab storiesTab; 

    public NoDataView noDataView;


    List<StoryModel> storiesList = new List<StoryModel>();

    MyStoriesController storiesController;

    public void Load()
    {
        if (storiesTab == EMyStoriesTab.MyStories)
        {
            GameManager.Instance.apiHandler.GetAllStories((status, storiesList) =>
            {

                if (status)
                {
                    this.storiesList = storiesList;

                    SetView();
                }
            });
        }
        else {
            GameManager.Instance.apiHandler.GetAlteredStories((status, storiesList) =>
            {
                if (status)
                {
                    this.storiesList = storiesList;

                    SetView();
                }
            });
        }
    }

    public void EnableView(MyStoriesController storiesController)
    {
        this.storiesController = storiesController;

        ClearData();

        Load();

        gameObject.SetActive(true);
    }

    void SetView()
    {
        content.DestroyChildrens();

        if (storiesList?.Count > 0) {

            for (int i = 0; i < storiesList.Count; i++)
            {
                GameObject storyObject = Instantiate(storyCell, content);

                if (storiesTab == EMyStoriesTab.MyStories)
                {
                    storyObject.GetComponent<StoryCell>().SetView(storiesList[i], OnStoryTapAction);
                }
                else {
                    storyObject.GetComponent<StoryCell>().SetView(storiesList[i], OnAlteredTapAction);
                }
            }
        }
        else {
            noDataView.SetView(GetNoDataModel());
        }
        noDataView.gameObject.SetActive(storiesList?.Count == 0);
    }

    void SetAlteredStoriesView()
    { 
    
    }

    public void ClearData()
    {
        content.DestroyChildrens();

        gameObject.SetActive(false);

        storiesList.Clear();
    }

    void OnStoryTapAction(object storyId)
    {
        StoryDetailsController.Instance.Load(storyId, OnStoryClosedAction);
    }

    void OnAlteredTapAction(object storyId)
    { 
    
    }

    void OnStoryClosedAction()
    {

    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Stories Right Now";

        noDataModel.buttonName = "Add Story";

        noDataModel.buttonAction = storiesController.OnAddButtonAction;

        return noDataModel;

    }
}

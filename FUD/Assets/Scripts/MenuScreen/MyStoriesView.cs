using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyStoriesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;

    public GameObject detailsPanel;

    public ETabType storiesTab; 

    public NoDataView noDataView;


    List<StoryModel> storiesList = new List<StoryModel>();

    MyStoriesController storiesController;

    public void Load()
    {
        if (storiesTab == ETabType.Created)
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
        Debug.Log("SetView : Tab Type : " + storiesTab + " Count = " + storiesList.Count);

        content.DestroyChildrens();

        if (storiesList?.Count > 0) {

            for (int i = 0; i < storiesList.Count; i++)
            {
                GameObject storyObject = Instantiate(storyCell, content);

                storyObject.GetComponent<StoryCell>().SetView(storiesList[i], OnStoryTapAction); 
            }
        }
        else {
            noDataView.SetView(GetNoDataModel());
        }
        noDataView.gameObject.SetActive(storiesList?.Count == 0);
    }


    public void ClearData()
    {
        content.DestroyChildrens();

        gameObject.SetActive(false);

        storiesList.Clear();
    }

    void OnStoryTapAction(int storyId)
    {
        StoryDetailsController.Instance.Load(storyId, OnStoryClosedAction);
    }

    void OnStoryClosedAction()
    {

    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        if (storiesTab == ETabType.Created)
        {
            noDataModel.subTitle = "No Stories Right Now";

            noDataModel.buttonName = "Add Story";

            noDataModel.buttonAction = storiesController.OnAddButtonAction;
        }
        else {
            noDataModel.subTitle = "No Altered Stories Right Now";
        }

        return noDataModel;

    }
}

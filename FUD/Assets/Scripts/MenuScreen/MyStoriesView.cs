using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class MyStoriesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;

    public GameObject detailsPanel;

    public ETabType storiesTab; 

    public NoDataView noDataView;

    public MyStoriesTableView tableView;


    public List<StoryModel> storiesList;

    MyStoriesController storiesController;

    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_CREATED_STORIES = 50;


    public void Load()
    {
        if (storiesTab == ETabType.Created)
        {
            GameManager.Instance.apiHandler.GetAllStories(pageNo, (status, storiesList) =>
            {
                if (status)
                {
                    this.storiesList = storiesList;

                    pageNo++;

                    if (storiesList.Count < MAX_CREATED_STORIES)
                    {
                        isPagingOver = true;

                        pageNo = 1;
                    }

                    if (!isInitialized)
                    {
                        tableView.gameObject.SetActive(true);

                        isInitialized = true;
                    }
                    else {
                        tableView.Data.Clear();

                        tableView.Data.Add(storiesList.Count);

                        tableView.Refresh();
                    }

                    if (storiesList.Count == 0)
                    {
                        noDataView.SetView(GetNoDataModel());
                    }

                    noDataView.gameObject.SetActive(storiesList?.Count == 0);
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


    public void ClearData()
    {
        gameObject.SetActive(false);

        storiesList.Clear();
    }

    public void OnStoryTapAction(int storyId)
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

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetAllStories(pageNo, (status, modelsList) =>
        {
            if (status)
            {
                this.storiesList = modelsList;

                pageNo++;

                if (modelsList.Count < MAX_CREATED_STORIES)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                tableView.Data.Clear();

                tableView.Data.Add(modelsList.Count);

                tableView.Refresh();
            }
        });
    }
}

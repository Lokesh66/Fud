using frame8.ScrollRectItemsAdapter.GridExample;
using System;
using System.Collections.Generic;
using UnityEngine;


public class MyStoriesBrowseView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;

    public ETabType storiesTab; 

    public NoDataView noDataView;

    public PublicStoriesTableView tableView;

    public StoryPublicPopUp detailView;


    public List<StoryModel> storiesList;

    MyStoriesController storiesController;

    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_CREATED_STORIES = 50;


    public void Load()
    {
        GameManager.Instance.apiHandler.GetAllPublicStories(pageNo, (status, storiesList) =>
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
                else
                {
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

    public void OnStoryTapAction(StoryModel storyModel)
    {
        detailView.Load(storyModel, OnStoryClosedAction);
    }

    void OnStoryClosedAction(int status)
    {

    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Public Stories Right Now";

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
        GameManager.Instance.apiHandler.GetAllPublicStories(pageNo, (status, modelsList) =>
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

using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class MyStoriesBrowseView : MonoBehaviour
{
    public RectTransform content;

    public GameObject storyCell;

    public ETabType storiesTab; 

    public NoDataView noDataView;

    public PublicStoriesTableView tableView;

    public StoryBrowseFilterView filterView;

    public StoryPublicPopUp detailView;


    public List<StoryModel> storiesList;


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
                    Reload();
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
        detailView.Load(storyModel, null);
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

                Reload();
            }
        });
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        storiesList = data as List<StoryModel>;

        Reload();

        noDataView.gameObject.SetActive(storiesList.Count == 0);
    }

    void Reload()
    {
        tableView.Data.Clear();

        tableView.Data.Add(storiesList.Count);

        tableView.Refresh();
    }
}

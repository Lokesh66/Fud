using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class StoryActivitiesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject activityCell;

    public GameObject noDataObject;

    public StoryActivityPopUp activityPopUp;

    public StoryOfferedTableView tableView;

    public StoryOfferedFilterView filterView;


    public List<StoryActivityModel> activityModels;

    bool isTableViewInitialised = false;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_PAGE_STORIES = 50;


    public void EnableView()
    {
        ClearData();

        Load();

        gameObject.SetActive(true);
    }

    void Load()
    {
        GameManager.Instance.apiHandler.GetStoryPosts(pageNo, (status, response) => {

            StoryActivityResponseModel responseModel = JsonUtility.FromJson<StoryActivityResponseModel>(response);

            if (status && responseModel != null)
            {
                activityModels = responseModel.data;

                pageNo++;

                if (activityModels.Count < MAX_PAGE_STORIES)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                if (!isTableViewInitialised)
                {
                    tableView.gameObject.SetActive(true);

                    isTableViewInitialised = true;
                }
                else {
                    tableView.Data.Clear();

                    tableView.Data.Add(activityModels.Count);

                    tableView.Refresh();
                }

                noDataObject.SetActive(activityModels.Count == 0);
            }
        });
    }

    public void OnStatusApplied(StoryActivityModel activityModel)
    {
        activityModels.Remove(activityModel);

        noDataObject.SetActive(activityModels.Count == 0);
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetStoryPosts(pageNo, (status, response) => {

            StoryActivityResponseModel responseModel = JsonUtility.FromJson<StoryActivityResponseModel>(response);

            if (status)
            {
                activityModels = responseModel.data;

                pageNo++;

                if (activityModels.Count < MAX_PAGE_STORIES)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }
                else {
                    isPagingOver = false;

                    pageNo++;
                }

                tableView.Data.Clear();

                tableView.Data.Add(activityModels.Count);

                tableView.Refresh();
            }
        });
    }

    void ClearData()
    {
        pageNo = 1;

        isPagingOver = false;
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        activityModels = data as List<StoryActivityModel>;

        tableView.Data.Clear();

        tableView.Data.Add(activityModels.Count);

        tableView.Refresh();

        noDataObject.SetActive(activityModels.Count == 0);
    }
}

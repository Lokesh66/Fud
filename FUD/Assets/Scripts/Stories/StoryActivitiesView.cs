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


    public List<StoryActivityModel> activityModels;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_PAGE_STORIES = 50;


    public void EnableView()
    {
        pageNo = 1;

        Load();

        gameObject.SetActive(true);
    }

    void Load()
    {
        tableView.gameObject.SetActive(false);

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

                tableView.gameObject.SetActive(true);

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

}

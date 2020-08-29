using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class StoriesHistoryView : MonoBehaviour
{
    public StoryDetailsController.EScreenSubType storiesTab; 

    public NoDataView noDataView;

    public StoriesHistoryTableView tableView;


    public StoryHistoryActivityPopUp popUpView;

    [HideInInspector]
    public List<StoryHistoryModel> historyModels;

    StoryDetailsController detaileController;

    bool isInitialized = false;

    bool isPagingOver = false;

    int storyId = -1;

    int pageNo = 1;

    int MAX_CREATED_STORIES = 50;


    public void Load(List<StoryHistoryModel> historyModels)
    {
        this.historyModels = historyModels;

        gameObject.SetActive(true);

        if (!isInitialized)
        {
            tableView.gameObject.SetActive(true);

            isInitialized = true;
        }
        else
        {
            tableView.Data.Clear();

            tableView.Data.Add(historyModels.Count);

            tableView.Refresh();
        }
    }


    public void ClearData()
    {
        gameObject.SetActive(false);

        historyModels.Clear();
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    void OnStoryClosedAction()
    {

    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Histories Now";

        return noDataModel;
    }
}

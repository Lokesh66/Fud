using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class StoriesHistoryView : MonoBehaviour
{
    public StoryDetailsController.EScreenSubType storiesTab; 

    public NoDataView noDataView;

    public StoriesHistoryTableView tableView;


    [HideInInspector]
    public List<StoryHistoryModel> historyModels;

    bool isInitialized = false;


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
            Reload();
        }

        noDataView.gameObject.SetActive(historyModels.Count <= 0);
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

    void Reload()
    {
        tableView.Data.Clear();

        tableView.Data.Add(historyModels.Count);

        tableView.Refresh();
    }
}

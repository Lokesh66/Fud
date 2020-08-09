using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class StoriesAlteredView : MonoBehaviour
{
    public RectTransform content;

    public GameObject alteredCell;

    public ETabType storiesTab;

    public StoriesAlteredPopUpView alteredPopUpView;

    public StoryAlteredTableView tableView;

    public NoDataView noDataView;

    public StoryAlteredFilterView filterView;


    public List<StoryAlteredModel> modelsList;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_ALTERED_STORIES = 50;

    bool isTableViewInitialised = false;


    public void EnableView()
    {
        ClearData();

        gameObject.SetActive(true);

        Load();
    }

    void Load()
    {
        GameManager.Instance.apiHandler.GetAlteredStories(pageNo, (status, modelsList) =>
        {
            if (status)
            {
                this.modelsList = modelsList;

                pageNo++;

                if (modelsList.Count < MAX_ALTERED_STORIES)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                if (!isTableViewInitialised)
                {
                    tableView.gameObject.SetActive(true);

                    isTableViewInitialised = true;
                }
                else
                {
                    tableView.Data.Clear();

                    tableView.Data.Add(modelsList.Count);

                    tableView.Refresh();
                }

                if (modelsList.Count == 0)
                {
                    noDataView.SetView(GetNoDataModel());
                }

                noDataView.gameObject.SetActive(modelsList?.Count == 0);
            }
        });
    }

    public void ClearData()
    {
        gameObject.SetActive(false);

        modelsList?.Clear();

        pageNo = 1;

        isPagingOver = false;
    }

    public void OnAlteredTapAction(StoryAlteredModel alteredModel)
    {
        alteredPopUpView.Load(alteredModel);
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Modified Stories Right Now";

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
        GameManager.Instance.apiHandler.GetAlteredStories(pageNo, (status, modelsList) =>
        { 
            if (status)
            {
                this.modelsList = modelsList;

                pageNo++;

                if (modelsList.Count < MAX_ALTERED_STORIES)
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

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        modelsList = data as List<StoryAlteredModel>;

        tableView.Data.Clear();

        tableView.Data.Add(modelsList.Count);

        tableView.Refresh();

        noDataView.gameObject.SetActive(modelsList.Count == 0);
    }
}

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


    public List<StoryAlteredModel> modelsList;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_ALTERED_STORIES = 50;


    public void EnableView()
    {
        ClearData();

        gameObject.SetActive(true);

        Load();
    }

    void Load()
    {
        tableView.gameObject.SetActive(false);

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

                tableView.gameObject.SetActive(true);

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

        noDataModel.subTitle = "No Altered Stories Right Now";

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
}

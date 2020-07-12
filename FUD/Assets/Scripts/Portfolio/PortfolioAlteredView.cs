using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class PortfolioAlteredView : MonoBehaviour
{
    public RectTransform content;

    public GameObject noDataObject;

    public ETabType tabType;

    public PortfolioActivityPopUp activityPopUp;

    public PortfolioAlteredTableView tableView;

    public PortfolioAlteredFilterView filterView;


    [HideInInspector]
    public List<PortfolioActivityModel> activityModels;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_ALTERED_PORTFOLIOS = 50;


    public void Load()
    {
        ClearData();

        LoadAlteredData();

        gameObject.SetActive(true);
    }

    void LoadAlteredData()
    {
        tableView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetAlteredPortfolios(pageNo, (status, response) => {

            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            if (status)
            {
                activityModels = responseModel.data;

                pageNo++;

                if (activityModels.Count < MAX_ALTERED_PORTFOLIOS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                tableView.gameObject.SetActive(true);

                noDataObject.SetActive(activityModels.Count == 0);
            }
        });
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetAlteredPortfolios(pageNo, (status, response) =>
        {
            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            if (status)
            {
                this.activityModels = responseModel.data;

                pageNo++;

                if (activityModels.Count < MAX_ALTERED_PORTFOLIOS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }
                else
                {
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
        isPagingOver = false;

        pageNo = 1;
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        activityModels = data as List<PortfolioActivityModel>;

        tableView.Data.Clear();

        tableView.Data.Add(activityModels.Count);

        tableView.Refresh();

        noDataObject.SetActive(activityModels.Count == 0);
    }
}

using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class PortfolioOfferedView : MonoBehaviour
{
    public RectTransform content;

    public GameObject noDataObject;

    public ETabType tabType;

    public PortfolioActivityPopUp activityPopUp;

    public PortfolioOfferedTableView tableView;

    public PortfolioOfferedFilterView filterView;


    [HideInInspector]
    public List<PortfolioActivityModel> activityModels;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_OFFERED_PORTFOLIOS = 50;


    public void Load()
    {
        ClearData();

        LoadOfferedData();

        gameObject.SetActive(true);
    }

    void LoadOfferedData()
    {
        tableView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetPortfolioPosts(pageNo, (status, response) => {

            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            Debug.Log("LoadOfferedData : Response = " + response);

            if (status)
            {
                activityModels = responseModel.data;

                pageNo++;

                if (activityModels.Count < MAX_OFFERED_PORTFOLIOS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                noDataObject.SetActive(activityModels.Count == 0);

                tableView.gameObject.SetActive(true);
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
        GameManager.Instance.apiHandler.GetPortfolioPosts(pageNo, (status, response) =>
        {
            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            if (status)
            {
                this.activityModels = responseModel.data;

                pageNo++;

                if (activityModels.Count < MAX_OFFERED_PORTFOLIOS)
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

                tableView.Data.Add(activityModels.Count);

                tableView.Refresh();
            }
        });
    }

    public void RemoveOffer(PortfolioActivityModel activityModel)
    {
        activityModels.Remove(activityModel);

        tableView.Data.Clear();

        tableView.Data.Add(activityModels.Count);

        tableView.Refresh();
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

        noDataObject.SetActive(activityModels?.Count == 0);
    }

    void ClearData()
    {
        filterView.ClearData();
    }
}

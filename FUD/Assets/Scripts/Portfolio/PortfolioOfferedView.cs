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

    bool isInitialized = false;

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
        GameManager.Instance.apiHandler.GetPortfolioPosts(pageNo, (status, response) => {

            PortfolioPostResponse responseModel = JsonUtility.FromJson<PortfolioPostResponse>(response);

            if (status)
            {
                activityModels = responseModel.data;

                pageNo++;

                if (activityModels.Count < MAX_OFFERED_PORTFOLIOS)
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

                Reload();
            }
        });
    }

    public void RemoveOffer(PortfolioActivityModel activityModel)
    {
        activityModels.Remove(activityModel);

        Reload();
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        activityModels = data as List<PortfolioActivityModel>;

        Reload();

        noDataObject.SetActive(activityModels?.Count == 0);
    }

    void ClearData()
    {
        filterView.ClearData();
    }

    void Reload()
    {
        tableView.Data.Clear();

        tableView.Data.Add(activityModels.Count);

        tableView.Refresh();
    }
}

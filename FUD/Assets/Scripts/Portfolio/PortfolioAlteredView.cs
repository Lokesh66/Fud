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


    bool isInitialized = false;

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

                Reload();
            }
        });
    }

    void ClearData()
    {
        isPagingOver = false;

        pageNo = 1;

        filterView.ClearData();
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

        noDataObject.SetActive(activityModels.Count == 0);
    }

    void Reload()
    {
        tableView.Data.Clear();

        tableView.Data.Add(activityModels.Count);

        tableView.Refresh();
    }
}

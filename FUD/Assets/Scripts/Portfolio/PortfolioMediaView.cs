using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class PortfolioMediaView : MonoBehaviour
{
    public RectTransform content;


    public PortfolioMediaDetails mediaDetailsView;

    public PortfolioShareView shareView;

    public PortfolioMediaTableView tableView;


    public GameObject noDataObject;


    PortfolioModel selectedModel;

    bool isInitialized = false;


    [HideInInspector]
    public List<PortfolioModel> portfolioModels;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_PORTFOLIO_ALBUMS = 50;


    public void Load()
    {
        tableView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetAllAlbums(pageNo, (status, models) =>
        {
            portfolioModels = models;

            pageNo++;

            if (portfolioModels.Count < MAX_PORTFOLIO_ALBUMS)
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
                tableView.Data.Clear();

                tableView.Data.Add(portfolioModels.Count);

                tableView.Refresh();
            }

            tableView.gameObject.SetActive(true);

            noDataObject.SetActive(portfolioModels.Count == 0);
        });
    }

    public void OnCellButtonAction(PortfolioModel portfolioModel)
    {
        mediaDetailsView.Load(portfolioModel, this);
    }

    public void OnShareButtonAction(PortfolioModel portfolioModel)
    {
        shareView.Load(portfolioModel);
    }

    public void RemovePortfolio(PortfolioModel portfolioModel)
    {
        portfolioModels.Remove(portfolioModel);

        tableView.Data.Clear();

        tableView.Data.Add(portfolioModels.Count);

        tableView.Refresh();
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetAllAlbums(pageNo, (status, models) =>
        {
            if (status)
            {
                this.portfolioModels = models;

                pageNo++;

                if (portfolioModels.Count < MAX_PORTFOLIO_ALBUMS)
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

                tableView.Data.Add(portfolioModels.Count);

                tableView.Refresh();
            }
        });
    }
}

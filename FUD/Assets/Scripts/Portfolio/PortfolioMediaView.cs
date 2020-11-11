using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class PortfolioMediaView : MonoBehaviour
{
    public PortfolioMediaDetails mediaDetailsView;

    public PortfolioShareView shareView;

    public PortfolioMediaTableView tableView;


    public GameObject noDataObject;


    bool isInitialized = false;


    [HideInInspector]
    public List<PortfolioModel> portfolioModels;


    PortfolioView portfolioView;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_PORTFOLIO_ALBUMS = 50;


    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

        tableView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetAllAlbums(pageNo, (status, models) =>
        {
            if (portfolioModels == null)
                return;

            portfolioModels = models;

            if (status)
            {
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
                    Reload();
                }

                tableView.gameObject.SetActive(true);

                noDataObject.SetActive(portfolioModels.Count == 0);
            }
        });
    }

    public void OnCellButtonAction(PortfolioModel mediaModel)
    {
        mediaDetailsView.Load(mediaModel, this);
    }

    public void OnShareButtonAction(PortfolioModel portfolioModel)
    {
        shareView.Load(portfolioModel, OnPortfolioShared);
    }

    public void RemovePortfolio(PortfolioModel portfolioModel)
    {
        portfolioModels.Remove(portfolioModel);

        Reload();
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

                Reload();
            }
        });
    }

    void OnPortfolioShared()
    {
        portfolioView.OnTabAction(1);
    }

    void Reload()
    {
        tableView.Data.Clear();

        tableView.Data.Add(portfolioModels.Count);

        tableView.Refresh();
    }
}

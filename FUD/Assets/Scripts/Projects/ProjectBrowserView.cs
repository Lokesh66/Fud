using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ProjectBrowserView : MonoBehaviour
{
    public RectTransform content;

    public RectTransform selectedAlbumsContent;

    public BrowseAlbumView albumView;

    public BrowseFilterView filterView;

    public ProfileInfoDetailView profileInfoView;

    public ETabType tabType;

    public GameObject mediaCell;

    public GameObject selectedAlbumCell;

    public NoDataView noDataView;

    public GameObject shortListButton;


    public ProjectBrowseTableView tableView;

    [HideInInspector]
    public List<PortfolioModel> albumModels;

    List<PortfolioModel> selectedAlbums = new List<PortfolioModel>();


    int pageNo = 1;

    bool isPagingOver = false;

    int MAX_ALBUMS = 50;


    public void Load()
    {
        gameObject.SetActive(true);

        GetBrowserData();
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnApplyFilter);
    }

    public void OnShortListButtonAction()
    {
        List<Dictionary<string, object>> dataList = new List<Dictionary<string, object>>();

        Dictionary<string, object> body = new Dictionary<string, object>();

        for (int i = 0; i < selectedAlbums.Count; i++)
        {
            Dictionary<string, object> albumData = new Dictionary<string, object>();

            albumData.Add("portfolio_id", selectedAlbums[i].id);

            albumData.Add("user_id", selectedAlbums[i].user_id);

            dataList.Add(albumData);
        }

        body.Add("portfolios", dataList);

        GameManager.Instance.apiHandler.UpdateShortListedAlbums(body, (apiStatus, apiRespose) => {

            OnAPIResponse(apiStatus, apiRespose);
        });
    }

    void OnAPIResponse(bool status, string response)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

        alertModel.message = status ? "Selected Albums are Shortlisted" : baseResponse.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;

            DataManager.Instance.UpdateFeaturedData(EFeatureType.StoryCreation);
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {

    }

    void GetBrowserData()
    {
        noDataView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetBrowserData(pageNo, (status, albums) => {

            if (status)
            {
                this.albumModels = albums;

                pageNo++;

                if (albumModels.Count < MAX_ALBUMS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                tableView.gameObject.SetActive(true);


                if (albumModels.Count == 0)
                {
                    noDataView.SetView(GetNoDataModel());
                }

                noDataView.gameObject.SetActive(albumModels?.Count == 0);
            }
        });
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Albums Available";

        return noDataModel;
    }

    void ClearData()
    {
        selectedAlbums.Clear();
    }

    public void OnAlbumSelection(bool isAlbumAction, PortfolioModel albumModel)
    {
        if (isAlbumAction)
        {
            albumView.Load(albumModel);
        }
        else
        {
            UpdateShortListView(albumModel);
        }
    }

    public void OnCellButtonAction(PortfolioModel albumModel)
    {
        GameManager.Instance.apiHandler.GetUserInfo(albumModel.user_id, (status, userData) => {

            if (status)
            {
                profileInfoView.Load(userData);
            }
        });
    }

    void UpdateShortListView(PortfolioModel albumModel)
    {
        if (selectedAlbums.Contains(albumModel))
        {
            int modelIndex = selectedAlbums.IndexOf(albumModel);

            Destroy(selectedAlbumsContent.GetChild(modelIndex).gameObject);

            selectedAlbums.Remove(albumModel);
        }
        else
        {
            GameObject albumCell = Instantiate(selectedAlbumCell, selectedAlbumsContent);

            albumCell.GetComponent<BrowserSelectedCell>().Load(albumModel);

            selectedAlbums.Add(albumModel);
        }

        shortListButton.SetActive(selectedAlbums.Count != 0);
    }

    void OnApplyFilter(List<PortfolioModel> portfolioModels)
    {
        albumModels = portfolioModels;

        tableView.Data.Clear();

        tableView.Data.Add(albumModels.Count);

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
        GameManager.Instance.apiHandler.GetBrowserData(pageNo, (status, albums) =>
        {
            if (status)
            {
                this.albumModels = albums;

                pageNo++;

                if (albumModels.Count < MAX_ALBUMS)
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

                tableView.Data.Add(albumModels.Count);

                tableView.Refresh();
            }
        });
    }
}

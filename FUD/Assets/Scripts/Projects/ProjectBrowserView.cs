using System.Collections.Generic;
using UnityEngine;
using System;


public class ProjectBrowserView : MonoBehaviour
{
    public RectTransform content;

    public RectTransform selectedAlbumsContent;

    public BrowseAlbumView albumView;

    public BrowseFilterView filterView;

    public ETabType tabType;

    public GameObject mediaCell;

    public GameObject selectedAlbumCell;

    public NoDataView noDataView;

    public GameObject shortListButton;


    List<PortfolioModel> albumModels;

    List<PortfolioModel> selectedAlbums = new List<PortfolioModel>();

    int pageNo = 1;

    public void Load()
    {
        gameObject.SetActive(true);

        content.DestroyChildrens();

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

        GameManager.Instance.apiHandler.UpdateShortListedAlbums(body, (apiStatus) => {

            OnAPIResponse(apiStatus);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Selected Albums are Shortlisted" : "Something went wrong, please try again.";

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

                SetView();
            }
        });
    }

    void SetView()
    {
        noDataView.gameObject.SetActive(albumModels.Count == 0);

        selectedAlbumsContent.DestroyChildrens();

        if (noDataView.gameObject.activeSelf)
        {
            noDataView.SetView(GetNoDataModel());
        }

        LoadData();
    }

    void LoadData()
    {
        content.DestroyChildrens();

        for (int i = 0; i < albumModels.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, content);

            mediaObject.GetComponent<BrowserAlbumCell>().SetView(albumModels[i], OnAlbumSelection);
        }
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

    void OnAlbumSelection(bool isAlbumAction, PortfolioModel albumModel)
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

        LoadData();
    }
}

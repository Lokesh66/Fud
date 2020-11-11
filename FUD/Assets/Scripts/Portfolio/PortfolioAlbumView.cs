using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PortfolioAlbumView : MonoBehaviour
{
    public PortfolioMediaDetails mediaDetailsView;

    public PortfolioShareView shareView;

    public PortfolioAlbumTableView tableView;

    public TextMeshProUGUI titleText;


    PortfolioModel selectedModel;

    bool isInitialized = false;



    [HideInInspector]
    public List<MultimediaModel> portfolioModels;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_PORTFOLIO_ALBUMS = 50;


    public void Load(PortfolioModel portfolioModel)
    {
        selectedModel = portfolioModel;

        titleText.text = portfolioModel.title;

        gameObject.SetActive(true);

        GameManager.Instance.apiHandler.GetAlbumDetails(selectedModel.id, pageNo, (status, models) =>
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
            }
        });
    }

    public void OnCellButtonAction(PortfolioAlbumCell albumCell)
    {
        EMediaType mediaType = DataManager.Instance.GetMediaType(albumCell.GetMediaModel().media_type);

        ShowMedia(mediaType, albumCell);
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetAlbumDetails(selectedModel.id, pageNo, (status, models) =>
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

    public void OnBackAction()
    {
        gameObject.SetActive(false);
    }

    void ShowMedia(EMediaType mediaType, PortfolioAlbumCell albumCell)
    {
        MultimediaModel multimedia = albumCell.GetMediaModel();

        switch (mediaType)
        {
            case EMediaType.Image:
                UIManager.Instance.topCanvas.ShowBigScreen(multimedia.content_url);
                break;
            case EMediaType.Video:
                VideoStreamer.Instance.Play(albumCell.rawImage, albumCell.mediaPlayer, EMediaType.Video);
                break;
            case EMediaType.Audio:
                VideoStreamer.Instance.Play(albumCell.rawImage, albumCell.mediaPlayer, EMediaType.Audio);
                break;
            case EMediaType.Document:
                PDFManager.Instance.LoadDocument(multimedia.content_url);
                break;
        }
    }

    void Reload()
    {
        tableView.Data.Clear();

        tableView.Data.Add(portfolioModels.Count);

        tableView.Refresh();
    }
}

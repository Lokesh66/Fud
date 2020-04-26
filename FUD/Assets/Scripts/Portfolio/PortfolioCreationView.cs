using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using DG.Tweening;

public class PortfolioCreationView : MonoBehaviour
{
    public TMP_InputField titleField;

    public TMP_InputField descriptionField;

    public RectTransform galleryPanel;

    public UploadedFilesHandler filesHandler;
    

    PortfolioView portfolioView = null;

    private bool isShowingGalleryPanel = false;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();


    public void Init(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;
    }

    public void OnMediaButtonAction(int mediaType)
    {
        EMediaType selectedType = (EMediaType)mediaType;

        SlideGalleryView(false);

        switch (selectedType)
        {
            case EMediaType.Image:
                GalleryManager.Instance.PickImages(OnImagesUploaded);
                break;
            case EMediaType.Audio:
                GalleryManager.Instance.GetAudiosFromGallery(OnAudiosUploaded);
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery(OnVideosUploaded);
                break;
        }
    }

    public void OnUploadButtonAction()
    {
        ShowGalleryPanel();
    }

    public void CreateButtonAction()
    {
        GameManager.Instance.apiHandler.CreatePortfolio(titleField.text, descriptionField.text, uploadedDict, (status, response) => {

            if (status)
            {
                
            }
            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Portfolio Creation Success" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        CanvasManager.Instance.alertView.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        OnBackAction();

        uploadedDict.Clear();
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false);

            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", audioUrls[i]);

                kvp.Add("media_type", "audio");

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "video");

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        }
    }

    public void OnBackAction()
    {
        if (isShowingGalleryPanel)
        {
            SlideGalleryView(false);
        }

        portfolioView.OnRemoveLastSubView();

        Destroy(gameObject);
    }

    public void OnCancelAction()
    {
        SlideGalleryView(false);
    }

    void ShowGalleryPanel()
    {
        SlideGalleryView(true);
    }

    void SlideGalleryView(bool canShow)
    {
        float panelPosition = galleryPanel.anchoredPosition.y;

        float targetPostion = panelPosition += canShow ? galleryPanel.rect.height : -galleryPanel.rect.height;

        galleryPanel.DOAnchorPosY(targetPostion, 0.4f);
    }
}


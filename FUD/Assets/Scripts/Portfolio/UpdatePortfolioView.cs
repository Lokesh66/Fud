using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UpdatePortfolioView : MonoBehaviour
{
    public TMP_InputField titleField;

    public TMP_InputField descriptionField;

    public RectTransform galleryPanel;

    public UploadedFilesHandler filesHandler;

    public GameObject mediaCell;

    public RectTransform content;


    PortfolioModel portfolioModel;

    string contentUrl = string.Empty;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();


    public void Load(PortfolioModel portfolioModel)
    {
        gameObject.SetActive(true);

        this.portfolioModel = portfolioModel;

        SetView();
    }

    void SetView()
    {
        titleField.text = portfolioModel.title;

        descriptionField.text = portfolioModel.description;

        for (int i = 0; i < portfolioModel.PortfolioMedia.Count; i++)
        {
            if (portfolioModel.PortfolioMedia[i].media_type == "image")
            {
                GameObject mediaObject = Instantiate(mediaCell, content);

                mediaObject.GetComponent<CreatedPortfolioMediaCell>().SetView(portfolioModel.PortfolioMedia[i].content_url);
            }
        }
    }

    public void OnUploadButtonAction()
    {
        ShowGalleryPanel();
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

    public void OnSaveButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }   
        GameManager.Instance.apiHandler.UpdatePortfolio(titleField.text, descriptionField.text, portfolioModel.id, uploadedDict, (status, response) => {

            if (status)
            {
                
            }
            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Update portfolio Success" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        CanvasManager.Instance.alertView.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        OnBackButtonAction();

        uploadedDict.Clear();
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(titleField.text))
        {
            errorMessage = "Title should not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Description should not be empty";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            CanvasManager.Instance.alertView.ShowAlert(alertModel);
            return false;
        }

        return true;
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

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

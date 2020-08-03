using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using DG.Tweening;

public class PortfolioCreationView : MonoBehaviour
{
    public TMP_InputField titleField;

    public TMP_Dropdown accessDropDown;

    public TMP_InputField descriptionField;

    public RectTransform galleryPanel;

    public UploadedFilesHandler filesHandler;
    

    PortfolioView portfolioView = null;

    private bool isShowingGalleryPanel = false;

    private string mediaSource = "portfolio";

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
                GalleryManager.Instance.PickImages(mediaSource, OnImagesUploaded);
                break;
            case EMediaType.Audio:
                GalleryManager.Instance.GetAudiosFromGallery(mediaSource, OnAudiosUploaded);
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery(mediaSource, OnVideosUploaded);
                break;
        }
    }

    public void OnUploadButtonAction()
    {
        if (!isShowingGalleryPanel)
        {
            ShowGalleryPanel();
        }
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(titleField.text))
        {
            errorMessage = "Title field not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Description should not be empty";
        }
        else if (uploadedDict.Count <= 0)
        {
            errorMessage = "Media Should not be empty";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            UIManager.Instance.ShowAlert(alertModel);
            return false;
        }

        return true;
    }

    public void CreateButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }
        GameManager.Instance.apiHandler.CreatePortfolio(titleField.text, descriptionField.text, accessDropDown.value, uploadedDict, (status, response) => {

            OnAPIResponse(status, response);
        });
    }

    void OnAPIResponse(bool status, string response)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

        alertModel.message = status ? "Portfolio Creation Success" : baseResponse.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;

            DataManager.Instance.UpdateFeaturedData(EFeatureType.PortfolioAlbums, uploadedDict.Count);

            DataManager.Instance.UpdateFeaturedData(EFeatureType.PortfolioCreation);
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        OnBackAction();

        uploadedDict.Clear();
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        //Debug.Log("OnImagesUploaded : Status = " + status + " url = " + imageUrls.Count);

        if (status)
        {
            //Debug.Log("Available : = " + DataManager.Instance.CanLoadScreen(EFeatureType.PortfolioAlbums));

            if (DataManager.Instance.CanLoadScreen(EFeatureType.PortfolioAlbums))
            {
                //Debug.Log("Loaded Files Length = " + GalleryManager.Instance.GetLoadedFiles().Length);

                filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false);

                for (int i = 0; i < imageUrls.Count; i++)
                {
                    Dictionary<string, object> kvp = new Dictionary<string, object>();

                    kvp.Add("content_id", 1);

                    kvp.Add("content_url", imageUrls[i]);

                    kvp.Add("media_type", "image");

                    uploadedDict.Add(kvp);

                    Debug.Log("Updating Media Dic");
                }
            }
            else {
                UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.PortfolioAlbums);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
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

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false);

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

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    public void OnBackAction()
    {
        if (isShowingGalleryPanel)
        {
            SlideGalleryView(false);
        }

        portfolioView.EnableCurrentScreen();

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


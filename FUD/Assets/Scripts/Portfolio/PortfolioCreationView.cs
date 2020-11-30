﻿using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


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

        int accessStatus = accessDropDown.value == 0 ? 2 : 1;

        GameManager.Instance.apiHandler.CreatePortfolio(titleField.text, descriptionField.text, accessStatus, uploadedDict, (status, response) => {

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
        if (status)
        {
            if (DataManager.Instance.CanLoadScreen(EFeatureType.PortfolioAlbums))
            {
                filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), OnDeleteAction: OnDeleteMediaAction);

                for (int i = 0; i < imageUrls.Count; i++)
                {
                    Dictionary<string, object> kvp = new Dictionary<string, object>();

                    kvp.Add("content_id", 1);

                    kvp.Add("content_url", imageUrls[i]);

                    kvp.Add("media_type", "image");

                    uploadedDict.Add(kvp);
                }
            }
            else {
                UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.PortfolioAlbums);
            }
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Audio, OnDeleteMediaAction);

            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", audioUrls[i]);

                kvp.Add("media_type", "audio");

                uploadedDict.Add(kvp);
            }
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Video, OnDeleteMediaAction);

            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "video");

                uploadedDict.Add(kvp);
            }
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

        panelPosition += canShow ? galleryPanel.rect.height : -galleryPanel.rect.height;

        galleryPanel.DOAnchorPosY(panelPosition, 0.4f);
    }

    void OnDeleteMediaAction(object mediaModel)
    {
        MultimediaModel model = mediaModel as MultimediaModel;

        string url = string.Empty;

        bool isItemRemoved = false;

        
        foreach (var item in uploadedDict)
        {
            Dictionary<string, object> mediaItem = item as Dictionary<string, object>;

            foreach (var kvp in mediaItem)
            {
                if (kvp.Key.Equals("content_url"))
                {
                    url = kvp.Value as string;

                    if (model.content_url.Contains(url))
                    {
                        int modelIndex = uploadedDict.IndexOf(mediaItem);

                        Destroy(filesHandler.content.GetChild(modelIndex).gameObject);

                        uploadedDict.Remove(mediaItem);

                        isItemRemoved = true;

                        break;
                    }
                }
            }

            if (isItemRemoved)
            {
                break;
            }
        }
    }
}


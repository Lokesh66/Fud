﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;


public class UpdateStoryVersionView : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;

    public TMP_InputField descriptionField;

    public RectTransform galleryPanel;

    public GameObject mediaCell;

    public RectTransform mediaContent;

    public TMP_Dropdown accessDropdown;


    List<string> imageUrls;

    List<int> deletedMedia = new List<int>();

    StoryVersion storyVersion;

    StoryVersion updatedModel;

    VersionDetailsView detailsView;

    StoryVersionDetailModel storyVersionDetail;

    List<MultimediaModel> mediaList = new List<MultimediaModel>();

    bool isShowingGalleryPanel = false;

    private string mediaSource = "stories";

    List<UpdateVersionMediaCell> mediaCells = new List<UpdateVersionMediaCell>();

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();


    public void Load(StoryVersion storyVersion, VersionDetailsView detailsView)
    {
        gameObject.SetActive(true);

        descriptionField.textComponent.enableWordWrapping = true;

        this.storyVersion = storyVersion;

        this.detailsView = detailsView;

        GameManager.Instance.apiHandler.GetStoryVersionDetails(storyVersion.id, (status, response) => {

            MultiMediaResponse responseModel = JsonUtility.FromJson<MultiMediaResponse>(response);

            if (status)
            {
                gameObject.SetActive(true);

                storyVersionDetail = responseModel.data[0];

                mediaList = storyVersionDetail.Multimedia;

                SetView();
            }
        });
    }

    void SetView()
    {
        descriptionField.text = storyVersion.description;

        accessDropdown.value = storyVersion.access_modifier;

        LoadMedia();
    }

    void LoadMedia()
    {
        mediaContent.DestroyChildrens(filesHandler.mediaButtonTrans?.gameObject);

        UpdateVersionMediaCell _mediaCell = null;

        mediaCells.Clear();

        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, mediaContent);

            _mediaCell = mediaObject.GetComponent<UpdateVersionMediaCell>();

            _mediaCell.SetView(mediaList[i], OnDeleteMediaAction);

            mediaCells.Add(_mediaCell);
        }

        filesHandler.mediaButtonTrans.SetAsLastSibling();
    }

    void OnDeleteMediaAction(MultimediaModel multimediaModel)
    {
        string url = string.Empty;

        bool isItemRemoved = false;

        int modelIndex = mediaList.IndexOf(multimediaModel);

        Destroy(mediaContent.GetChild(modelIndex).gameObject);

        if (multimediaModel.id != -1)
        {
            deletedMedia.Add(multimediaModel.id);
        }
        else
        {
            foreach (var item in uploadedDict)
            {
                Dictionary<string, object> mediaItem = item as Dictionary<string, object>;

                foreach (var kvp in mediaItem)
                {
                    if (kvp.Key.Equals("content_url"))
                    {
                        url = kvp.Value as string;

                        if (url.Equals(multimediaModel.content_url))
                        {
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

        mediaList.Remove(multimediaModel);
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
            case EMediaType.Document:
                GalleryManager.Instance.GetDocuments(mediaSource, OnDocumentsUploaded);
                break;
        }
    }

    public void OnCancelButtonAction()
    {
        SlideGalleryView(false);
    }

    public void OnBackButtonAction()
    {
        if (isShowingGalleryPanel)
        {
            SlideGalleryView(false);
        }

        uploadedDict.Clear();

        deletedMedia.Clear();

        Reset();
    }

    public void OnSubmitAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.UpdateStoryVersion(storyId, storyVersion.id, descriptionField.text, accessDropdown.value + 1, uploadedDict, deletedMedia, (status, response) => {

            OnAPIResponse(status, response);
        });
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Description field should not be empty";
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

    void OnAPIResponse(bool status, string response)
    {
        UpdatedStoryVersionResponse versionResponse = JsonUtility.FromJson<UpdatedStoryVersionResponse>(response);

        updatedModel = versionResponse.data[0];

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Version updation Success" : versionResponse.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        OnBackButtonAction();

        detailsView.OnEditCallBack(storyVersion, updatedModel);
    }

    void Reset()
    {
        gameObject.SetActive(false);

        descriptionField.text = string.Empty;
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            this.imageUrls = imageUrls;

            MultimediaModel model = null;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles());

            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                model = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

                model.media_type = "image";

                model.content_url = imageUrls[i];

                uploadedDict.Add(kvp);

                mediaList.Add(model);
            }
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Audio);

            MultimediaModel model = null;

            this.imageUrls = audioUrls;

            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                model = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", audioUrls[i]);

                kvp.Add("media_type", "audio");

                model.media_type = "audio";

                model.content_url = audioUrls[i];

                uploadedDict.Add(kvp);

                mediaList.Add(model);
            }
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Video);

            MultimediaModel model = null;

            this.imageUrls = videoUrls;

            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                model = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "video");

                model.media_type = "video";

                model.content_url = videoUrls[i];

                uploadedDict.Add(kvp);

                mediaList.Add(model);
            }
        }
    }

    void OnDocumentsUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Document);

            MultimediaModel model = null;

            this.imageUrls = videoUrls;

            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                model = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "document");

                model.media_type = "document";

                model.content_url = videoUrls[i];

                uploadedDict.Add(kvp);

                mediaList.Add(model);
            }
        }
    }

    public void OnUploadAction()
    {
        ShowGalleryPanel();
    }

    void ShowGalleryPanel()
    {
        SlideGalleryView(true);
    }

    void SlideGalleryView(bool canShow)
    {
        isShowingGalleryPanel = canShow;

        float panelPosition = galleryPanel.anchoredPosition.y;

        float targetPostion = panelPosition += canShow ? galleryPanel.rect.height : -galleryPanel.rect.height;

        galleryPanel.DOAnchorPosY(targetPostion, 0.4f);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;


public class UpdateStoryVersionView : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;

    public TMP_Dropdown roledropDown;

    public TMP_InputField descriptionField;

    public RectTransform galleryPanel;

    public GameObject mediaCell;

    public RectTransform mediaContent;

    public TMP_Dropdown accessDropdown;


    List<Genre> genres;

    List<string> imageUrls;

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

        PopulateDropdown();

        LoadMedia();
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        Genre requiredGenre = genres.Find(genre => genre.id == storyVersion.genre_id);

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        roledropDown.ClearOptions();

        roledropDown.AddOptions(options);

        roledropDown.value = genres.IndexOf(requiredGenre);
    }

    void LoadMedia()
    {
        mediaContent.DestroyChildrens();

        UpdateVersionMediaCell _mediaCell = null;

        mediaCells.Clear();

        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, mediaContent);

            _mediaCell = mediaObject.GetComponent<UpdateVersionMediaCell>();

            _mediaCell.SetView(mediaList[i], OnDeleteMediaAction);

            //UpdateMediaDict(mediaList[i]);

            mediaCells.Add(_mediaCell);
        }

        if (mediaList.Count > 0)
        {
            SetVideoThumbnails(0);
        }
    }

    void SetVideoThumbnails(int index)
    {
        mediaCells[index].SetVideoThumbnail(() => {

            index++;

            if (index >= mediaList.Count)
            {
                return;
            }

            SetVideoThumbnails(index);
        });
    }

    void UpdateMediaDict(MultimediaModel model)
    {
        Dictionary<string, object> kvp = new Dictionary<string, object>();

        kvp.Add("content_id", 1);

        kvp.Add("content_url", model.content_url);

        kvp.Add("media_type", model.media_type);

        uploadedDict.Add(kvp);
    }

    void OnDeleteMediaAction(MultimediaModel multimediaModel)
    {
        string url = string.Empty;

        bool isItemRemoved = false;

        int modelIndex = mediaList.IndexOf(multimediaModel);

        Destroy(mediaContent.GetChild(modelIndex).gameObject);

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

        Reset();
    }

    public void OnSubmitAction()
    {
        if (!CanCallAPI())
        {
            return;
        }
        string selectedGenreText = roledropDown.options[roledropDown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.UpdateStoryVersion(storyId, storyVersion.id, descriptionField.text, selectedGenre.id, accessDropdown.value, uploadedDict, (status, response) => {

            if (status)
            {
                
            }
            else
            {
                Debug.LogError("Story Updation Failed");
            }

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

        updatedModel = versionResponse.data;

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

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false);

            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                model = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

                model.media_type = "image";

                model.content_url = imageUrls[i];

                mediaList.Add(model);

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

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false, EMediaType.Audio);

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

                mediaList.Add(model);

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false, EMediaType.Video);

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

                mediaList.Add(model);

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
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

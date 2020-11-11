using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

[DisallowMultipleComponent]
public class CreateStoryVersion : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;

    public TMP_Dropdown accessDropdown;

    public TMP_InputField descriptionField;

    public RectTransform galleryPanel;


    StoryVersionsView versionsView;

    List<string> imageUrls;

    string apiResponse;

    private string mediaSource = "stories";


    bool isShowingGalleryPanel = false;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    public void Load(StoryVersionsView versionsView)
    {
        gameObject.SetActive(true);

        this.versionsView = versionsView;
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
        versionsView.gameObject.SetActive(true);

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

        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.CreateStoryVersion(storyId, descriptionField.text, accessDropdown.value + 1, uploadedDict, (status, response) => {

            apiResponse = response;

            OnAPIResponse(status);
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

    void OnAPIResponse(bool status)
    {
        CreatedStoryVersionResponse responseModel = JsonUtility.FromJson<CreatedStoryVersionResponse>(apiResponse);

        Debug.Log("apiResponse = " + apiResponse + " responseModel = " + responseModel);

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Version Creation Success" : responseModel.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;

            DataManager.Instance.UpdateFeaturedData(EFeatureType.StoryVersionCreation);
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        CreatedStoryVersionResponse responseModel = JsonUtility.FromJson<CreatedStoryVersionResponse>(apiResponse);

        Reset();

        versionsView.AddStoryVersion(responseModel.data);

        OnBackButtonAction();

        apiResponse = string.Empty;
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

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles());

            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

                uploadedDict.Add(kvp);
            }
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            this.imageUrls = audioUrls;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Audio, OnDeleteAction);

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
            this.imageUrls = videoUrls;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Video, OnDeleteAction);

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

    void OnDocumentsUploaded(bool status, List<string> documentURLs)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Document, OnDeleteAction);

            for (int i = 0; i < documentURLs.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", documentURLs[i]);

                kvp.Add("media_type", "document");

                uploadedDict.Add(kvp);
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

    void OnDeleteAction(object mediaModel)
    {
        MultimediaModel multimediaModel = mediaModel as MultimediaModel;

        Debug.Log("multimediaModel = " + multimediaModel.content_url);

        string url = string.Empty;

        bool isItemRemoved = false;

        //int modelIndex = mediaList.IndexOf(mediaModel);


        foreach (var item in uploadedDict)
        {
            Dictionary<string, object> mediaItem = item as Dictionary<string, object>;

            foreach (var kvp in mediaItem)
            {
                if (kvp.Key.Equals("content_url"))
                {
                    url = kvp.Value as string;

                    Debug.Log("url = " + url);

                    if (multimediaModel.content_url.Contains(url))
                    {
                        Destroy(filesHandler.content.GetChild(uploadedDict.IndexOf(mediaItem)).gameObject);

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

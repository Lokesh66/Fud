using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class StoryCreationView : MonoBehaviour
{
    #region Singleton

    private static StoryCreationView instance = null;

    private StoryCreationView()
    {

    }

    public static StoryCreationView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StoryCreationView>();
            }
            return instance;
        }
    }

    #endregion

    public UploadedFilesHandler filesHandler;

    public Transform parentPanel;

    public RectTransform galleryPanel;

    public TMP_InputField storyTitleField;

    public TMP_InputField subTitleField;

    public TMP_Dropdown dropdown;

    public TMP_Dropdown accessDropdown;

    public TMP_InputField descriptionField;

    public GameObject addImageObject;

    public Sprite addPosterTexture;


    public Image titlePosterImage;


    StoryCreatedModel createdModel;

    List<Genre> genres;

    string titlePosterURL = string.Empty;

    System.Action<StoryCreatedModel> OnClose;

    private string mediaSource = "stories";


    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    private bool isShowingGalleryPanel = false;


    public void Load(System.Action<StoryCreatedModel> onClose)
    {
        parentPanel.gameObject.SetActive(true);

        OnClose = onClose;

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(options);
    }

    public void OnBackButtonAction()
    {
        if (isShowingGalleryPanel)
        {
            SlideGalleryView(false);
        }

        parentPanel.gameObject.SetActive(false);

        OnClose?.Invoke(createdModel);

        OnClose = null;

        createdModel = null;

        Reset();
    }

    public void OnUploadAction()
    {
        ShowGalleryPanel();
    }

    public void OnSubmitAction()
    {
        if (!CanCallAPI())
        {
            return;
        }
        else
        {
            string selectedGenreText = dropdown.options[dropdown.value].text;

            Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

            int accessStatus = accessDropdown.value == 0 ? 2 : 1;

            GameManager.Instance.apiHandler.CreateStory(storyTitleField.text, subTitleField.text, descriptionField.text, titlePosterURL, selectedGenre.id, accessStatus, uploadedDict, (status, response) =>
            {
                OnAPIResponse(status, response);
            });
        }
    }

    void OnAPIResponse(bool status, string response)
    {
        UpdatedStoryResponse responseModel = JsonUtility.FromJson<UpdatedStoryResponse>(response);

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Creation Success" : responseModel.message;

        if (status)
        {
            createdModel = responseModel.data;

            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;

            DataManager.Instance.UpdateFeaturedData(EFeatureType.StoryCreation);
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        Reset();

        uploadedDict.Clear();

        OnBackButtonAction();
    }

    public void OnTitlePosterButtonAction()
    {
        GalleryManager.Instance.GetImageFromGallaery(mediaSource, OnTitlePosterUploaded);
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

    public void OnTItleAction()
    {
        storyTitleField.Select();
    }

    public void OnStotyLineAction()
    {
        subTitleField.Select();
    }

    public void OnDescriptionAction()
    {
        descriptionField.Select();

        TouchScreenKeyboard.Open(descriptionField.text);
    }

    public void OnCancelButtonAction()
    {
        SlideGalleryView(false);
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(storyTitleField.text))
        {
            errorMessage = "Story title should not be empty";
        }
        else if (string.IsNullOrEmpty(subTitleField.text))
        {
            errorMessage = "Story sub title should not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Story description should not be empty";
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

    void Reset()
    {
        //storyTitleField.text = string.Empty;
        parentPanel.gameObject.SetActive(false);

        addImageObject.SetActive(true);

        titlePosterImage.sprite = addPosterTexture;

        storyTitleField.text = string.Empty;

        subTitleField.text = string.Empty;

        titlePosterURL = descriptionField.text = string.Empty;
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), OnDeleteAction: OnDeleteAction);

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
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Audio, OnDeleteAction: OnDeleteAction);

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
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Document);

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

    public void OnTitlePosterUploaded(bool status, List<string> imageURls)
    {
        if (status)
        {
            titlePosterURL = imageURls[0];

            GameManager.Instance.apiHandler.DownloadImage(titlePosterURL, (sprite) => {

                if (sprite != null && this != null)
                {
                    titlePosterImage.sprite = sprite;
                }
            });

            addImageObject.SetActive(false);
        }
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

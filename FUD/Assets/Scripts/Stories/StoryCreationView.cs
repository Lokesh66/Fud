using frame8.ScrollRectItemsAdapter.MultiplePrefabsExample;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections;

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

    public Texture2D addPosterTexture;


    public RemoteImageBehaviour titlePosterImage;


    StoryModel createdModel;

    List<Genre> genres;

    List<string> imageUrls;

    string titlePosterURL = string.Empty;

    System.Action<StoryModel> OnClose;

    private string mediaSource = "stories";


    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    private bool isShowingGalleryPanel = false;


    public void Load(System.Action<StoryModel> onClose)
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

            GameManager.Instance.apiHandler.CreateStory(storyTitleField.text, subTitleField.text, descriptionField.text, titlePosterURL, selectedGenre.id, 1 - accessDropdown.value, uploadedDict, (status, response) =>
            {
                if (status)
                {
                    Debug.Log("Story Uploaded Successfully");
                }
                else
                {
                    Debug.LogError("Story Updation Failed");
                }

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

        titlePosterImage._RawImage.texture = addPosterTexture;

        storyTitleField.text = string.Empty;

        subTitleField.text = string.Empty;

        titlePosterURL = descriptionField.text = string.Empty;
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            this.imageUrls = imageUrls;

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
        else {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            this.imageUrls = audioUrls;

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

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false);

            this.imageUrls = videoUrls;

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

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    public void OnTitlePosterUploaded(bool status, List<string> imageURls)
    {
        if (status)
        {
            titlePosterURL = imageURls[0];

            titlePosterImage.Load(titlePosterURL, true, (fromCache, success) =>
            {
                if (success)
                {
                    StartCoroutine(UpdateTitlePoster());
                }
            });

            addImageObject.SetActive(false);
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    IEnumerator UpdateTitlePoster()
    {
        yield return new WaitForSeconds(0.5f);

        titlePosterImage.Load(titlePosterURL);
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

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.IO;
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
    public Transform parentPanel;

    public RectTransform galleryPanel;

    public TMP_InputField storyTitleField;

    public TMP_InputField subTitleField;

    public TMP_Dropdown dropdown;

    public TMP_InputField descriptionField;

    public Image screenShotImage;

    public TextMeshProUGUI contentType;

    public TextMeshProUGUI filePath;

    public TextMeshProUGUI statusText;

    public TextMeshProUGUI canSupportMultipleText;


    MyStoriesController storiesController;

    List<Genre> genres;

    List<string> imageUrls;

    System.Action OnClose;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    private bool isShowingGalleryPanel = false;

    public void Load(System.Action onClose)
    {
        parentPanel.gameObject.SetActive(true);

        OnClose = onClose;

        statusText.text = galleryPanel.anchoredPosition.y.ToString();

        filePath.text = galleryPanel.rect.height.ToString(); ;

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

        OnClose?.Invoke();
        OnClose = null;
    }

    public void OnUploadAction()
    {
        ShowGalleryPanel();
    }

    public void OnSubmitAction()
    {
        string selectedGenreText = dropdown.options[dropdown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        Debug.Log("Genre Id = " + selectedGenre.id);

        GameManager.Instance.apiHandler.CreateStory(storyTitleField.text, subTitleField.text, descriptionField.text, selectedGenre.id, uploadedDict, (status, response) => {

            if (status)
            {
                Reset();

                uploadedDict.Clear();

                OnBackButtonAction();
                Debug.Log("Story Uploaded Successfully");
            }
            else {
                Debug.LogError("Story Updation Failed");
            }
        });
    }

    public void OnScreenShotAction()
    {
        GetScreenShot();
    }

    public void OnVideosAction()
    {
        GetGalleryVideos();
    }

    public void OnCancelAction()
    { 
        
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

    void Reset()
    {
        //storyTitleField.text = string.Empty;
        parentPanel.gameObject.SetActive(false);

        storyTitleField.text = string.Empty;

        subTitleField.text = string.Empty;

        descriptionField.text = string.Empty;
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            this.imageUrls = imageUrls;

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

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
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

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
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

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
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

        float targetPostion = panelPosition += canShow ? 500 : -500;

        statusText.text = galleryPanel.anchoredPosition.y.ToString();

        canSupportMultipleText.text = "targetPostion = " + targetPostion;

        galleryPanel.DOAnchorPosY(targetPostion, 0.4f);
    }

    void GetScreenShot()
    { 
    
    }

    void GetGalleryVideos()
    { 
    
    }
}

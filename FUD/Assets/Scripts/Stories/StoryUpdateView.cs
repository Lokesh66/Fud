using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class StoryUpdateView : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;

    public RectTransform galleryPanel;

    public TMP_InputField storyTitleField;

    public TMP_InputField subTitleField;

    public TMP_Dropdown dropdown;

    public TMP_Dropdown accessDropdown;

    public TMP_InputField descriptionField;

    public Image posterImage;


    List<Genre> genres;

    List<string> imageUrls;

    StoryModel storyModel;

    StoryCell storyCell;

    private string titlePosterURL = string.Empty;

    private string mediaSource = "stories";

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();



    public void Load(StoryModel storyModel, StoryCell storyCell)
    {
        this.storyModel = storyModel;

        this.storyCell = storyCell;

        SetView();
    }

    void SetView()
    {
        storyTitleField.text = storyModel.title;

        subTitleField.text = storyModel.story_line;

        descriptionField.text = storyModel.description;

        accessDropdown.value = storyModel.access_modifier;

        UpdateTitlePosterImage();

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        Debug.Log("genre_id = " + storyModel.genre_id);

        Genre requiredGenre = genres.Find(genre => genre.id == storyModel.genre_id);

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(requiredGenre.name));

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        dropdown.ClearOptions();

        dropdown.AddOptions(options);

        dropdown.value = genres.IndexOf(selectedGenre);
    }

    void UpdateTitlePosterImage()
    {
        UpdateTitlePoster(storyModel.title_poster);
    }

    public void OnUploadAction()
    {
        ShowGalleryPanel();
    }

    public void OnBackButtonAction()
    {
        Destroy(gameObject);
    }

    public void OnTitleAction()
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
    }

    public void OnSubmitAction()
    {
        if(!CanCallAPI())
        {
            return;
        }

        string selectedGenreText = dropdown.options[dropdown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        GameManager.Instance.apiHandler.UpdateStory(storyModel.id, storyTitleField.text, subTitleField.text, descriptionField.text, titlePosterURL, selectedGenre.id, accessDropdown.value, uploadedDict, (status, response) => {

            OnAPIResponse(status, response);
        });
    }

    void OnAPIResponse(bool status, string response)
    {
        UpdatedStoryResponse storyResponse = JsonUtility.FromJson<UpdatedStoryResponse>(response);

        storyModel = storyResponse.data;

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Updation Success" : storyResponse.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        storyCell.SetView(storyModel, storyCell.OnTapActon);

        Destroy(gameObject);

        uploadedDict.Clear();
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

    public void OnEditTitlePosterAction()
    {
        GalleryManager.Instance.GetImageFromGallaery(mediaSource, OnTitlePosterUploaded);
    }

    public void OnTitlePosterUploaded(bool status, List<string> imageURls)
    {
        if (status)
        {
            titlePosterURL = imageURls[0];

            UpdateTitlePoster(titlePosterURL);
        }
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
    }

    void OnDocumentsUploaded(bool status, List<string> documentURLs)
    {
        if (status)
        {
            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false, EMediaType.Document);

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

    public void OnCancelAction()
    {
        SlideGalleryView(false);
    }

    void UpdateTitlePoster(string imageURL)
    {
        GameManager.Instance.apiHandler.DownloadImage(imageURL, sprite => {

            if (this != null && sprite != null)
            {
                posterImage.sprite = sprite;
            }
        });
    }

    void Reset()
    {
        //storyTitleField.text = string.Empty;
        gameObject.SetActive(false);

        storyTitleField.text = string.Empty;

        subTitleField.text = string.Empty;

        descriptionField.text = string.Empty;
    }
}

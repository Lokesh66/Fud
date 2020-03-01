using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateStoryVersionView : MonoBehaviour
{
    public TMP_Dropdown roledropDown;

    public TMP_InputField descriptionField;

    public RectTransform galleryPanel;


    List<Genre> genres;

    List<string> imageUrls;

    StoryVersion storyVersion;

    VersionDetailsView detailsView;

    bool isShowingGalleryPanel = false;

    public void Load(StoryVersion storyVersion, VersionDetailsView detailsView)
    {
        gameObject.SetActive(true);

        this.storyVersion = storyVersion;

        this.detailsView = detailsView;

        SetView();
    }

    void SetView()
    {
        descriptionField.text = storyVersion.description;

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        Genre requiredGenre = genres.Find(genre => genre.id == storyVersion.genre_id);

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(requiredGenre.name));

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        roledropDown.ClearOptions();

        roledropDown.AddOptions(options);

        roledropDown.value = roledropDown.options.FindIndex(option => options.Equals(selectedGenre.name));
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
                GalleryManager.Instance.GetAudiosFromGallery();
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery();
                break;
        }
    }

    public void OnBackButtonAction()
    {
        Reset();
    }

    public void OnSubmitAction()
    {
        List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();

        bool canUpdateMedia = false;

        if (imageUrls != null && imageUrls.Count > 0)
        {
            parameters.Add(new Dictionary<string, object>());

            parameters[0].Add("content_id", 1);

            parameters[0].Add("content_url", imageUrls[0]);

            parameters[0].Add("media_type", "image");

            canUpdateMedia = true;
        }

        string selectedGenreText = roledropDown.options[roledropDown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        Debug.Log("Genre Id = " + selectedGenre.id);

        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.UpdateStoryVersion(storyId, storyVersion.id, descriptionField.text, selectedGenre.id, canUpdateMedia, parameters, (status, response) => {

            if (status)
            {


                OnBackButtonAction();
                Debug.Log("Story Uploaded Successfully");
            }
            else
            {
                Debug.LogError("Story Updation Failed");
            }
        });
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
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
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

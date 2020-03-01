﻿using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.IO;
using TMPro;


public class StoryUpdateView : MonoBehaviour
{
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

    List<Genre> genres;

    List<string> imageUrls;

    StoryModel storyModel;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();



    public void Load(StoryModel storyModel)
    {
        this.storyModel = storyModel;

        SetView();

        canSupportMultipleText.text = "Can Support Text " + NativeGallery.CanSelectMultipleFilesFromGallery().ToString();

        statusText.text = galleryPanel.anchoredPosition.y.ToString();

        filePath.text = galleryPanel.rect.height.ToString(); ;
    }

    void SetView()
    {
        storyTitleField.text = storyModel.title;

        subTitleField.text = storyModel.story_line;

        descriptionField.text = storyModel.description;

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        Genre requiredGenre = genres.Find(genre => genre.id == storyModel.genre_id);

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(requiredGenre.name));

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        dropdown.ClearOptions();

        dropdown.AddOptions(options);

        dropdown.value = dropdown.options.FindIndex(option => options.Equals(selectedGenre.name));
    }

    public void OnUploadAction()
    {
        ShowGalleryPanel();
    }

    public void OnBackButtonAction()
    {
        Destroy(gameObject);
    }

    public void OnSubmitAction()
    {
        string selectedGenreText = dropdown.options[dropdown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        Debug.Log("Genre Id = " + selectedGenre.id);

        GameManager.Instance.apiHandler.UpdateStory(storyTitleField.text, subTitleField.text, descriptionField.text, selectedGenre.id, (status, response) => {

            if (status)
            {
                Destroy(gameObject);

                uploadedDict.Clear();

                Debug.Log("Story Uploaded Successfully");
            }
            else
            {
                Debug.LogError("Story Updation Failed");
            }
        });
    }

    public void OnScreenShotAction()
    {
        GetScreenShot();
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
                GalleryManager.Instance.PickImages(OnImagesUploaded);
                break;
            case EMediaType.Audio:
                GalleryManager.Instance.GetAudiosFromGallery(OnAudiosUploaded);
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery(OnAudiosUploaded);
                break;
        }
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
        else
        {
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


    public void OnCancelAction()
    {
        SlideGalleryView(false);
    }

    void Reset()
    {
        //storyTitleField.text = string.Empty;
        gameObject.SetActive(false);

        storyTitleField.text = string.Empty;

        subTitleField.text = string.Empty;

        descriptionField.text = string.Empty;
    }

    void GetScreenShot()
    {

    }

    void GetGalleryVideos()
    {

    }
}

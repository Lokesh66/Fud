using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class CreateExperienceView : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;

    public RectTransform galleryPanel;

    public TMP_Dropdown industryDropDown;

    public TMP_Dropdown roleDropDown;

    public TMP_Text startDateText;

    public TMP_Text endDateText;

    public TMP_InputField descriptionField;

    DateTime startDate;
    DateTime endDate;
    string dateDefaultText = "Select Date";

    List<string> imageUrls;

    PortfolioView portfolioView = null;

    List<Craft> craftRoles;

    private bool isShowingGalleryPanel = false;

    private string mediaSource = "portfolio";

    List<IndustryModel> industryModels;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    public void Load(PortfolioView portfolioView)
    {
        this.portfolioView = portfolioView;

        startDate = DateTime.MinValue;
        endDate = DateTime.Now;

        LoadRoles();

        GameManager.Instance.apiHandler.GetIndustries((status, industriesList) => {

            if (status) 
            { 
                industryModels = industriesList;

                LoadIndustries();
            }
        });
           
    }

    void LoadRoles()
    {
        craftRoles = DataManager.Instance.crafts;

        List<string> options = new List<string>();

        foreach (var option in craftRoles)
        {
            options.Add(option.name);
        }

        roleDropDown.ClearOptions();
        roleDropDown.AddOptions(options);
    }

    void LoadIndustries()
    {
        List<string> options = new List<string>();

        foreach (var option in industryModels)
        {
            options.Add(option.name);
        }

        industryDropDown.ClearOptions();
        industryDropDown.AddOptions(options);
    }

    public void OnUploadButtonAction()
    {
        ShowGalleryPanel();
    }

    public void CreateButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }
        else
        {
            string selectedGenreText = roleDropDown.options[roleDropDown.value].text;

            Craft selectedGenre = craftRoles.Find(genre => genre.name.Equals(selectedGenreText));

            string selectedIndustryText = industryDropDown.options[industryDropDown.value].text;

            IndustryModel selectedIndustry = industryModels.Find(industry => industry.name.Equals(selectedIndustryText));

            CreateExperianceModel experianceModel = new CreateExperianceModel();

            experianceModel.roleId = selectedGenre.id;

            experianceModel.description = descriptionField.text;

            experianceModel.industryId = selectedIndustry.id;

            experianceModel.startDate = startDateText.text;

            experianceModel.endDate = endDateText.text;

            GameManager.Instance.apiHandler.CreateWorkExperiance(experianceModel, uploadedDict, (status, response) =>
            {
                if (status)
                {

                }

                OnAPIResponse(status);
            });
        }
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Portfolio Experiance Creation Success" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        OnBackAction();

        uploadedDict.Clear();
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if ((string.IsNullOrEmpty(startDateText.text) || startDateText.text.Equals(dateDefaultText)))
        {
            errorMessage = "Start date should not be empty";
        }
        else if (string.IsNullOrEmpty(endDateText.text) || endDateText.text.Equals(dateDefaultText))
        {
            errorMessage = "End date should not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Experience description should not be empty";
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

    public void OnBackAction()
    {
        if (isShowingGalleryPanel)
        {
            SlideGalleryView(false);
        }

        portfolioView.OnRemoveLastSubView();

        Destroy(gameObject);
    }

    public void OnStartDateSelectedAction()
    {
        DatePicker.Instance.GetDate(startDate == DateTime.MinValue ? DateTime.Now : startDate, DateTime.MinValue, endDate, (date, value) =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                startDateText.text = value;
                startDate = date;
            }
            else
            {
                startDateText.text = dateDefaultText;
            }
        });
    }

    public void OnEndDateSelectedAction()
    {
        DatePicker.Instance.GetDate(endDate, startDate, DateTime.Now, (date, value) =>
        {
            if (!string.IsNullOrEmpty(value))
            {
                endDateText.text = value;
                endDate = date;
            }
            else
            {
                endDateText.text = dateDefaultText;
            }
        });
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
        }
    }

    public void OnCancelButtonAction()
    {
        SlideGalleryView(false);
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
            this.imageUrls = videoUrls;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false, EMediaType.Video);

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
}


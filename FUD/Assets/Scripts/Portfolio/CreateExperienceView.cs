using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;


public class CreateExperienceView : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;

    public RectTransform galleryPanel;

    public TMP_Dropdown industryDropDown;

    public TMP_Dropdown roleDropDown;

    public TMP_Dropdown roleCatageryDropdown;

    public TMP_Text startDateText;

    public TMP_Text endDateText;

    public TMP_InputField descriptionField;

    DateTime startDate;
    DateTime endDate;
    string dateDefaultText = "Select Date";

    PortfolioView portfolioView = null;

    List<Craft> craftRoles;

    Craft selectedCraft;

    List<RoleCategeryModel> categeryModels;

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

        DataManager.Instance.GetIndustries((industriesList) => {

            industryModels = industriesList;

            LoadIndustries();
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

    public void OnRoleValueChange()
    {
        selectedCraft = craftRoles[roleDropDown.value];

        GameManager.Instance.apiHandler.GetRoleCategeries(selectedCraft.id, (status, response) => {

            RoleCategeryResponse responseModel = JsonUtility.FromJson<RoleCategeryResponse>(response);

            if (status)
            {
                categeryModels = responseModel.data;

                UpdateRoleCategeryDropdown();
            }
        });
    }

    void UpdateRoleCategeryDropdown()
    {
        List<string> options = new List<string>();

        foreach (var option in categeryModels)
        {
            options.Add(option.name);
        }

        roleCatageryDropdown.ClearOptions();

        roleCatageryDropdown.AddOptions(options);
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
            string selectedIndustryText = industryDropDown.options[industryDropDown.value].text;

            IndustryModel selectedIndustry = industryModels.Find(industry => industry.name.Equals(selectedIndustryText));

            CreateExperianceModel experianceModel = new CreateExperianceModel();

            experianceModel.roleId = selectedCraft.id;

            experianceModel.roleCategoryId = categeryModels[roleCatageryDropdown.value].id;

            experianceModel.description = descriptionField.text;

            experianceModel.industryId = selectedIndustry.id;

            experianceModel.startDate = startDateText.text;

            experianceModel.endDate = endDateText.text;

            GameManager.Instance.apiHandler.CreateWorkExperiance(experianceModel, uploadedDict, (status, response) =>
            {
                OnAPIResponse(status, response);
            });
        }
    }

    void OnAPIResponse(bool status, string response)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

        alertModel.message = status ? "Portfolio Experiance Creation Success" : baseResponse.message;

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
            case EMediaType.Document:
                GalleryManager.Instance.GetDocuments(mediaSource, OnDocumentsUploaded);
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

    void OnDeleteAction(object mediaModel)
    {
        MultimediaModel multimediaModel = mediaModel as MultimediaModel;

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


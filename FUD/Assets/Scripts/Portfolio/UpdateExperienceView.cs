using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;
using System;
using DG.Tweening;


public class UpdateExperienceView : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;

    public Transform mediaContent;

    public GameObject mediaCell;

    public RectTransform galleryPanel;

    public TMP_Dropdown industryDropDown;

    public TMP_Dropdown roleDropDown;

    public TMP_Dropdown roleCatageryDropdown;

    public TMP_Text startDateText;

    public TMP_Text endDateText;

    public TMP_InputField descriptionField;


    MultimediaModel albumModel;

    WorkExperianceModel workModel = null;

    string[] imageURls;

    DateTime startDate;

    DateTime endDate;

    string dateDefaultText = "Select Date";

    List<Craft> craftRoles;

    List<RoleCategeryModel> categeryModels;

    Craft selectedCraft;

    List<string> imageUrls;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    private bool isShowingGalleryPanel = false;

    List<IndustryModel> industryModels;

    Action<WorkExperianceModel> OnUpdate;

    WorkExperianceModel updatedModel;

    List<MultimediaModel> mediaList;

    List<int> deletedMedia = new List<int>();

    private string mediaSource = "portfolio";


    public void Load(WorkExperianceModel workModel, Action<WorkExperianceModel> OnUpdate)
    {
        this.workModel = workModel;

        this.OnUpdate = OnUpdate;

        gameObject.SetActive(true);

        descriptionField.text = workModel.description;

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        startDate = dateTime.AddSeconds(workModel.start_date);

        endDate = dateTime.AddSeconds(workModel.end_date);

        if (startDate != DateTime.MinValue || startDate != DateTime.MaxValue)
        {
            startDateText.text = DatePicker.Instance.GetDateString(startDate);
        }

        if (endDate != DateTime.MinValue || endDate != DateTime.MaxValue)
        {
            endDateText.text = DatePicker.Instance.GetDateString(endDate);
        }

        mediaList = workModel.WorkExpMedia;

        UpdateMediaView();

        LoadRoles();

        DataManager.Instance.GetIndustries((industriesList) => {

            industryModels = industriesList;

            LoadIndustries();
        });
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

    void LoadRoles()
    {
        craftRoles = DataManager.Instance.crafts;

        Craft requiredGenre = craftRoles.Find(genre => genre.id == workModel.role_id);

        List<string> options = new List<string>();

        foreach (var option in craftRoles)
        {
            options.Add(option.name);
        }

        roleDropDown.ClearOptions();

        roleDropDown.AddOptions(options);

        roleDropDown.value = craftRoles.IndexOf(requiredGenre);
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
        IndustryModel requiredGenre = industryModels.Find(industryModel => industryModel.id == workModel.industry_id);

        List<string> options = new List<string>();

        foreach (var option in industryModels)
        {
            options.Add(option.name);
        }

        industryDropDown.ClearOptions();

        industryDropDown.AddOptions(options);

        industryDropDown.value = industryModels.IndexOf(requiredGenre);
    }

    public void OnCancelButtonAction()
    {
        SlideGalleryView(false);
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

            experianceModel.roleCategoryId = categeryModels[roleCatageryDropdown.value].id;

            experianceModel.description = descriptionField.text;

            experianceModel.industryId = selectedIndustry.id;

            experianceModel.startDate = startDateText.text;

            experianceModel.endDate = endDateText.text;

            GameManager.Instance.apiHandler.UpdateWorkExperiance(experianceModel, workModel.id, deletedMedia, uploadedDict, (status, response) =>
            {
                OnAPIResponse(status, response);
            });
        }
    }

    void OnAPIResponse(bool status, string apiResponse)
    {
        AlertModel alertModel = new AlertModel();

        WorkExperianceResponseModel responseModel = JsonUtility.FromJson<WorkExperianceResponseModel>(apiResponse);

        alertModel.message = status ? "Experiance Updation Success" : responseModel.message;

        updatedModel = responseModel.data[0];

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

        OnUpdate?.Invoke(updatedModel);

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
        ClearData();

        gameObject.SetActive(false);
    }

    void ClearData()
    {
        industryDropDown.ClearOptions();

        roleDropDown.ClearOptions();
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

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), OnDeleteAction: OnDeleteButtonAction);

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

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Audio, OnDeleteButtonAction);

            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new MultimediaModel();

                albumModel.content_url = audioUrls[i];

                albumModel.media_type = "audio";

                mediaList.Add(albumModel);

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

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Video, OnDeleteButtonAction);

            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new MultimediaModel();

                albumModel.content_url = videoUrls[i];

                albumModel.media_type = "video";

                mediaList.Add(albumModel);

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
            this.imageUrls = documentURLs;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Document, OnDeleteButtonAction);

            for (int i = 0; i < documentURLs.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new MultimediaModel();

                albumModel.content_url = documentURLs[i];

                albumModel.media_type = "document";

                mediaList.Add(albumModel);

                kvp.Add("content_id", 1);

                kvp.Add("content_url", documentURLs[i]);

                kvp.Add("media_type", "document");

                uploadedDict.Add(kvp);
            }
        }
    }

    void UpdateMediaView()
    {
        GameObject cellObject;

        EMediaType mediaType = EMediaType.Image;

        for(int i = 0; i < mediaList.Count; i++)
        {
            cellObject = Instantiate(mediaCell, mediaContent);

            mediaType = DataManager.Instance.GetMediaType(mediaList[i].media_type);

            cellObject.GetComponent<UploadedFileCell>().Load(workModel.WorkExpMedia[i], mediaType, OnDeleteButtonAction);
        }
    }

    void OnDeleteButtonAction(object model)
    {
        MultimediaModel multimediaModel = model as MultimediaModel;

        string url = string.Empty;

        bool isItemRemoved = false;

        int modelIndex = mediaList.IndexOf(multimediaModel) + 1;

        Destroy(mediaContent.GetChild(modelIndex).gameObject);

        if (multimediaModel.id != -1)
        {
            deletedMedia.Add(multimediaModel.id);
        }
        else
        {
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
        }

        mediaList.Remove(multimediaModel);
    }
}


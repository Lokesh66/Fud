using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;
using System.IO;

public class CreateAuditionView : MonoBehaviour
{
    #region Singleton

    private static CreateAuditionView instance = null;

    private CreateAuditionView()
    {

    }

    public static CreateAuditionView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CreateAuditionView>();
            }
            return instance;
        }
    }
    #endregion

    public Transform parentPanel;

    public TMP_Dropdown typeDropdown;
    public TMP_InputField membersText;
    public TMP_InputField topicText;
    public TMP_InputField titleText;
    public TMP_InputField payAmountText;
    public TMP_InputField ageFromText;
    public TMP_InputField ageToText;
    public TMP_InputField descriptionText;
    public TMP_Text endDateText;

    public TMP_Text buttonText;
    public TMP_Text errorText;

    bool isNewAuditionCreated;

    int projectId;

    string defaultDateText = "Select Date";

    string uploadedImageUrl = string.Empty;

    Audition audition;

    bool isUpdate = false;

    DateTime selectedDate;

    DateTime previousDate;

    System.Action<bool> backAction;
    public void SetView(int projectId, Action<bool> action)
    {
        isUpdate = false;
        this.projectId = projectId;
        parentPanel.gameObject.SetActive(true);
        uploadedImageUrl = string.Empty;
        backAction = action;
        isNewAuditionCreated = false;
        buttonText.text = "Create Audition";
    }

    public void EditView(Audition audition, Action<bool> action)
    {
        isUpdate = true;
        this.audition = audition;
        SetData();
        parentPanel.gameObject.SetActive(true);
        backAction = action;
        isNewAuditionCreated = false;
    }

    void SetData()
    {
        projectId = audition.project_id;

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        previousDate = selectedDate = dateTime.AddSeconds(audition.end_date);

        Debug.Log("selectedDate = " + selectedDate);

        typeDropdown.value = typeDropdown.options.FindIndex(option => option.text == audition.type);
        if (audition.type.Equals("group"))
        {
            membersText.gameObject.SetActive(true);
            membersText.text = audition.no_of_persons_req.ToString();
        }
        else
        {
            membersText.gameObject.SetActive(false);
        }
        topicText.text = audition.topic;
        titleText.text = audition.title;
        payAmountText.text = audition.rate_of_pay.ToString();
        ageFromText.text = audition.age_from.ToString();
        ageToText.text = audition.age_to.ToString();
        if (selectedDate != DateTime.MinValue || selectedDate != DateTime.MaxValue)
        {
            endDateText.text = DatePicker.Instance.GetDateString(selectedDate);
        }
        else
        {
            endDateText.text = defaultDateText;
        }
        descriptionText.text = audition.description;

        if (!string.IsNullOrEmpty(audition.image_url))
        {
            uploadedImageUrl = audition.image_url;
        }
        else
        {
            uploadedImageUrl = string.Empty;
        }
        buttonText.text = "Update Audition";
    }

    public void OnAuditionTypeSelectedAction()
    {
        Debug.Log(typeDropdown.captionText.text);
        if (typeDropdown.captionText.text.ToLower().Equals("group"))
        {
            membersText.gameObject.SetActive(true);
        }
        else
        {
            membersText.gameObject.SetActive(false);
        }
    }

    public void OnDateSelectAction()
    {
        DatePicker.Instance.GetDate(DateTime.Now, DateTime.Now, DateTime.MaxValue, (date, dateString) =>
        {
            if(!string.IsNullOrEmpty(dateString))
                endDateText.text = dateString;
        });
    }
    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke(isNewAuditionCreated);
        backAction = null;
        ClearData();
    }

    public void UploadImageAction()
    {
        GalleryManager.Instance.GetImageFromGallaery(OnImagesUploaded);
    }

    //public void OnRecordVideoAction()
    //{
    //    NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
    //    {
    //        string fileName = Path.GetFileName(path);

    //        byte[] fileBytes = File.ReadAllBytes(path);

    //        titleText.text = fileBytes.Length.ToString();

    //        NativeGallery.SaveVideoToGallery(fileBytes, "Videos", fileName);

    //        GalleryManager.Instance.UploadVideoFile(path, OnVideoUploaded);
    //    });
    //}

    void OnImagesUploaded(bool status, List<string> imagesList)
    {
        if (status)
        {            
            if (imagesList != null && imagesList.Count > 0){
                uploadedImageUrl = imagesList[0];
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = "Media upload failed";

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    public void CreateAuditionButtonAction()
    {
        string errorMessage = string.Empty;

        //Call an API to add into audition list
        if (string.IsNullOrEmpty(typeDropdown.captionText.text))
        {
            errorMessage = "Audition type should not be empty";
            //ShowErrorMessage("Audition type should not be empty");
        }else if (typeDropdown.captionText.text.ToLower().Equals("group") &&
            string.IsNullOrEmpty(membersText.text))
        {
            errorMessage = "Group Audition members should not be empty";
        }
        else if (string.IsNullOrEmpty(topicText.text))
        {
            errorMessage = "Audition topic should not be empty";
            //ShowErrorMessage("Audition topic should not be empty");
        }
        else if (string.IsNullOrEmpty(titleText.text))
        {
            errorMessage = "Audition title should not be empty";
            //ShowErrorMessage("Audition title should not be empty");
        }
        else if (string.IsNullOrEmpty(payAmountText.text))
        {
            errorMessage = "Audition payment should not be empty";
            //ShowErrorMessage("Audition payment should not be empty");
        }
        else if (string.IsNullOrEmpty(ageFromText.text))
        {
            errorMessage = "Audition from age should not be empty";
            //ShowErrorMessage("Audition age should not be empty");
        }
        else if (string.IsNullOrEmpty(ageToText.text))
        {
            errorMessage = "Audition to age should not be empty";         
            //ShowErrorMessage("Audition age should not be empty");
        }
        else if (int.Parse(ageToText.text) < int.Parse(ageFromText.text))
        {
            errorMessage = "Audition to age should be greater than from age";
            //ShowErrorMessage("Audition age should not be empty");
        }
        else if (string.IsNullOrEmpty(endDateText.text) || endDateText.text.Equals(defaultDateText))
        {
            errorMessage = "Audition date should not be empty";          
            //ShowErrorMessage("Audition date should not be empty");
        }
        else if (string.IsNullOrEmpty(descriptionText.text))
        {
            errorMessage = "Audition description should not be empty";
            //ShowErrorMessage("Audition description should not be empty");
        }
        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            UIManager.Instance.ShowAlert(alertModel); 
            return;
        }        
        
        if (isUpdate)
        {
            UpdateAudition();
        }
        else
        {
            CreateAudition();
        }
    }
    
    void CreateAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("project_id", projectId);
        parameters.Add("topic", topicText.text);
        parameters.Add("rate_of_pay", long.Parse(payAmountText.text));
        parameters.Add("end_date", endDateText.text);// "2020-03-23");
        parameters.Add("title", titleText.text);
        parameters.Add("description", descriptionText.text);
        parameters.Add("age_from", Convert.ToInt16(ageFromText.text));
        parameters.Add("age_to", Convert.ToInt16(ageToText.text));

        string auditionType = typeDropdown.captionText.text.ToLower();
        parameters.Add("type", auditionType);// "group","individual");
        if (auditionType.Equals("group"))
        {
            parameters.Add("no_of_persons_req", Convert.ToInt16(membersText.text));
        }
        else if (auditionType.Equals("individual"))
        {
            parameters.Add("no_of_persons_req", 1);
        }

        if (!string.IsNullOrEmpty(uploadedImageUrl))
        {
            parameters.Add("image_url", uploadedImageUrl);
        }

        GameManager.Instance.apiHandler.CreateAudition(parameters, (status, response) =>
        {
            Debug.Log("OnCreateAudition : " + response);
            if (status)
            {
                isNewAuditionCreated = true;
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Created Successfully";
                alertModel.okayButtonAction = BackButtonAction;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);

                DataManager.Instance.UpdateFeaturedData(EFeatureType.AuditionCreation);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Creating Audition Failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    void UpdateAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("id", audition.id);
        if (!audition.topic.Equals(topicText.text))
            parameters.Add("topic", topicText.text);
        if (!audition.rate_of_pay.Equals(long.Parse(payAmountText.text)))
            parameters.Add("rate_of_pay", long.Parse(payAmountText.text));
        if (!DatePicker.Instance.GetDateString(selectedDate).Equals(previousDate))
            parameters.Add("end_date", selectedDate);// "2020-03-23");
        if (!audition.title.Equals(titleText.text))
            parameters.Add("title", titleText.text);
        if (!audition.description.Equals(descriptionText.text))
            parameters.Add("description", descriptionText.text);
        if (!audition.age_from.Equals(Convert.ToInt16(ageFromText.text)))
            parameters.Add("age_from", Convert.ToInt16(ageFromText.text));
        if (!audition.age_to.Equals(Convert.ToInt16(ageToText.text)))
            parameters.Add("age_to", Convert.ToInt16(ageToText.text));

        string auditionType = typeDropdown.captionText.text.ToLower();
        if (!audition.type.Equals(auditionType) ||
            (audition.type.Equals("group") && audition.no_of_persons_req != Convert.ToInt16(membersText.text)))
        {
            parameters.Add("type", auditionType);// "group","individual");
            if (auditionType.Equals("group"))
            {
                parameters.Add("no_of_persons_req", Convert.ToInt16(membersText.text));
            }
            else if (auditionType.Equals("individual"))
            {
                parameters.Add("no_of_persons_req", 1);
            }
        }

        if (!string.IsNullOrEmpty(uploadedImageUrl))
        {
            if (string.IsNullOrEmpty(audition.image_url) || !audition.image_url.Equals(uploadedImageUrl))
            {
                parameters.Add("image_url", uploadedImageUrl);
            }
        }

        if (parameters.Count == 0)
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = "No data to update";
            UIManager.Instance.ShowAlert(alertModel);
            return;
        }

        GameManager.Instance.apiHandler.ModifyAudition(parameters, (status, response) =>
        {
            Debug.Log("ModifyAudition : " + response);
            if (status)
            {
                isNewAuditionCreated = true;
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Updated Successfully";
                alertModel.okayButtonAction = BackButtonAction;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Updating Audition Failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    void ClearData()
    {
        typeDropdown.value = 0;

        payAmountText.text = titleText.text = topicText.text = membersText.text = string.Empty;

        ageFromText.text = ageToText.text = string.Empty;

        endDateText.text = descriptionText.text = string.Empty;
    }
}

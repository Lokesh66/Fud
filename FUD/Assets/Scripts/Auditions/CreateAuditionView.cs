using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;

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
    public TMP_InputField topicText;
    public TMP_InputField titleText;
    public TMP_InputField payAmountText;
    public TMP_InputField ageFromText;
    public TMP_InputField ageToText;
    public TMP_InputField descriptionText;
    public TMP_Text endDateText;

    public TMP_Text errorText;

    bool isNewAuditionCreated;

    int projectId;

    string defaultDateText = "Select Date";

    System.Action<bool> backAction;
    public void SetView(int projectId, System.Action<bool> action)
    {
        this.projectId = projectId;
        parentPanel.gameObject.SetActive(true);
        backAction = action;
        isNewAuditionCreated = false;
    }

    public void OnDateSelectAction()
    {
        DatePicker.Instance.GetDate(System.DateTime.Now, (dateString) =>
        {
            if (string.IsNullOrEmpty(dateString))
            {
                endDateText.text = defaultDateText;
            }
            else
            {
                endDateText.text = dateString;
            }
        });
    }
    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke(isNewAuditionCreated);
        backAction = null;
    }
    public void CreateAuditionButtonAction()
    {
        string errorMessage = string.Empty;

        //Call an API to add into audition list
        if (string.IsNullOrEmpty(typeDropdown.captionText.text))
        {
            errorMessage = "Audition type should not be empty";
            //ShowErrorMessage("Audition type should not be empty");
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
            CanvasManager.Instance.alertView.ShowAlert(alertModel); 
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("project_id", projectId);
        parameters.Add("topic", topicText.text);
        parameters.Add("rate_of_pay", long.Parse(payAmountText.text));
        parameters.Add("end_date", endDateText.text);// "2020-03-23");
        parameters.Add("title", titleText.text);
        parameters.Add("description", descriptionText.text);
        parameters.Add("age_from", Convert.ToInt16(ageFromText.text));
        parameters.Add("age_to", Convert.ToInt16(ageToText.text));
        parameters.Add("type", typeDropdown.captionText.text.ToLower());// "group","individual");
        GameManager.Instance.apiHandler.CreateAudition(parameters, (status, response) => {
            Debug.Log("OnCreateAudition : "+response);
            if (status)
            {
                isNewAuditionCreated = true;
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Created Successfully";
                alertModel.okayButtonAction = BackButtonAction;
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Creating Audition Failed";
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
        });
    }

    void ShowErrorMessage(string message)
    {
        errorText.text = message;
        if (IsInvoking("DisableErrorMessage"))
            CancelInvoke("DisableErrorMessage");
        Invoke("DisableErrorMessage", 2.0f);
    }

    void DisableErrorMessage()
    {
        errorText.DOFade(0f,0.5f).OnComplete(() => {
            errorText.text = string.Empty;
            errorText.color = Color.red;
        });
    }
}

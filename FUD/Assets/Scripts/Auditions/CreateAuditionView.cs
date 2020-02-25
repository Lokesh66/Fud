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
    public TMP_InputField endDateText;
    public TMP_InputField descriptionText;

    public TMP_Text errorText;

    bool isNewAuditionCreated;

    int projectId;

    System.Action<bool> backAction;
    public void SetView(int projectId, System.Action<bool> action)
    {
        this.projectId = projectId;
        parentPanel.gameObject.SetActive(true);
        backAction = action;
        isNewAuditionCreated = false;
    }
    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke(isNewAuditionCreated);
        backAction = null;
    }
    public void CreateAuditionButtonAction()
    {
        //Call an API to add into audition list
        if (string.IsNullOrEmpty(typeDropdown.captionText.text))
        {
            ShowErrorMessage("Audition type should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(topicText.text))
        {
            ShowErrorMessage("Audition topic should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(titleText.text))
        {
            ShowErrorMessage("Audition title should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(payAmountText.text))
        {
            ShowErrorMessage("Audition payment should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(ageFromText.text))
        {
            ShowErrorMessage("Audition age should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(ageToText.text))
        {
            ShowErrorMessage("Audition age should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(endDateText.text))
        {
            ShowErrorMessage("Audition date should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(descriptionText.text))
        {
            ShowErrorMessage("Audition description should not be empty");
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
                BackButtonAction();
            }
            else
            {

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

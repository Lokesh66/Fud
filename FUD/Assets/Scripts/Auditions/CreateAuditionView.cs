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

    public TMP_InputField typeText;
    public TMP_InputField topicText;
    public TMP_InputField ageText;
    public TMP_InputField endDateText;
    public TMP_InputField descriptionText;

    public TMP_Text errorText;

    System.Action backAction;
    public void SetView(System.Action action)
    {
        parentPanel.gameObject.SetActive(true);
        backAction = action;
    }
    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke();
        backAction = null;
    }
    public void CreateAuditionButtonAction()
    {
        //Call an API to add into audition list
        if (string.IsNullOrEmpty(typeText.text))
        {
            ShowErrorMessage("Audition type should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(topicText.text))
        {
            ShowErrorMessage("Audition topic should not be empty");
            return;
        }
        if (string.IsNullOrEmpty(ageText.text))
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

        parameters.Add("topic", topicText.text);
        parameters.Add("rate_of_pay", 10000);
        parameters.Add("end_date", endDateText.text);// "2020-03-23");
        parameters.Add("title", "For all the young  fresh talent");
        parameters.Add("description", descriptionText.text);
        parameters.Add("age_from", Convert.ToInt16(ageText.text));
        parameters.Add("age_to", Convert.ToInt16(ageText.text));
        parameters.Add("type", typeText.text);// "group","individual");

        GameManager.Instance.apiHandler.CreateAudition(parameters, (status, response) => {
            Debug.Log("OnCreateAudition : "+response);
            if (status)
            {
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

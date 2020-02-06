using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class CreateAuditionView : MonoBehaviour
{
    
    public TMP_Text typeText;
    public TMP_InputField topicText;
    public TMP_InputField ageText;
    public TMP_InputField endDateText;
    public TMP_InputField descriptionText;

    public TMP_Text errorText;

    System.Action backAction;
    public void SetView(System.Action action)
    {
        gameObject.SetActive(true);
        backAction = action;
    }
    public void BackButtonAction()
    {
        gameObject.SetActive(false);
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
        parameters.Add("title", "");
        parameters.Add("description", descriptionText.text);
        parameters.Add("age_from", ageText.text);
        parameters.Add("age_to", ageText.text);
        parameters.Add("type", typeText.text);// "group");

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

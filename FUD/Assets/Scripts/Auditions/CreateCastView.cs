using UnityEngine;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class CreateCastView : MonoBehaviour
{
    #region Singleton

    private static CreateCastView instance = null;

    private CreateCastView()
    {

    }

    public static CreateCastView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CreateCastView>();
            }
            return instance;
        }
    }
    #endregion

    public Transform parentPanel;

    public TMP_Dropdown storyCharacterDropdown;
    public TMP_Dropdown storyMemberDropdown;
    public TMP_InputField descriptionText;

    public TMP_Text errorText;

    bool isNewCastCreated;

    int projectId;

    System.Action<bool> backAction;
    public void SetView(int projectId, System.Action<bool> action)
    {
        this.projectId = projectId;
        parentPanel.gameObject.SetActive(true);
        backAction = action;
        isNewCastCreated = false;

        GameManager.Instance.apiHandler.GetProjectCharacters(projectId, (status, response) => {
            Debug.Log("GetProjectCharacters : " + response);
            storyCharacterDropdown.options.Clear();
        });
    }
    public void BackButtonAction()
    {
        parentPanel.gameObject.SetActive(false);
        backAction?.Invoke(isNewCastCreated);
        backAction = null;
    }
    public void CreateCastButtonAction()
    {
        //Call an API to add into audition list
        if (string.IsNullOrEmpty(storyCharacterDropdown.captionText.text))
        {
            ShowErrorMessage("Select character for casting");
            return;
        }
        if (string.IsNullOrEmpty(storyMemberDropdown.captionText.text))
        {
            ShowErrorMessage("Select member for casting");
            return;
        }
        if (string.IsNullOrEmpty(descriptionText.text))
        {
            ShowErrorMessage("Cast description should not be empty");
            return;
        }
        Dictionary<string, object> parameters = new Dictionary<string, object>();
               
        parameters.Add("project_id", projectId);
        parameters.Add("story_character_id", storyCharacterDropdown.captionText.text);
        parameters.Add("selected_member", storyMemberDropdown.captionText.text);
        parameters.Add("description", descriptionText.text);

        GameManager.Instance.apiHandler.CreateProjectCast(parameters, (status, response) => {
            Debug.Log("OnCreateCast : "+response);
            if (status)
            {
                isNewCastCreated = true;
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

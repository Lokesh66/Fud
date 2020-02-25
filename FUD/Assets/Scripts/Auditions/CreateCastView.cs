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
    public TMP_InputField storyMemberField;
    public TMP_InputField descriptionText;

    public TMP_Text errorText;

    bool isNewCastCreated;

    int projectId;

    public RectTransform searchContent;
    public GameObject scrollObject;
    public GameObject searchCell;

    UserSearchModel selectedModel = null;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;
    List<ProjectCharacter> characters = new List<ProjectCharacter>();

    System.Action<bool> backAction;
    public void SetView(int projectId, List<ProjectCharacter> characters, System.Action<bool> action)
    {
        this.projectId = projectId;
        backAction = action;
        isNewCastCreated = false;
        this.characters = characters;
        storyCharacterDropdown.options.Clear();
        foreach (ProjectCharacter character in characters)
        {
            storyCharacterDropdown.options.Add(new TMP_Dropdown.OptionData() { text = character.title });
        }
        parentPanel.gameObject.SetActive(true);
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
        if (selectedModel == null)
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
        ProjectCharacter character = characters.Find(item => item.title.Equals(storyCharacterDropdown.captionText.text));

        parameters.Add("story_character_id", character.id);
        parameters.Add("selected_member", selectedModel.id);
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

    void GetSearchedUsers()
    {
        GameManager.Instance.apiHandler.SearchTeamMember(keyword, (status, response) =>
        {
            if (status)
            {
                UserSearchResponse searchResponse = JsonUtility.FromJson<UserSearchResponse>(response);

                PopulateDropdown(searchResponse.data);

                isSearchAPICalled = false;
            }
        });
    }

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as UserSearchModel;

        storyMemberField.text = selectedModel.name;

        searchContent.DestroyChildrens();

        scrollObject.SetActive(false);
    }

    public void OnValueChange()
    {
        if (selectedModel == null)
        {
            if (storyMemberField.text.Length > 2 && !isSearchAPICalled)
            {
                //Call Search API
                isSearchAPICalled = true;

                keyword = storyMemberField.text;

                GetSearchedUsers();
            }
        }
    }

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        if (searchModels.Count > 0)
        {
            scrollObject.SetActive(true);

            for (int i = 0; i < searchModels.Count; i++)
            {
                cellObject = Instantiate(searchCell, searchContent);

                cellObject.GetComponent<UserSearchCell>().SetView(searchModels[i], OnSelectMember);
            }
        }
    }
}

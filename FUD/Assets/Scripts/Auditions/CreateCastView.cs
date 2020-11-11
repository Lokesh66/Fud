using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(storyCharacterDropdown.captionText.text))
        {
            errorMessage = "Select character for casting";            
        }
        else if (selectedModel == null)
        {
            errorMessage = "Select member for casting";
        }
        else if (string.IsNullOrEmpty(descriptionText.text))
        {
            errorMessage = "Cast description should not be empty";
        }
        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            UIManager.Instance.ShowAlert(alertModel);
            return;
        }

        Dictionary<string, object> parameters = new Dictionary<string, object>();
               
        parameters.Add("project_id", projectId);
        ProjectCharacter character = characters.Find(item => item.title.Equals(storyCharacterDropdown.captionText.text));

        parameters.Add("story_character_id", character.id);
        parameters.Add("selected_member", selectedModel.id);
        parameters.Add("description", descriptionText.text);

        GameManager.Instance.apiHandler.CreateProjectCast(parameters, (status, response) => {
            if (status)
            {
                isNewCastCreated = true;

                AlertModel alertModel = new AlertModel();
                alertModel.message = "Cast Added Successfully";
                alertModel.okayButtonAction = BackButtonAction;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Cast Creation Failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
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

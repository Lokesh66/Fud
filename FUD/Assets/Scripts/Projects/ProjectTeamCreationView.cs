using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class ProjectTeamCreationView : MonoBehaviour
{
    public RectTransform searchContent;

    public TMP_InputField teamNameField;

    public TMP_InputField descriptionField;

    public TMP_InputField memberField;

    public TMP_Dropdown accessDropdown;

    public GameObject searchCell;

    public GameObject scrollObject;



    UserSearchModel selectedModel;

    List<UserSearchModel> addedModels = new List<UserSearchModel>();

    Action<ProjectTeamModel> OnAddedTeam; 

    bool isSearchAPICalled = false;

    string keyword = string.Empty;

    string inputData = string.Empty;

    string apiResponse = string.Empty;



    public void SetView(Action<ProjectTeamModel> OnAddedTeam)
    {
        this.OnAddedTeam = OnAddedTeam;
    }

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        scrollObject.SetActive(searchModels.Count > 0);

        for (int i = 0; i < searchModels.Count; i++)
        {
            cellObject = Instantiate(searchCell, searchContent);

            cellObject.GetComponent<UserSearchCell>().SetView(searchModels[i], OnSelectMember);
        }
    }

    public void OnValueChange()
    {
        string addedMember = string.Empty;

        if (addedModels.Count > 0)
        {
            addedMember = memberField.text.Split(',').Last();
        }
        else {
            addedMember = memberField.text;
        }

        if (addedMember.Length > 2 && !isSearchAPICalled)
        {
            isSearchAPICalled = true;

            keyword = addedMember;

            GetSearchedUsers();
        }
    }

    public void OnButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        string[] membersList = memberField.text.Split(',');

        List<string> member = new List<string>(membersList);

        string members = GetMemberIds(member);

        int projectId = ProjectsDetailedView.Instance.GetCurrentProjectModel().id;

        GameManager.Instance.apiHandler.CreateProjectTeam(projectId, teamNameField.text, descriptionField.text, members, accessDropdown.value, (status, response) =>
        {
            if (status)
            {
                apiResponse = response;
            }

            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        UpdatedTeamModel responseModel = JsonUtility.FromJson<UpdatedTeamModel>(apiResponse);

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Project Team Creation Success" : responseModel.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        UpdatedProjectTeamModel responseModel = JsonUtility.FromJson<UpdatedProjectTeamModel>(apiResponse);

        ProjectTeamModel teamModel = responseModel.data;

        OnAddedTeam?.Invoke(teamModel);

        Destroy(gameObject);

        apiResponse = string.Empty;
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(teamNameField.text))
        {
            errorMessage = "Team title should not be empty";
        }
        else if (string.IsNullOrEmpty(memberField.text))
        {
            errorMessage = "Add the team members";
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

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as UserSearchModel;

        if (!inputData.Contains(selectedModel.name))
        {
            addedModels.Add(selectedModel);

            memberField.text = string.Empty;

            memberField.text += inputData + selectedModel.name + ",";

            inputData = memberField.text;

            keyword = string.Empty;
        }
        else {
            memberField.text = inputData;
        }
        searchContent.DestroyChildrens();

        scrollObject.SetActive(false);
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

    string GetMemberIds(List<string> members)
    {
        members.Remove(members[members.Count - 1]);

        string memberIds = string.Empty;

        string appendString = string.Empty;

        for (int i = 0; i < members.Count; i++)
        {
            UserSearchModel addModel = addedModels.Find(searchModel => searchModel.name.Equals(members[i]));

            appendString = (i + 1 != members.Count) ? "," : string.Empty;
 
            if (addModel != null)
            {
                memberIds += addModel.id + appendString;
            }
        }

        return memberIds;
    }
}

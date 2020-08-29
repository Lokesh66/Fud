using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using TMPro;


public class ProjectTeamUpdateView : MonoBehaviour
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

    Action<ProjectTeamModel> OnTeamUpdated;

    bool isSearchAPICalled = false;

    string keyword = string.Empty;

    string inputData = string.Empty;

    string apiResponse = string.Empty;


    ProjectTeamModel teamModel;


    public void Load(ProjectTeamModel teamModel, Action<ProjectTeamModel> OnUpdation)
    {
        gameObject.SetActive(true);

        this.teamModel = teamModel;

        this.OnTeamUpdated = OnUpdation;

        SetView();
    }

    public void SetView()
    {
        teamNameField.text = teamModel.title;

        descriptionField.text = teamModel.description;

        List<TeamMembersItem> membersItem = teamModel.TeamMembers.FindAll(item => item.users.id == 0);

        foreach (var item in membersItem)
        {
            teamModel.TeamMembers.Remove(item);
        }

        StartCoroutine(UpdateTeamMembers());
    }

    IEnumerator UpdateTeamMembers()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < teamModel.TeamMembers.Count; i++)
        {
            UserSearchModel searchModel = new UserSearchModel();

            searchModel.id = teamModel.TeamMembers[i].users.id;

            searchModel.name = teamModel.TeamMembers[i].users.name;

            addedModels.Add(searchModel);

            inputData = memberField.text += teamModel.TeamMembers[i].users.name + ",";
        }
    }

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        scrollObject.SetActive(searchModels.Count != 0);

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
        else
        {
            addedMember = memberField.text;
        }

        if (addedMember.Length > 2 && !isSearchAPICalled)
        {
            isSearchAPICalled = true;

            keyword = addedMember;

            GetSearchedUsers();
        }
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);

        ClearData();
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

        Project currentProjectModel = ProjectsDetailedView.Instance.GetCurrentProjectModel();

        string title = currentProjectModel.title;

        int projectId = currentProjectModel.id;

        GameManager.Instance.apiHandler.UpdateProjectTeam(projectId, title, teamModel.id, descriptionField.text, members, accessDropdown.value, (status, response) =>
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
        UpdatedProjectTeamModel responseModel = JsonUtility.FromJson<UpdatedProjectTeamModel>(apiResponse);

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Project Team Updation Success" : responseModel.message;

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

        OnTeamUpdated?.Invoke(teamModel);

        OnTeamUpdated = null;

        OnBackButtonAction();
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

            searchContent.DestroyChildrens();
        }
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

    void ClearData()
    {
        searchContent.DestroyChildrens();

        apiResponse = inputData = keyword = teamNameField.text = memberField.text = string.Empty;

        selectedModel = null;

        isSearchAPICalled = false;
    }
}

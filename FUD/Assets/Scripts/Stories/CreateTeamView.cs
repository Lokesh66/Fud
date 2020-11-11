using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

public class CreateTeamView : MonoBehaviour
{
    public RectTransform searchContent;

    public TMP_InputField teamNameField;

    public TMP_InputField descriptionField;

    public TMP_InputField memberField;

    public TMP_Dropdown accessDropdown;

    public GameObject searchCell;

    public GameObject scrollObject;


    StoryDetailsModel detailsModel;

    UserSearchModel selectedModel;

    List<UserSearchModel> addedModels = new List<UserSearchModel>();

    Action<StoryTeamModel> OnAddedTeam; 

    bool isSearchAPICalled = false;

    string keyword = string.Empty;

    string inputData = string.Empty;

    string apiResponse = string.Empty;



    public void SetView(StoryDetailsModel detailsModel, Action<StoryTeamModel> OnAddedTeam)
    {
        this.detailsModel = detailsModel;

        this.OnAddedTeam = OnAddedTeam;
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
        else {
            addedMember = memberField.text;
        }

        Debug.Log("addedMember = " + addedMember);

        if (addedMember.Length > 2 && !isSearchAPICalled)
        {
            isSearchAPICalled = true;

            keyword = addedMember;

            GetSearchedUsers();
        }
        else {
            if (string.IsNullOrEmpty(memberField.text))
            {
                searchContent.DestroyChildrens();

                scrollObject.SetActive(false);
            }
        }
    }

    public void OnButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        memberField.text = inputData;

        string[] membersList = memberField.text.Split(',');

        List<string> member = new List<string>(membersList);

        string members = GetMemberIds(member);

        GameManager.Instance.apiHandler.CreateStoryTeam(detailsModel.id, teamNameField.text, descriptionField.text, members, 1 + accessDropdown.value, (status, response) =>
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

        alertModel.message = status ? "Story Team Creation Success" : responseModel.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        UpdatedTeamModel responseModel = JsonUtility.FromJson<UpdatedTeamModel>(apiResponse);

        StoryTeamModel teamModel = responseModel.data;

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
        else if (addedModels.Count <= 0)
        {
            errorMessage = "Invalid member name";
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

        Debug.Log("GetMemberIds : members.Count = " + members.Count);

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

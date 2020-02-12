using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTeamView : MonoBehaviour
{
    public RectTransform searchContent;

    public TMP_InputField teamNameField;

    public TMP_InputField memberField;

    public GameObject searchCell;


    StoryDetailsModel detailsModel;

    UserSearchModel selectedModel;

    bool isSearchAPICalled = false;

    string keyword = string.Empty;


    public void SetView(StoryDetailsModel detailsModel)
    {
        this.detailsModel = detailsModel;
    }

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        for (int i = 0; i < searchModels.Count; i++)
        {
            cellObject = Instantiate(searchCell, searchContent);

            cellObject.GetComponent<UserSearchCell>().SetView(searchModels[i], OnSelectMember);
        }
    }

    public void OnValueChange()
    {
        if (memberField.text.Length > 2 && !isSearchAPICalled)
        {
            isSearchAPICalled = true;

            keyword = memberField.text;

            GetSearchedUsers();
        }
    }

    public void OnButtonAction()
    {
        string[] membersList = memberField.text.Split(',');

        List<string> member = new List<string>(membersList);

        string members = GetMemberIds(member);

        GameManager.Instance.apiHandler.UpdateStoryTeam(detailsModel.id, detailsModel.title, members, (status, response) =>
        {

            if (status)
            {
                Destroy(gameObject);
            }
        });
    }

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as UserSearchModel;

        memberField.text += "," + selectedModel.name;
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
        List<int> memberIds = new List<int>();

       /* for (int i = 0; i < members.Length; i++)
        { 
            int memberId = members.Fin
            memberIds.Add()
        }*/

        return memberIds.ToString();
    }
}

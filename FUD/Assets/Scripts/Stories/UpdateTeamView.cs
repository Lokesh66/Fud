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
        /* GameManager.Instance.apiHandler.UpdateTeamDetails(detailsModel.id, detailsModel.title, detailsModel.description, genderLabel.text, (status, response) => {

             if (status)
             {
                 Reset();
             }
         });*/
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
                PopulateDropdown(new List<UserSearchModel>());

                isSearchAPICalled = false;
            }
        });
    }
}

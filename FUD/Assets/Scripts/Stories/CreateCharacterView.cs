using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateCharacterView : MonoBehaviour
{
    public RectTransform searchContent;

    public TMP_InputField castField;

    public TextMeshProUGUI genderLabel;

    public TMP_InputField suitableField;

    public TMP_InputField descriptionField;

    public GameObject searchCell;


    StoryDetailsModel detailsModel;

    UserSearchModel selectedModel = null;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;


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
        if (suitableField.text.Length > 2 && !isSearchAPICalled)
        {
            //Call Search API
            isSearchAPICalled = true;

            keyword = suitableField.text;

            GetSearchedUsers();
        }
    }

    public void OnButtonAction()
    {
        GameManager.Instance.apiHandler.UpdateCharacterDetails(detailsModel.id, detailsModel.title, detailsModel.description, genderLabel.text, selectedModel.id, (status, response) => {

            if (status)
            {
                Destroy(gameObject);
            }
        });
    }

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as UserSearchModel;

        suitableField.text = selectedModel.name;
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

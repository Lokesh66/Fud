using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class CreateCharacterView : MonoBehaviour
{
    public RectTransform searchContent;

    public TMP_InputField castField;

    public TextMeshProUGUI genderLabel;

    public TMP_InputField suitableField;

    public TMP_InputField descriptionField;

    public GameObject searchCell;

    public GameObject scrollObject;


    StoryDetailsModel detailsModel;

    UserSearchModel selectedModel = null;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;


    Action<StoryCharacterModel> OnCreateCharacter;

    public void SetView(StoryDetailsModel detailsModel, Action<StoryCharacterModel> action)
    {
        this.detailsModel = detailsModel;

        OnCreateCharacter = action;
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

    public void OnValueChange()
    {
        if (selectedModel == null)
        {
            if (suitableField.text.Length > 2 && !isSearchAPICalled)
            {
                //Call Search API
                isSearchAPICalled = true;

                keyword = suitableField.text;

                GetSearchedUsers();
            }
        }
    }

    public void OnButtonAction()
    {
        GameManager.Instance.apiHandler.UpdateCharacterDetails(detailsModel.id, detailsModel.title, detailsModel.description, genderLabel.text, selectedModel.id, (status, response) => {

            if (status)
            {
                UpdatedCharaterModel responseModel = JsonUtility.FromJson<UpdatedCharaterModel>(response);

                StoryCharacterModel characterModel = responseModel.data;

                OnCreateCharacter?.Invoke(characterModel);

                Destroy(gameObject);
            }
        });
    }

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as UserSearchModel;

        suitableField.text = selectedModel.name;

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
}

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PortfolioShareView : MonoBehaviour
{
    public RectTransform searchContent;

    public GameObject searchCell;

    public TMP_InputField searchField;

    public TMP_InputField commentField;


    PortfolioModel currentModel;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;

    private void OnEnable()
    {
        keyword = string.Empty;

        searchField.text = keyword;
    }

    public void Load(PortfolioModel portfolioModel)
    {
        gameObject.SetActive(true);

        currentModel = portfolioModel;
    }

    public void OnValueChange()
    {
        if (searchField.text.Length > 2 && !isSearchAPICalled)
        {
            //Call Search API
            isSearchAPICalled = true;

            keyword = searchField.text;

            GetSearchedUsers();
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

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        for (int i = 0; i < searchModels.Count; i++)
        {
            cellObject = Instantiate(searchCell, searchContent);

            cellObject.GetComponent<PortfolioSearchCell>().SetView(searchModels[i], OnSelectMember);
        }
    }

    void OnSelectMember(object id)
    {
        int userId = (int)id;

        string storyTitle = StoryDetailsController.Instance.GetStoryTitle();

        GameManager.Instance.apiHandler.PostPortfolio(currentModel.id, commentField.text, userId, (status, response) => {

            if (status)
            {
                Reset();

                OnBackAction();
            }
        });
    }

    public void OnBackAction()
    {
        Reset();

        gameObject.SetActive(false);
    }

    void Reset()
    {
        searchField.text = commentField.text = string.Empty;

        searchContent.DestroyChildrens();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StoryShareView : MonoBehaviour
{
    public RectTransform searchContent;

    public GameObject searchCell;

    public TMP_InputField searchField;

    public TMP_InputField commentField;


    StoryVersion currentVersion;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;

    private void OnEnable()
    {
        keyword = string.Empty;

        searchField.text = keyword;
    }

    public void Load(StoryVersion storyVersion)
    {
        gameObject.SetActive(true);

        currentVersion = storyVersion;
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

            cellObject.GetComponent<ShareSearchCell>().SetView(searchModels[i], OnSelectMember);
        }
    }

    void OnSelectMember(object id)
    {
        if (!CanCallAPI())
        {
            return;
        }

        int userId = (int)id;

        string storyTitle = StoryDetailsController.Instance.GetStoryTitle();

        GameManager.Instance.apiHandler.UpdateStoryPost(currentVersion.story_id, currentVersion.id, storyTitle, commentField.text, userId, (status, response) => {

            if (status)
            {
                Reset();

                OnBackAction();
            }

        });
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(commentField.text))
        {
            errorMessage = "Comment field should not be empty";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            CanvasManager.Instance.alertView.ShowAlert(alertModel);
            return false;
        }

        return true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StoryShareView : MonoBehaviour
{
    public RectTransform searchContent;

    public GameObject searchCell;

    public TMP_InputField searchField;


    StoryVersionsView versionsView;

    StoryVersion currentVersion;

    UserSearchModel selectedModel = null;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;


    private void OnEnable()
    {
        keyword = string.Empty;

        searchField.text = keyword;
    }

    public void Load(StoryVersion storyVersion, StoryVersionsView versionsView)
    {
        gameObject.SetActive(true);

        currentVersion = storyVersion;

        this.versionsView = versionsView;
    }

    public void OnValueChange()
    {
        if (selectedModel == null)
        {
            if (searchField.text.Length > 2 && !isSearchAPICalled)
            {
                //Call Search API
                isSearchAPICalled = true;

                keyword = searchField.text;

                GetSearchedUsers();
            }
        }
        else
        {
            if (!searchField.text.Equals(selectedModel.name))
            {
                selectedModel = null;
            }
        }
    }

    public void OnShareButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        int userId = selectedModel.id;

        string storyTitle = StoryDetailsController.Instance.GetStoryTitle();

        GameManager.Instance.apiHandler.UpdateStoryPost(currentVersion.story_id, currentVersion.id, storyTitle, userId, (status, response) => {

            OnAPIResponse(status, response);
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

    void OnSelectMember(object searchModel)
    {
        selectedModel = searchModel as UserSearchModel;

        searchField.text = selectedModel.name;

        searchContent.DestroyChildrens();
    }

    void OnAPIResponse(bool status, string response)
    {
        BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(response); 

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Version Share Success" : responseModel.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        Reset();

        OnBackAction();

        versionsView?.OnRemoveVersion(currentVersion);

        DataManager.Instance.UpdateFeaturedData(EFeatureType.ShareStoryVersion);
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (null == selectedModel)
        {
            errorMessage = "Please Select the member to share";
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

    public void OnBackAction()
    {
        Reset();

        gameObject.SetActive(false);
    }

    void Reset()
    {
        searchField.text = string.Empty;

        selectedModel = null;

        searchContent.DestroyChildrens();
    }
}

using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StoryShareView : MonoBehaviour
{
    public ProfileInfoDetailView profileInfoView;

    public GameObject shareButtonObject;

    public StoryShareTableView tableView;

    public StoryShareFilterView filterView;

    public NoDataView noDataView;


    [HideInInspector]
    public List<UserSearchModel> dataList;


    StoryVersionsView versionsView;

    StoryVersion currentVersion;

    ShareSearchCell currentCell;

    UserSearchModel selectedModel = null;


    string keyword = string.Empty;

    string sourceFrom = "story_share";

    bool isSearchAPICalled = false;


    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_USERS = 50;


    private void OnEnable()
    {
        keyword = string.Empty;

        //searchField.text = keyword;
    }

    public void Load(StoryVersion storyVersion, StoryVersionsView versionsView)
    {
        gameObject.SetActive(true);

        currentVersion = storyVersion;

        this.versionsView = versionsView;

        GameManager.Instance.apiHandler.GetAllUsers(pageNo, sourceFrom, (status, dataList) => {

            if (status)
            {
                this.dataList = dataList;

                if (dataList.Count < MAX_USERS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                if (!isInitialized)
                {
                    tableView.gameObject.SetActive(true);

                    isInitialized = true;
                }
                else
                {
                    tableView.Data.Clear();

                    tableView.Data.Add(dataList.Count);

                    tableView.Refresh();
                }
            }
        });
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

                Reload(searchResponse.data);

                isSearchAPICalled = false;
            }
        });
    }

    void Reload(List<UserSearchModel> searchModels)
    {
        dataList = searchModels;

        tableView.Data.Clear();

        tableView.Data.Add(searchModels.Count);

        tableView.Refresh();
    }

    public void OnCellButtonAction(object searchModel)
    {
        selectedModel = searchModel as UserSearchModel;

        GameManager.Instance.apiHandler.GetUserInfo(selectedModel.id, (status, userInfo) => {

            if (status)
            {
                profileInfoView.Load(userInfo);
            }

        });        
    }

    public void OnSelectMember(ShareSearchCell selectedCell, object searchModel)
    {
        selectedModel = searchModel as UserSearchModel;

        //searchField.text = selectedModel.name;

        currentCell?.UpdateDeselectViw();

        currentCell = selectedCell;

        shareButtonObject.SetActive(currentCell != null);
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

        StoryDetailsController.Instance.OnBackButtonAction(true);
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

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetAllUsers(pageNo, sourceFrom, (status, modelsList) =>
        {
            if (status)
            {
                pageNo++;

                this.dataList = modelsList;

                if (modelsList.Count < MAX_USERS)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                tableView.Data.Clear();

                tableView.Data.Add(modelsList.Count);

                tableView.Refresh();
            }
        });
    }

    void Reset()
    {
        //searchField.text = string.Empty;

        selectedModel = null;

        pageNo = 1;

        isPagingOver = false;

        noDataView.gameObject.SetActive(false);

        shareButtonObject.SetActive(false);
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        dataList = data as List<UserSearchModel>;

        tableView.Data.Clear();

        tableView.Data.Add(dataList.Count);

        tableView.Refresh();

        noDataView.gameObject.SetActive(dataList.Count == 0);
    }
}

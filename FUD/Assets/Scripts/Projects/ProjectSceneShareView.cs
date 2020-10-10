using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ProjectSceneShareView : MonoBehaviour
{
    public ProfileInfoDetailView profileInfoView;

    public GameObject shareButtonObject;

    public TMP_InputField searchField;

    public ProjectSceneShareTableView tableView;

    public Toggle selectAllToggle;

    public NoDataView noDataView;


    [HideInInspector]
    public List<SceneAlbumModel> dataList;

    [HideInInspector]
    public List<SceneAlbumCell> cellsList;


    StoryVersionsView versionsView;

    StoryVersion currentVersion;

    SceneAlbumCell currentCell;

    SceneAlbumModel selectedModel = null;

    SceneModel sceneModel;

    List<int> selectedCharacters = new List<int>();



    bool isInitialized = false;

    bool isPagingOver = false;


    public void Load(SceneModel sceneModel)
    {
        this.sceneModel = sceneModel;

        gameObject.SetActive(true);

        GameManager.Instance.apiHandler.GetSceneMembers(sceneModel.project_id, (status, dataList) => {

            if (status)
            {
                this.dataList = dataList;

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

                noDataView.gameObject.SetActive(dataList.Count == 0);
            }
        });
    }

    public void OnSelectAllButtonAction(Toggle selectAllToggle)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            dataList[i].isSeeAllSelected = selectAllToggle.isOn;

            UpdateSelectAllView(selectAllToggle.isOn, dataList[i]);
        }

        tableView.Data.Clear();

        tableView.Data.Add(dataList.Count);

        tableView.Refresh();
    }

    public void OnShareButtonAction()
    {
        bool isAllUsersSelected = selectedCharacters.Count == dataList.Count;

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("scene_id", sceneModel.id);

        parameters.Add("send_to", isAllUsersSelected ? 0 : 1);

        parameters.Add("scene_characters", selectedCharacters);

        parameters.Add("status", 1);

        GameManager.Instance.apiHandler.ShareProjectScene(parameters, (status, response) => {

            OnAPIResponse(status, response);
        });
    }

    void Reload(List<UserSearchModel> searchModels)
    {
        //dataList = searchModels;

        tableView.Data.Clear();

        tableView.Data.Add(searchModels.Count);

        tableView.Refresh();
    }

    public void OnCellButtonAction(object searchModel)
    {
        selectedModel = searchModel as SceneAlbumModel;

        GameManager.Instance.apiHandler.GetUserInfo(selectedModel.Users.id, (status, userInfo) => {

            if (status)
            {
                profileInfoView.Load(userInfo);
            }

        });        
    }

    public void UpdateSelectAllView(bool isEnabled, object searchModel)
    {
        ProjectOfferedModel selectedModel = searchModel as ProjectOfferedModel;

        if (isEnabled)
        {
            if (!selectedCharacters.Contains(selectedModel.Users.id))
            {
                selectedCharacters.Add(selectedModel.Users.id);
            }
        }
        else {
            selectedCharacters.Remove(selectedModel.Users.id);
        }

        shareButtonObject.SetActive(selectedCharacters.Count > 0);
    }

    public void OnSelectMember(SceneAlbumCell selectedCell, object searchModel)
    {
        ProjectOfferedModel selectedModel = searchModel as ProjectOfferedModel;

        if (!selectedCharacters.Contains(selectedModel.Users.id))
        {
            selectedCharacters.Add(selectedModel.Users.id);
        }
        else
        {
            selectedCharacters.Remove(selectedModel.Users.id);

            selectedCell.UpdateDeselectView();
        }

        shareButtonObject.SetActive(selectedCharacters.Count > 0);
    }

    void OnAPIResponse(bool status, string response)
    {
        BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(response); 

        AlertModel alertModel = new AlertModel();

        alertModel.message = responseModel.message;

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

        DataManager.Instance.UpdateFeaturedData(EFeatureType.ShareStoryVersion);
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

        //GetNextPageData();
    }

    void Reset()
    {
        //searchField.text = string.Empty;

        selectedModel = null;

        selectAllToggle.isOn = false;

        selectedCharacters.Clear();

        isPagingOver = false;

        noDataView.gameObject.SetActive(false);

        shareButtonObject.SetActive(false);
    }
}

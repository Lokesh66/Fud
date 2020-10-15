using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class CreateSceneAlbumView : MonoBehaviour
{
    public ProfileInfoDetailView profileInfoView;

    public SceneAlbumsTableView tableView;

    public CreateSceneAlbumView projectCharactersView;

    public GameObject noDataObject;

    public bool canAddCharacters;


    [HideInInspector]
    public List<SceneAlbumModel> dataList;

    List<SceneAlbumModel> selectedCharacters = new List<SceneAlbumModel>();

    int MAX_USERS = 50;

    int projectId;

    bool isInitialized = false;


    public void Load(int projectId)
    {
        gameObject.SetActive(true);

        if (dataList != null && dataList.Count > 0)
        {
            return;
        }

        GameManager.Instance.apiHandler.GetSceneMembers(projectId, (status, dataList) => {

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

                noDataObject.SetActive(dataList.Count == 0);
            }
        });
    }

    void Reload()
    {
        if (dataList != null && dataList.Count > 0)
        {
            if (isInitialized)
            {
                tableView.Data.Clear();

                tableView.Data.Add(dataList.Count);

                tableView.Refresh();
            }
            else {
                tableView.gameObject.SetActive(true);

                isInitialized = true;

                noDataObject.SetActive(dataList.Count == 0);
            }
        }
    }

    public void SetSceneCharactersView(int projectId)
    {
        gameObject.SetActive(true);

        this.projectId = projectId;

        dataList = projectCharactersView.GetSelectedCharacters();

        

        Reload();
    }

    public void OnSelectMember(SceneAlbumCell selectedCell, object searchModel)
    {
        SceneAlbumModel selectedModel = searchModel as SceneAlbumModel;

        if (!selectedCharacters.Contains(selectedModel))
        {
            selectedCharacters.Add(selectedModel);
        }
        else
        {
            selectedCharacters.Remove(selectedModel);

            selectedCell.UpdateDeselectView();
        }
    }

    public void OnCellButtonAction(object searchModel)
    {
        SceneAlbumModel selectedModel = searchModel as SceneAlbumModel;

        GameManager.Instance.apiHandler.GetUserInfo(selectedModel.Users.id, (status, userInfo) => {

            if (status)
            {
                profileInfoView.Load(userInfo);
            }
        });
    }

    public List<SceneAlbumModel> GetSelectedCharacters()
    {
        return selectedCharacters;
    }

    public void OnCloseProjectCharactersView()
    {
        SetSceneCharactersView(projectId);
    }

    public void ClearData()
    {
        selectedCharacters.Clear();

        dataList = null;
    }

    #region Button Actions

    public void OnAddSceneCharactersButtonAction()
    {
        projectCharactersView.Load(projectId);
    }

    public void OnCloseButtonAction()
    {
        gameObject.SetActive(false);
    }

    #endregion
}

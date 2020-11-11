using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class UpdateSceneAlbumView : MonoBehaviour
{
    public ProfileInfoDetailView profileInfoView;

    public UpdateSceneAlbumsTableView tableView;

    public UpdateSceneAlbumView projectCharactersView;

    public GameObject noDataObject;

    public bool canAddCharacters;


    [HideInInspector]
    public List<SceneCharacter> dataList;

    List<SceneAlbumModel> selectedCharacters = new List<SceneAlbumModel>();

    List<SceneCharacter> sceneCharacters = new List<SceneCharacter>(); 

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
                sceneCharacters.Clear();

                UpdateLocalModel(dataList);

                if (!isInitialized)
                {
                    tableView.gameObject.SetActive(true);

                    isInitialized = true;
                }
                else
                {
                    tableView.Data.Clear();

                    tableView.Data.Add(sceneCharacters.Count);

                    tableView.Refresh();
                }

                noDataObject.SetActive(dataList.Count == 0);
            }
        });
    }

    void UpdateLocalModel(List<SceneAlbumModel> dataList)
    {
        SceneCharacter sceneCharacter = null;

        foreach (var model in dataList)
        {
            sceneCharacter = new SceneCharacter();

            sceneCharacter.id = model.id;

            sceneCharacter.Users = new UserData();

            sceneCharacter.Users.id = model.user_id;

            sceneCharacter.Users.name = model.Users.name;

            sceneCharacter.Users.profile_image = model.Users.profile_image;

            this.dataList.Add(sceneCharacter);
        }
    }

    void Reload()
    {
        tableView.Data.Clear();

        if (dataList != null && dataList.Count > 0)
        {
            tableView.Data.Add(sceneCharacters.Count);

            tableView.Refresh();
        }
    }

    public void SetSceneCharactersView()
    {
        dataList = projectCharactersView.GetSelectedCharacters();

        Reload();
    }
    
    public void SetSceneCharactersView(int projectId, List<SceneCharacter> sceneCharacters)
    {
        gameObject.SetActive(true);

        this.projectId = projectId;

        dataList = sceneCharacters;

        tableView.gameObject.SetActive(true);

        noDataObject.SetActive(dataList.Count == 0);
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

    public List<SceneCharacter> GetSelectedCharacters()
    {
        return sceneCharacters;
    }

    public void OnCloseProjectCharactersView()
    {
        SetSceneCharactersView();
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

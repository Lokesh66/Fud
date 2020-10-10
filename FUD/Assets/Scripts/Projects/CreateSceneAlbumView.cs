using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class CreateSceneAlbumView : MonoBehaviour
{
    public ProfileInfoDetailView profileInfoView;

    public SceneAlbumsTableView tableView;

    public GameObject noDataObject;


    [HideInInspector]
    public List<SceneAlbumModel> dataList;

    int MAX_USERS = 50;

    bool isInitialized = false;


    public void Load(int projectId)
    {
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
}

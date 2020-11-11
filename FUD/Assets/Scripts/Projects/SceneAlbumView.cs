using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class SceneAlbumView : MonoBehaviour
{
    public ProfileInfoDetailView profileInfoView;

    public SceneProfileTableView tableView;

    public GameObject noDataObject;


    [HideInInspector]
    public List<SceneCharacter> sceneCharacters;

    bool isInitialized = false;


    public void Load(List<SceneCharacter> sceneCharacters)
    {
        this.sceneCharacters = sceneCharacters;

        if (sceneCharacters == null && sceneCharacters.Count <= 0)
        {
            return;
        }

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

        noDataObject.SetActive(sceneCharacters.Count == 0);
    }

    public void OnCellButtonAction(object searchModel)
    {
        SceneCharacter selectedModel = searchModel as SceneCharacter;

        profileInfoView.Load(selectedModel.Users);
    }
}

using System.Collections.Generic;
using UnityEngine;


public class CreateDialoguesView : MonoBehaviour
{
    public CreateSceneAutoView autoView;

    public CreateSceneAlbumView sceneCharactersView;

    public CreateSceneAlbumView projectCharactersView;

    List<Dictionary<string, object>> autoDialogues = new List<Dictionary<string, object>>();

    List<Dictionary<string, int>> sceneCharacters = new List<Dictionary<string, int>>();

    int projectId;


    public void Load(int projectId)
    {
        this.projectId = projectId;

        gameObject.SetActive(true);

        autoView.EnableView(this);

        sceneCharactersView.SetSceneCharactersView(projectId);
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    public void SetAutoDialogues(List<Dictionary<string, object>> autoDialogues)
    {
        this.autoDialogues = autoDialogues;

        OnBackButtonAction();
    }

    public List<Dictionary<string, object>> GetDialogues()
    {
        return autoDialogues;
    }

    void SetSceneCharacters(List<SceneAlbumModel> albumModels)
    {
        sceneCharacters.Clear();

        for (int i = 0; i < albumModels.Count; i++)
        {
            sceneCharacters.Add(new Dictionary<string, int>() { { "character_id", albumModels[i].id } });
        }         
    }

    public List<Dictionary<string, int>> GetSceneCharacters()
    {
        return sceneCharacters;
    }

    public void OnAlbumCloseAction()
    {
        sceneCharactersView.SetSceneCharactersView(projectId);

        SetSceneCharacters(sceneCharactersView.dataList);

        projectCharactersView.OnCloseButtonAction();
    }
}

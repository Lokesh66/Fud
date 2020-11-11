using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UpdateSceneCharacterView : MonoBehaviour
{
    public enum ESubTabType
    {
        Manual,
        Auto,
    }

    public TextMeshProUGUI[] buttonsList;

    public UpdateSceneAutoView autoView;

    public UpdateSceneAlbumView sceneCharactersView;

    public UpdateSceneAlbumView projectCharactersView;


    public Color selectedColor;

    public Color disabledColor;


    GameObject currentObject;

    SceneDetailsModel detailsModel;

    List<Dictionary<string, object>> autoDialogues = new List<Dictionary<string, object>>();

    List<Dictionary<string, int>> sceneCharacters = new List<Dictionary<string, int>>();

    int projectId;


    public void Load(SceneDetailsModel detailsModel)
    {
        this.detailsModel = detailsModel;

        this.projectId = detailsModel.project_id;

        autoView.EnableView(this, detailsModel);

        gameObject.SetActive(true);
    }

    void SetSceneCharacters(List<SceneCharacter> albumModels)
    {
        sceneCharacters.Clear();

        for (int i = 0; i < albumModels.Count; i++)
        {
            sceneCharacters.Add(new Dictionary<string, int>() { { "character_id", albumModels[i].id } });
        }
    }

    public void OnBackButtonAction()
    {
        autoView.OnBackButtonAction();

        gameObject.SetActive(false);
    }

    public void SetAutoDialogues(List<Dictionary<string, object>> autoDialogues)
    {
        this.autoDialogues = autoDialogues;

        OnBackButtonAction();
    }

    public List<Dictionary<string, int>> GetSceneCharacters()
    {
        return sceneCharacters;
    }

    public List<Dictionary<string, object>> GetDialogues()
    {
        return autoDialogues;
    }

    public void OnAlbumCloseAction()
    {
        sceneCharactersView.SetSceneCharactersView();

        SetSceneCharacters(sceneCharactersView.dataList);

        projectCharactersView.OnCloseButtonAction();
    }

    public void ClearData()
    {
        autoView.ClearData();
    }
}

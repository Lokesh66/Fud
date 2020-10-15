using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CreateDialoguesView : MonoBehaviour
{
    public enum ESubTabType
    {
        Manual,
        Auto,
    }

    public TextMeshProUGUI[] buttonsList;


    public CreateSceneAutoView autoView;

    public CreateSceneAlbumView sceneCharactersView;

    public CreateSceneAlbumView projectCharactersView;


    public Color selectedColor;

    public Color disabledColor;


    GameObject currentObject;

    List<Dictionary<string, object>> autoDialogues = new List<Dictionary<string, object>>();

    List<Dictionary<string, object>> manualDialogues = new List<Dictionary<string, object>>();

    List<int> sceneCharacters = new List<int>();

    int projectId;

    private ESubTabType currentTab = ESubTabType.Auto;


    public void Load(int projectId)
    {
        //OnTabAction(0);

        this.projectId = projectId;

        gameObject.SetActive(true);

        autoView.EnableView(this);

        sceneCharactersView.SetSceneCharactersView(projectId);

        //albumView.Load(projectId);
    }

    #region Button Actions

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (ESubTabType)tabIndex)
        {
            buttonsList[(int)currentTab].color = disabledColor;

            buttonsList[tabIndex].color = selectedColor;

            currentTab = (ESubTabType)tabIndex;

            currentObject?.SetActive(false);
        }
    }

    #endregion

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);

        currentTab = ESubTabType.Auto;
    }

    public void SetAutoDialogues(List<Dictionary<string, object>> autoDialogues)
    {
        this.autoDialogues = autoDialogues;

        OnBackButtonAction();
    }

    public List<Dictionary<string, object>> GetDialogues(bool isManual)
    {
        return isManual ? manualDialogues : autoDialogues;
    }

    void SetSceneCharacters(List<SceneAlbumModel> albumModels)
    {
        sceneCharacters.Clear();

        for (int i = 0; i < albumModels.Count; i++)
        {
            sceneCharacters.Add(albumModels[i].id);
        }
    }

    public List<int> GetSceneCharacters()
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

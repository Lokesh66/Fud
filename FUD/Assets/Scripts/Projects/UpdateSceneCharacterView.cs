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


    public UpdateSceneManualView manualView;

    public UpdateSceneAutoView autoView;

    public UpdateSceneAlbumView sceneCharactersView;

    public UpdateSceneAlbumView projectCharactersView;


    public Color selectedColor;

    public Color disabledColor;


    GameObject currentObject;

    SceneDetailsModel detailsModel;

    List<Dictionary<string, object>> autoDialogues = new List<Dictionary<string, object>>();

    List<Dictionary<string, object>> manualDialogues = new List<Dictionary<string, object>>();

    private ESubTabType currentTab = ESubTabType.Auto;

    int projectId;


    public void Load(SceneDetailsModel detailsModel)
    {
        this.detailsModel = detailsModel;

        //OnTabAction(0);

        this.projectId = detailsModel.project_id;

        autoView.EnableView(this, detailsModel.ScenesMultimedia);

        gameObject.SetActive(true);
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

            UpdateCurrentView();
        }
    }

    #endregion

    void UpdateCurrentView()
    {
        switch (currentTab)
        {
            case ESubTabType.Manual:
                currentObject = manualView.gameObject;
                manualView.EnableView(detailsModel.SceneCharacters, this);
                break;

            case ESubTabType.Auto:
                currentObject = autoView.gameObject;
                autoView.EnableView(this, null);
                break;
        }
    }

    public void OnBackButtonAction()
    {
        autoView.OnBackButtonAction();

        gameObject.SetActive(false);

        currentTab = ESubTabType.Auto;
    }

    public void SetManualDialogues(List<Dictionary<string, object>> manualDialogues)
    {
        this.manualDialogues = manualDialogues;

        OnBackButtonAction();
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

    public void OnAlbumCloseAction()
    {
        sceneCharactersView.SetSceneCharactersView(projectId);

        projectCharactersView.OnCloseButtonAction();
    }

    public void ClearData()
    {
        manualView.ClearData();

        autoView.ClearData();
    }
}

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;

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

    public CreateSceneAlbumView albumView;


    public Color selectedColor;

    public Color disabledColor;


    GameObject currentObject;

    SceneDetailsModel detailsModel;

    List<Dictionary<string, object>> autoDialogues = new List<Dictionary<string, object>>();

    List<Dictionary<string, object>> manualDialogues = new List<Dictionary<string, object>>();

    private ESubTabType currentTab = ESubTabType.Auto;


    public void Load(SceneDetailsModel detailsModel)
    {
        this.detailsModel = detailsModel;

        //OnTabAction(0);

        gameObject.SetActive(true);

        albumView.Load(detailsModel.project_id);
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
                autoView.EnableView(this);
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

    public void ClearData()
    {
        manualView.ClearData();

        autoView.ClearData();
    }
}

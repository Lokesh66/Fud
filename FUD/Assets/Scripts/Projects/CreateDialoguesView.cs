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


    public CreateSceneManualView manualView;

    public CreateSceneAutoView autoView; 


    public Color selectedColor;

    public Color disabledColor;


    GameObject currentObject;

    List<Dictionary<string, object>> autoDialogues = new List<Dictionary<string, object>>();

    List<Dictionary<string, object>> manualDialogues = new List<Dictionary<string, object>>();

    private ESubTabType currentTab = ESubTabType.Auto;


    public void Load()
    {
        OnTabAction(0);

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
                manualView.EnableView(this);
                break;

            case ESubTabType.Auto:
                currentObject = autoView.gameObject;
                autoView.EnableView(this);
                break;
        }
    }

    public void OnBackButtonAction()
    {
        manualView.OnBackButtonAction();

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
}

using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SceneCharacterView : MonoBehaviour
{
    public enum ESceneTabType
    {
        Manual,
        Auto
    }


    public SceneManualView manualView;

    public SceneAutoView autoView;


    public TextMeshProUGUI[] buttonsList;


    public Color selectedColor;

    public Color disabledColor;


    GameObject currentObject;

    ESceneTabType currentTab;

    List<SceneCharacter> scenesList;


    public void Load(List<SceneCharacter> scensList)
    {
        this.scenesList = scensList;

        gameObject.SetActive(true);

        UpdateCurrentView();
    }

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (ESceneTabType)tabIndex)
        {
            buttonsList[(int)currentTab].color = disabledColor;

            buttonsList[tabIndex].color = selectedColor;

            currentTab = (ESceneTabType)tabIndex;

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    void UpdateCurrentView()
    {
        switch (currentTab)
        {
            case ESceneTabType.Manual:
                currentObject = manualView.gameObject;
                manualView.Load(scenesList);
                break;

            case ESceneTabType.Auto:
                currentObject = autoView.gameObject;
                autoView.Load(scenesList);
                break;
        }
    }

    public void OnBackAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void ClearData()
    {
        currentTab = ESceneTabType.Manual;

        manualView.ClearData();

        autoView.ClearData();
    }
}

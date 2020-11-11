using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SceneCharacterView : MonoBehaviour
{
   
    public SceneAutoView autoView;


    public TextMeshProUGUI[] buttonsList;


    public Color selectedColor;

    public Color disabledColor;


    GameObject currentObject;

    List<SceneCharacter> scenesList;


    public void Load(List<SceneCharacter> scensList, List<MultimediaModel> models)
    {
        this.scenesList = scensList;

        gameObject.SetActive(true);

        autoView.Load(scenesList, models);
    }

    public void OnBackAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void ClearData()
    {
        autoView.ClearData();
    }
}

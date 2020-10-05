using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAutoView : MonoBehaviour
{
    public ScenesDocumentHandler filesHandler;


    public void Load(List<SceneCharacter> sceneCharacters)
    {
        gameObject.SetActive(true);

        //filesHandler.Load(sceneCharacters, false, EMediaType.Document);
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    public void ClearData()
    {
        filesHandler.content.DestroyChildrens();
    }
}

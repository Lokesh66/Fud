using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAutoView : MonoBehaviour
{
    public ScenesDocumentHandler filesHandler;

    public SceneAlbumView sceneAlbumView;


    public void Load(List<SceneCharacter> sceneCharacters, List<MultimediaModel> models)
    {
        gameObject.SetActive(true);

        sceneAlbumView.Load(sceneCharacters);

        filesHandler.Load(models, EMediaType.Document);
    }

    public void OnBackButtonAction()
    {
        //gameObject.SetActive(false);
    }

    public void ClearData()
    {
        filesHandler.content.DestroyChildrens();
    }
}

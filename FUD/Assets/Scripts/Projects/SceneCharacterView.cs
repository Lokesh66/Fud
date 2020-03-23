using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCharacterView : MonoBehaviour
{
    public RectTransform content;

    public GameObject dialogueCell;


    bool isLeftAlign = true;


    public void Load(List<SceneCharacter> sceneCharacters)
    {
        gameObject.SetActive(true);

        for (int i = 0; i < sceneCharacters.Count; i++)
        {
            GameObject dialogueObject = Instantiate(dialogueCell, content);

            string fieldMessage = sceneCharacters[i].Users.name + " : " + sceneCharacters[i].dailogue;

            dialogueObject.GetComponent<DialogueCell>().SetView(fieldMessage, isLeftAlign, null);

            isLeftAlign = !isLeftAlign;
        }
    }

    public void OnBackAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void ClearData()
    {
        content.DestroyChildrens();
    }
}

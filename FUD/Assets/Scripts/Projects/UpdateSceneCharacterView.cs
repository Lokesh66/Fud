using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UpdateSceneCharacterView : MonoBehaviour
{
    public ScrollRect scrollRect;

    public GameObject dialogueCell;

    public Button enterButton;

    public TMP_InputField dialogueField;

    public RectTransform searchContent;

    public RectTransform searchScrollTrans;

    public GameObject searchCell;

    public GameObject scrollObject;


    UserSearchModel selectedModel;


    DialogueCell editedDialogueCell;

    List<Dictionary<string, object>> dialogues = new List<Dictionary<string, object>>();

    List<DialogueCell> dialogueCells = new List<DialogueCell>();

    string keyword = string.Empty;

    bool isLeftAlign = true;


    public void Load(List<SceneCharacter> sceneCharacters)
    {
        for (int i = 0; i < sceneCharacters.Count; i++)
        {
            GameObject dialogueObject = Instantiate(dialogueCell, scrollRect.content);

            string fieldMessage = sceneCharacters[i].Users.name + " : " + sceneCharacters[i].dailogue;

            dialogueObject.GetComponent<DialogueCell>().SetView(fieldMessage, isLeftAlign, null, OnCellButtonAction);

            isLeftAlign = !isLeftAlign;
        }
    }

    public void OnBackAction()
    {
        ClearData();

        gameObject.SetActive(false);
    }

    public void OnEnterButtonAction()
    {
        if (editedDialogueCell != null)
        {
            int index = dialogueCells.IndexOf(editedDialogueCell);

            //editedDialogueCell.SetView(dialogueField.text, editedDialogueCell.isLeftAlign, OnCellButtonAction);

            editedDialogueCell = null;
        }
        else
        {
            GameObject dialogueObj = Instantiate(dialogueCell, scrollRect.content);

            //dialogueObj.GetComponent<DialogueCell>().SetView(dialogueField.text, isLeftAlign, OnCellButtonAction);

            dialogueCells.Add(dialogueObj.GetComponent<DialogueCell>());

            Dictionary<string, object> sceneCharacter = new Dictionary<string, object>();

            sceneCharacter.Add("character_id", selectedModel.id);

            sceneCharacter.Add("dailogue", dialogueField.text);

            dialogues.Add(sceneCharacter);

            keyword = dialogueField.text = string.Empty;

            scrollRect.verticalNormalizedPosition = 0.0f;

            selectedModel = null;

            isLeftAlign = !isLeftAlign;
        }
    }

    public void ClearData()
    {
        scrollRect.content.DestroyChildrens();
    }

    void OnCellButtonAction(DialogueCell editedDialogueCell)
    {
        this.editedDialogueCell = editedDialogueCell;

        TouchScreenKeyboard.Open(editedDialogueCell.dialogueText.text);

        dialogueField.text = editedDialogueCell.dialogueText.text;
    }
}

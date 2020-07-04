using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;

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

    public RectTransform enterPanelTrans;


    UserSearchModel selectedModel;


    UpdateSceneView updateSceneView;

    DialogueCell editedDialogueCell;

    List<Dictionary<string, object>> dialogues = new List<Dictionary<string, object>>();

    string keyword = string.Empty;

    float searchScrollMaxHeight = 400.0f;

    float searchCellHeight = 100.0f;

    bool isLeftAlign = true;

    bool isSearchAPICalled = false;


    public void EnableView(List<SceneCharacter> sceneCharacters, UpdateSceneView updateSceneView)
    {
        gameObject.SetActive(true);

        this.updateSceneView = updateSceneView;

        dialogueField.onTouchScreenKeyboardStatusChanged.RemoveAllListeners();

        dialogueField.onTouchScreenKeyboardStatusChanged.AddListener(delegate { OnStatusChanged(); });

        Load(sceneCharacters);
    }
    void OnStatusChanged()
    {
        if (!dialogueField.wasCanceled)
        {
            enterPanelTrans.anchoredPosition = new Vector2(enterPanelTrans.anchoredPosition.x, 0.0f);
        }
    }

    void SetDialougeFieldPosition()
    {
        enterPanelTrans.anchoredPosition = new Vector2(enterPanelTrans.anchoredPosition.x, GetKeyboardHeight(true));
    }

    public void OnSelect()
    {
#if !UNITY_EDITOR
        Invoke("SetDialougeFieldPosition", 0.3f);
#endif
    }

    public void OnValueChange()
    {
        if (selectedModel == null)
        {
            enterButton.interactable = false;

            if (!isSearchAPICalled && selectedModel == null)
            {
                keyword = dialogueField.text;

                CallSearchAPI();
            }
        }
        else
        {
            int characterIndex = dialogueField.text.IndexOf(':');

            int startIndex = dialogueField.text.Contains("@") ? 1 : 0;

            int lastIndex = dialogueField.text.Contains(":") ? characterIndex - startIndex : selectedModel.name.Length - startIndex;

            string subString = dialogueField.text.Substring(startIndex, lastIndex);

            if (!subString.Equals(selectedModel.name) && !isSearchAPICalled)
            {
                keyword = subString;

                CallSearchAPI();
            }

            if (characterIndex + 2 < dialogueField.text.Length)
            {
                enterButton.interactable = true;
            }
            else
            {
                enterButton.interactable = false;
            }
        }
    }

    void CallSearchAPI()
    {
        if (keyword.IndexOf('@') == 0)
        {
            keyword = keyword.Remove(0, 1);
        }

        Debug.Log("keyword = " + keyword);

        if (keyword.Length > 2)
        {
            //Call Search API
            isSearchAPICalled = true;

            GetSearchedUsers();
        }
    }

    void GetSearchedUsers()
    {
        GameManager.Instance.apiHandler.SearchTeamMember(keyword, (status, response) =>
        {
            if (status)
            {
                UserSearchResponse searchResponse = JsonUtility.FromJson<UserSearchResponse>(response);

                PopulateDropdown(searchResponse.data);

                isSearchAPICalled = false;
            }
        });
    }

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        if (searchModels.Count > 0)
        {
            scrollObject.SetActive(true);

            float scrollHeight = searchModels.Count > 3 ? searchScrollMaxHeight : searchCellHeight * searchModels.Count;

            searchScrollTrans.sizeDelta = new Vector2(searchScrollTrans.rect.width, scrollHeight);

            searchScrollTrans.anchoredPosition = new Vector2(0, scrollHeight + searchCellHeight + searchCellHeight / 2);

            for (int i = 0; i < searchModels.Count; i++)
            {
                cellObject = Instantiate(searchCell, searchContent);

                cellObject.GetComponent<UserSearchCell>().SetView(searchModels[i], OnSelectMember);
            }
        }
    }

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as UserSearchModel;

        dialogueField.text = "@" + selectedModel.name + ":";

        searchContent.DestroyChildrens();

        scrollObject.SetActive(false);
    }

    void Load(List<SceneCharacter> sceneCharacters)
    {
        if (scrollRect.content.childCount <= 0)
        {
            for (int i = 0; i < sceneCharacters.Count; i++)
            {
                GameObject dialogueObject = Instantiate(dialogueCell, scrollRect.content);

                string fieldMessage = sceneCharacters[i].dailogue;

                UserSearchModel searchModel = new UserSearchModel();

                searchModel.id = sceneCharacters[i].character_id;

                searchModel.name = sceneCharacters[i].Users.name;

                DialogueCell _dialogueCell = dialogueObject.GetComponent<DialogueCell>();

                _dialogueCell.SetView(fieldMessage, isLeftAlign, searchModel, OnCellButtonAction, OnDeleteDialogueAction, sceneCharacters[i].id);

                //dialogueCells.Add(_dialogueCell);

                isLeftAlign = !isLeftAlign;
            }
        }
    }

    public void OnBackAction()
    {
        //ClearData();

        gameObject.SetActive(false);
    }

    public void OnEnterButtonAction()
    {
        Debug.Log("editedDialogueCell = " + editedDialogueCell);

        if (editedDialogueCell != null)
        {
            Dictionary<string, object> characterBody = new Dictionary<string, object>();

            if (editedDialogueCell.dialogueId != -1)
            {
                Dictionary<string, object> existedBody = dialogues.Find(item => item.TryGetValue("id", out object id) == true ? editedDialogueCell.dialogueId == (int)id : false);

                characterBody["character_id"] = selectedModel.id;

                characterBody["dailogue"] = dialogueField.text;

                characterBody["id"] = editedDialogueCell.dialogueId;

                if (existedBody != null)
                {
                    int index = dialogues.IndexOf(existedBody);

                    dialogues.RemoveAt(index);

                    dialogues.Insert(index, characterBody);
                }
                else
                {
                    dialogues.Add(characterBody);
                }

                editedDialogueCell.SetView(dialogueField.text, editedDialogueCell.isLeftAlign, editedDialogueCell.userSearchModel, OnCellButtonAction, OnDeleteDialogueAction, editedDialogueCell.dialogueId);
            }
            else {
                int index = dialogues.IndexOf(editedDialogueCell.dialogueModel);

                dialogues.RemoveAt(index);

                editedDialogueCell.SetView(dialogueField.text, editedDialogueCell.isLeftAlign, editedDialogueCell.userSearchModel, OnCellButtonAction, OnDeleteDialogueAction, editedDialogueCell.dialogueId);

                dialogues.Insert(index, editedDialogueCell.dialogueModel);
            }

            editedDialogueCell = null;

            /*
            int index = dialogueCells.IndexOf(editedDialogueCell);

            Debug.Log("index = " + index);

            Dictionary<string, object> characterBody = dialogues.ElementAt(index);

            characterBody["character_id"] = selectedModel.id;

            characterBody["dailogue"] = dialogueField.text;

            dialogues.RemoveAt(index);

            Debug.Log("After Removing");

            if (editedDialogueCell.dialogueId != -1)
            {
                characterBody["id"] = editedDialogueCell.dialogueId;
            }

            editedDialogueCell.SetView(dialogueField.text, editedDialogueCell.isLeftAlign, editedDialogueCell.userSearchModel, OnCellButtonAction, editedDialogueCell.dialogueId);

            dialogues.Insert(index, characterBody);

            editedDialogueCell = null;
            */
        }
        else
        {
            GameObject dialogueObj = Instantiate(dialogueCell, scrollRect.content);

            DialogueCell _dialogueCell = dialogueObj.GetComponent<DialogueCell>();

            _dialogueCell.SetView(dialogueField.text, isLeftAlign, selectedModel, OnCellButtonAction, OnDeleteDialogueAction);

           /* Dictionary<string, object> sceneCharacter = new Dictionary<string, object>();

            sceneCharacter.Add("character_id", selectedModel.id);

            sceneCharacter.Add("dailogue", dialogueField.text);*/

            dialogues.Add(_dialogueCell.dialogueModel);

            isLeftAlign = !isLeftAlign;
        }

        ClearFieldData();
    }

    public void OnSaveButtonAction()
    {
        updateSceneView.OnSaveDialogues(dialogues);

        ResetData();

        gameObject.SetActive(false);
    }

    public void ClearData()
    {
        dialogues.Clear();

        scrollRect.content.DestroyChildrens();
    }

    void ResetData()
    {
        selectedModel = null;

        keyword = dialogueField.text = string.Empty;

        searchContent.DestroyChildrens();
    }

    void OnCellButtonAction(DialogueCell editedDialogueCell)
    {
        this.editedDialogueCell = editedDialogueCell;

        selectedModel = editedDialogueCell.userSearchModel;

        TouchScreenKeyboard.Open(editedDialogueCell.dialogueText.text);

        dialogueField.text = editedDialogueCell.dialogueText.text;
    }

    void ClearFieldData()
    {
        enterButton.interactable = false;

        selectedModel = null;

        scrollRect.verticalNormalizedPosition = 0.0f;

        keyword = dialogueField.text = string.Empty;
    }

    void OnDeleteDialogueAction(DialogueCell dialogueCell)
    {
        int cellIndex = dialogueCell.transform.GetSiblingIndex();

        Destroy(scrollRect.content.GetChild(cellIndex).gameObject);

        if (dialogues.Contains(dialogueCell.dialogueModel))
        {
            dialogues.Remove(dialogues[cellIndex]);
        }

        ClearFieldData();
    }

    int GetKeyboardHeight(bool includeInput)
    {
#if UNITY_ANDROID
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            var view = unityPlayer.Call<AndroidJavaObject>("getView");
            var dialog = unityPlayer.Get<AndroidJavaObject>("b");

            if (view == null || dialog == null)
                return 0;

            var decorHeight = 0;

            if (includeInput)
            {
                var decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

                if (decorView != null)
                    decorHeight = decorView.Call<int>("getHeight");
            }

            using (var rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return Display.main.systemHeight - rect.Call<int>("height") + decorHeight;
            }
        }
#else
        var height = Mathf.RoundToInt(TouchScreenKeyboard.area.height);
        return height >= Display.main.systemHeight ? 0 : height;
#endif
    }
}

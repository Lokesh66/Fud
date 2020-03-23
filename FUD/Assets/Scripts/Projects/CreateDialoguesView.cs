using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CreateDialoguesView : MonoBehaviour
{
    public RectTransform enterPanelTrans;

    public ScrollRect scrollRect;

    public GameObject dialogueCell;

    public Button enterButton;

    public TMP_InputField dialogueField;

    public RectTransform searchContent;

    public RectTransform searchScrollTrans;

    public GameObject searchCell;

    public GameObject scrollObject;


    List<Dictionary<string, object>> dialogues = new List<Dictionary<string, object>>();

    List<DialogueCell> dialogueCells = new List<DialogueCell>();

    UserSearchModel selectedModel = null;

    bool isSearchAPICalled = false;

    bool isLeftAlign = true;

    string keyword = string.Empty;

    float searchScrollMaxHeight = 400.0f;

    float searchCellHeight = 100.0f;

    DialogueCell editedDialogueCell;


    public void EnableView()
    {
        gameObject.SetActive(true);

        dialogueField.onTouchScreenKeyboardStatusChanged.RemoveAllListeners();

        dialogueField.onTouchScreenKeyboardStatusChanged.AddListener(delegate { OnStatusChanged(); });
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

            //Debug.Log("characterIndex = " + characterIndex);

            int startIndex = dialogueField.text.Contains("@") ? 1 : 0;

            int lastIndex = dialogueField.text.Contains(":") ? characterIndex - startIndex : selectedModel.name.Length - startIndex;

            //Debug.Log("lastIndex = " + lastIndex);

            string subString = dialogueField.text.Substring(startIndex, lastIndex);

            //Debug.Log("subString = " + subString);

            if (!subString.Equals(selectedModel.name) && !isSearchAPICalled)
            {
                keyword = subString;

                CallSearchAPI();
            }

            if (characterIndex + 2 < dialogueField.text.Length)
            {
                enterButton.interactable = true;

                Debug.Log("Making True");
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

    public void OnEnterButtonAction()
    {
        if (editedDialogueCell != null)
        {
            int index = dialogueCells.IndexOf(editedDialogueCell);

            editedDialogueCell.SetView(dialogueField.text, editedDialogueCell.isLeftAlign, editedDialogueCell.userSearchModel, OnButtonAction);

            Dictionary<string, object> characterBody = dialogues.ElementAt(index);

            characterBody["character_id"] = selectedModel.id;

            characterBody["dailogue"] = dialogueField.text;

            dialogues.RemoveAt(index);

            dialogues.Insert(index, characterBody);

            editedDialogueCell = null;

            ClearFieldData();
        }
        else
        {
            GameObject dialogueObj = Instantiate(dialogueCell, scrollRect.content);

            dialogueObj.GetComponent<DialogueCell>().SetView(dialogueField.text, isLeftAlign, selectedModel, OnButtonAction);

            dialogueCells.Add(dialogueObj.GetComponent<DialogueCell>());

            Dictionary<string, object> characterBody = new Dictionary<string, object>();

            characterBody.Add("character_id", selectedModel.id);

            characterBody.Add("dailogue", dialogueField.text);

            dialogues.Add(characterBody);

            scrollRect.verticalNormalizedPosition = 0.0f;

            ClearFieldData();
        }
    }

    public void OnSaveButtonAction()
    {
        CreateScenesView.Instance.OnSaveDialogues(dialogues);

        OnBackButtonAction();
    }

    public void OnBackButtonAction()
    {
        ResetData();

        gameObject.SetActive(false);
    }

    void ClearFieldData()
    {
        enterButton.interactable = false;

        selectedModel = null;

        isLeftAlign = !isLeftAlign;

        keyword = dialogueField.text = string.Empty;
    }

    void OnButtonAction(DialogueCell editedDialogueCell)
    {
        this.editedDialogueCell = editedDialogueCell;

        int modelIndex = dialogueCells.IndexOf(editedDialogueCell);

        selectedModel = editedDialogueCell.userSearchModel;

        TouchScreenKeyboard.Open(editedDialogueCell.dialogueText.text);

        dialogueField.text = editedDialogueCell.dialogueText.text;
    }

    void ResetData()
    {
        selectedModel = null;

        keyword = dialogueField.text = string.Empty;

        dialogues.Clear();

        dialogueCells.Clear();

        searchContent.DestroyChildrens();

        scrollRect.content.DestroyChildrens();
    }

    public static int GetKeyboardHeight(bool includeInput)
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

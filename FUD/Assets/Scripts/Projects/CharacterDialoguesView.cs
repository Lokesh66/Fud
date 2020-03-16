using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterDialoguesView : MonoBehaviour
{
    public ScrollRect scrollRect;

    public GameObject dialogueCell;

    public Button enterButton;

    public TMP_InputField dialogueField;

    public RectTransform searchContent;

    public RectTransform searchScrollTrans;

    public GameObject searchCell;

    public GameObject scrollObject;


    List<Dictionary<string, object>> dialogues = new List<Dictionary<string, object>>();

    UserSearchModel selectedModel = null;

    bool isSearchAPICalled = false;

    bool isLeftAlign = true;

    string keyword = string.Empty;

    float searchScrollMaxHeight = 400.0f;

    float searchCellHeight = 100.0f;


    public void EnableView()
    {
        gameObject.SetActive(true);
    }

    public void OnValueChange()
    {
        if (selectedModel == null)
        {
            enterButton.interactable = false;

            if (!isSearchAPICalled && selectedModel == null)
            {
                keyword = dialogueField.text;

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
        }
        else
        {
            int characterIndex = dialogueField.text.IndexOf(':');

            if (characterIndex + 1 < dialogueField.text.Length)
            {
                enterButton.interactable = true;
            }
            else
            {
                enterButton.interactable = false;
            }
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

        dialogueField.text = "@" + selectedModel.name + " : ";

        searchContent.DestroyChildrens();

        scrollObject.SetActive(false);
    }

    public void OnEnterButtonAction()
    {
        GameObject dialogueObj = Instantiate(dialogueCell, scrollRect.content);

        dialogueObj.GetComponent<DialogueCell>().SetView(dialogueField.text, isLeftAlign);

        Dictionary<string, object> kvp = new Dictionary<string, object>();

        kvp.Add("character_id", selectedModel.id);

        kvp.Add("dailogue", dialogueField.text);

        dialogues.Add(kvp);

        keyword = dialogueField.text = string.Empty;

        scrollRect.verticalNormalizedPosition = 0.0f;

        selectedModel = null;

        isLeftAlign = !isLeftAlign;
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

    void ResetData()
    {
        selectedModel = null;

        keyword = dialogueField.text = string.Empty;

        searchContent.DestroyChildrens();

        scrollRect.content.DestroyChildrens();
    }

    public int GetKeyboardSize()
    {
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

            using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);

                return Screen.height - Rct.Call<int>("height");
            }
        }
    }
}

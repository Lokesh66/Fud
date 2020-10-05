using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;


public class FilterCell : MonoBehaviour
{
    public enum ESubScrollContentType
    {
        None,
        Genres,
        CraftRoles,
        CraftRoleCategery,
    }

    public RectTransform rectTransform;

    public RectTransform subCellParent;

    public GameObject subScrollObject;

    public GameObject cellObject;

    public GameObject addObject;

    public GameObject disableObject;

    public VerticalLayoutGroup layoutGroup;

    public ESubScrollContentType contentType;

    public float contentHeight;


    public List<string> filterKeys = new List<string>();

    public List<int> filterValues = new List<int>();


    FilterSubCell currentCell;

    Action<object> OnRoleSelectAction;

    Action<int> OnCastCategeryAction;

    object selectedModel;


    void Start()
    {
        if (contentType == ESubScrollContentType.Genres)
        {
            SetGenresView();
        }
        else if (contentType == ESubScrollContentType.CraftRoles)
        {
            SetCraftRolesView();
        }
        else {
            if (subCellParent.childCount == 0)
            {
                SetSubScrollView();
            }
        }
    }

    public void Load(List<DropdownModel> modelsList)
    {
        filterKeys.Clear();

        filterValues.Clear();

        for (int i = 0; i < modelsList.Count; i++)
        {
            filterKeys.Add(modelsList[i].text);

            filterValues.Add(modelsList[i].id);
        }

        SetSubScrollView();
    }

    public void OnRoleSelection(Action<object> OnRoleSelection)
    {
        this.OnRoleSelectAction = OnRoleSelection;
    }

    public void OnCastCategeryChange(Action<int> OnCastCategeryChange)
    {
        this.OnCastCategeryAction = OnCastCategeryChange;
    }

    void SetGenresView()
    {
        List<Genre> genres = DataManager.Instance.genres;

        GameObject clownedObject = null;

        for (int i = 0; i < genres.Count; i++)
        {
            clownedObject = Instantiate(cellObject, subCellParent);

            clownedObject.GetComponent<FilterSubCell>().Load(genres[i], genres[i].name, OnTapAction);
        }
    }

    void SetCraftRolesView()
    {
        List<Craft> crafts = DataManager.Instance.crafts;

        GameObject clownedObject = null;

        for (int i = 0; i < crafts.Count; i++)
        {
            clownedObject = Instantiate(cellObject, subCellParent);

            clownedObject.GetComponent<FilterSubCell>().Load(crafts[i], crafts[i].name, OnTapAction);
        }
    }

    void SetSubScrollView()
    {
        GameObject clownedObject = null;

        subCellParent.DestroyChildrens();

        for (int i = 0; i < filterKeys.Count; i++)
        {
            KeyValuePair<string, int> item = new KeyValuePair<string, int>(filterKeys[i], filterValues[i]);

            clownedObject = Instantiate(cellObject, subCellParent);

            clownedObject.GetComponent<FilterSubCell>().Load(item, filterKeys[i], OnTapAction);
        }
    }

    public void OnButtonAction()
    {
        StartCoroutine(UpdateContentHeight());

        subScrollObject.SetActive(true);

        addObject.SetActive(false);

        disableObject.SetActive(true);
    }

    public void OnCloseButtonAction()
    {
        subScrollObject.SetActive(false);

        addObject.SetActive(true);

        disableObject.SetActive(false);

        StartCoroutine(DisableContentHeight());
    }

    IEnumerator DisableContentHeight()
    {
        layoutGroup.enabled = false;

        Vector2 contentSize = rectTransform.sizeDelta;

        contentSize.y -= contentHeight;

        rectTransform.sizeDelta = contentSize;

        yield return new WaitForEndOfFrame();

        layoutGroup.enabled = true;        
    }

    IEnumerator UpdateContentHeight()
    {
        layoutGroup.enabled = false;

        Vector2 contentSize = rectTransform.sizeDelta;

        contentSize.y += contentHeight;

        rectTransform.sizeDelta = contentSize;

        yield return new WaitForEndOfFrame();

        layoutGroup.enabled = true;
    }

    void OnTapAction(object selectedModel, FilterSubCell currentSubCell)
    {
        if (currentCell == currentSubCell)
            return;

        currentCell?.DeSelect();

        this.currentCell = currentSubCell;

        this.selectedModel = selectedModel;

        if (contentType == ESubScrollContentType.CraftRoles)
        {
            OnRoleSelectAction?.Invoke(selectedModel);
        }
        else if (contentType == ESubScrollContentType.CraftRoleCategery)
        {
            OnCastCategeryAction?.Invoke(((KeyValuePair<string, int>)selectedModel).Value);
        }
    }

    public int GetStatus()
    {
        int statusValue = -1;

        if (selectedModel == null)
            return statusValue;

        switch (contentType)
        {
            case ESubScrollContentType.Genres:
                statusValue = (selectedModel as Genre).id;
                break;

            case ESubScrollContentType.CraftRoles:
                statusValue = (selectedModel as Craft).id;
                break;

            case ESubScrollContentType.None:
                statusValue = ((KeyValuePair<string, int>)selectedModel).Value;
                break;
        }

        return statusValue;
    }

    public void ClearSelectedModels()
    {
        currentCell?.DeSelect();

        selectedModel = null;

        currentCell = null;
    }
}

public class DropdownModel
{
    public string text;
    public int id;
}

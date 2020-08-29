using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class FilterCell : MonoBehaviour
{
    public enum ESubScrollContentType
    {
        None,
        Genres,
        CraftRoles,
    }

    public enum ECustomFilterType
    {
        AuditionProjects,
        Producers,
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


    Dictionary<string, int> statusDict;


    FilterSubCell currentCell;

    object selectedModel;

    private bool isOpened = false;


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
            SetSubScrollView();
        }
    }

    public void Load(List<DropdownModel> modelsList)
    {
        for (int i = 0; i < modelsList.Count; i++)
        {
            filterKeys.Add(modelsList[i].text);

            filterValues.Add(modelsList[i].id);
        }

        SetSubScrollView();
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
        currentCell?.DeSelect();

        this.currentCell = currentSubCell;

        this.selectedModel = selectedModel;
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

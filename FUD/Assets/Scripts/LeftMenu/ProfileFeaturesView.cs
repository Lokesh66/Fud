using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileFeaturesView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    public GameObject noDataObject;


    List<FeaturedModel> modelsList;

    Action<bool> OnCloseAction;


    public void Load(Action<bool> OnClose)
    {
        modelsList = DataManager.Instance.featuredModels;

        OnCloseAction = OnClose;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        content.DestroyChildrens();

        for (int i = 0; i < modelsList.Count; i++)
        {
            if (modelsList[i].available_count > 0)
            {
                GameObject subCellObject = Instantiate(cellCache, content);

                subCellObject.GetComponent<PlanSubCell>().Load(modelsList[i], true);
            }
        }

        UpdateNoDataView();
    }

    void UpdateNoDataView()
    {
        List<FeaturedModel> models = modelsList.FindAll(item => item.available_count.Equals(0));

        noDataObject.SetActive(models.Count == modelsList.Count);
    }

    public void NoDataButtonAction()
    {
        OnBackButtonAction(true);

        CanvasManager.Instance.leftMenu.OnSubscrptionButtonAction();
    }

    public void OnBackButtonAction(bool isGoingToStore)
    {
        OnCloseAction?.Invoke(isGoingToStore);

        gameObject.SetActive(false);
    }
}

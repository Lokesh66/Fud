using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class SubscriptionPlanCell : MonoBehaviour
{
    public RectTransform content;

    public TextMeshProUGUI planNameText;

    public TextMeshProUGUI priceText;

    public GameObject planSubCell;

    public Image topBG;


    public Sprite inActiveTopBG;

    public Sprite activeTopBG;


    SubscriptionModel subscriptionModel;
    Action<SubscriptionModel> OnClick;

    public void Load(SubscriptionModel subscriptionModel, Action<SubscriptionModel> action)
    {
        this.subscriptionModel = subscriptionModel;
        OnClick = action;
        SetView();
    }

    void SetView()
    {
        planNameText.text = subscriptionModel.name;

        priceText.text = "Rs." + subscriptionModel.price;

        List<Feature> featuresList = subscriptionModel.features;

        content.DestroyChildrens();

        for (int i = 0; i < featuresList.Count; i++)
        {
            GameObject subCellObject = Instantiate(planSubCell, content);

            subCellObject.GetComponent<PlanSubCell>().Load(featuresList[i]);
        }
    }

    public void OnClickAction()
    {
        OnClick?.Invoke(subscriptionModel);
    }
}

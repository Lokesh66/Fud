using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class SubscriptionPlanCell : MonoBehaviour
{
    public RectTransform content;

    public Canvas scrollCanvas;

    public TextMeshProUGUI planNameText;

    public TextMeshProUGUI priceText;

    public GameObject planSubCell;

    public Image topBG;


    public Sprite inActiveTopBG;

    public Sprite activeTopBG;


    SubscriptionModel subscriptionModel;
    Action<SubscriptionModel> OnClick;

    bool isPlanActive = false;


    public void Load(SubscriptionModel subscriptionModel, Action<SubscriptionModel> action)
    {
        this.subscriptionModel = subscriptionModel;

        isPlanActive = subscriptionModel.IsPlanActive();

        OnClick = action;

        SetView();
    }

    void SetView()
    {
        planNameText.text = subscriptionModel.name;


        priceText.text = isPlanActive ? "Active" : "Rs." + subscriptionModel.price;

        topBG.sprite = isPlanActive ? activeTopBG : inActiveTopBG;

        content.DestroyChildrens();

        if (isPlanActive) {

            List<FeaturedModel> featuredModels = DataManager.Instance.featuredModels;

            for (int i = 0; i < featuredModels.Count; i++)
            {
                GameObject subCellObject = Instantiate(planSubCell, content);

                subCellObject.GetComponent<PlanSubCell>().Load(featuredModels[i], isPlanActive);
            }
        }
        else {
            List<Feature> featuredModels = subscriptionModel.features;

            for (int i = 0; i < featuredModels.Count; i++)
            {
                GameObject subCellObject = Instantiate(planSubCell, content);

                subCellObject.GetComponent<PlanSubCell>().Load(featuredModels[i], isPlanActive);
            }
        }
    }

    public void OnClickAction()
    {
        OnClick?.Invoke(subscriptionModel);
    }
}

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

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

    public void Load(SubscriptionModel subscriptionModel)
    {
        this.subscriptionModel = subscriptionModel;

        SetView();
    }

    void SetView()
    {
        planNameText.text = subscriptionModel.name;

        priceText.text = "Rs." + subscriptionModel.price_per_month;

        List<Feature> featuresList = subscriptionModel.features;

        content.DestroyChildrens();

        for (int i = 0; i < featuresList.Count; i++)
        {
            GameObject subCellObject = Instantiate(planSubCell, content);

            subCellObject.GetComponent<PlanSubCell>().Load(featuresList[i]);
        }
    }
}

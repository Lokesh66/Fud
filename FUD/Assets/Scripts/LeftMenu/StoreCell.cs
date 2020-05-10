using UnityEngine;
using System;
using TMPro;


public class StoreCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI priceText;

    public TextMeshProUGUI countText;

    public TextMeshProUGUI durationText;


    SubscriptionModel subscriptionModel;

    Action<SubscriptionModel> OnTapAction;


    public void Load(SubscriptionModel subscriptionModel, Action<SubscriptionModel> OnTapAction)
    {
        this.subscriptionModel = subscriptionModel;

        this.OnTapAction = OnTapAction;

        SetView(subscriptionModel);
    }

    void SetView(SubscriptionModel subscriptionModel)
    {
        titleText.text = subscriptionModel.name;

        priceText.text = "Rs. " + subscriptionModel.price.ToString();

        if (subscriptionModel.features.Count > 0)
        {
            countText.text = subscriptionModel.features[0].count.ToString() + " " + subscriptionModel.name;

            durationText.text =  "1 "+ subscriptionModel.durationTitle;
        }
    }

    public void OnButtonAction()
    {
        OnTapAction?.Invoke(subscriptionModel);
    }
}

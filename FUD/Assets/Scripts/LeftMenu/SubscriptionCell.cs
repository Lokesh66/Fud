using UnityEngine;
using TMPro;
using System;

public class SubscriptionCell : MonoBehaviour
{
    public TextMeshProUGUI planText;

    public TextMeshProUGUI priceText;

    public TextMeshProUGUI descriptionText;

    SubscriptionModel subscriptionModel;

    Action<SubscriptionModel> OnClick;

    public void SetView(SubscriptionModel model, Action<SubscriptionModel> action)
    {
        subscriptionModel = model;
        priceText.text = model.price_per_month.ToString();
        planText.text = model.name;
        descriptionText.text = model.description;

        OnClick = action;
    }

    public void OnClickAction()
    {
        OnClick?.Invoke(subscriptionModel);
    }
}

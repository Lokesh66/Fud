using System.Collections.Generic;
using System;

public partial class APIHandler
{
    public void GetSubscriptionPlans(int tabIndex, string roleId, string planDuration, Action<bool, string> OnResponse)
    {
        string url = APIConstants.GET_SUBSCRIPTION_PLANS;

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(roleId))
        {
            url += "?role_id=" + roleId + "&type=" + tabIndex;
            
            parameters.Add("Plan_duration", planDuration);
        }


        gameManager.StartCoroutine(GetRequest(url, true, (status, response) => {

            OnResponse(status, response);
        }));
    }

    public void VerifyPurchsedOrderId(string orderId, Action<bool> OnResponse)
    {
        string url = APIConstants.VERIFY_ORDER_ID;

        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("order_id", orderId);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            OnResponse(status);

        }));
    }
}

[Serializable]
public class Feature : FeaturedModel
{
    public int count;
}

[Serializable]
public class SubscriptionModel
{
    public int id;
    public string name;
    public string description;
    public int price_per_month;
    public int price_per_year;
    public int price_per_forever;
    public int plan_purchase;
    public List<Feature> features;

    public int price;

    public string durationTitle;

    public bool IsPlanActive()
    {
        //plan_purchase = 1;
        
        return plan_purchase > 0;
    }

    public void SetPlanPrice(string duration)
    {
        switch (duration)
        {
            case "Monthly":
                price = price_per_month;
                durationTitle = "Month";
                break;

            case "Yearly":
                durationTitle = "Year";
                price = price_per_year;
                break;

            case "Forever":
                durationTitle = "Forever";
                price = price_per_forever;
                break;
        }
    }
}


[Serializable]
public class SubscriptionResponse : BaseResponse
{
    public List<SubscriptionModel> data;
}

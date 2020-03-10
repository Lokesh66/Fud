using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void GetSubscriptionPlans(int roleId, Action<bool, string> OnResponse)
    {
        string url = APIConstants.GET_SUBSCRIPTION_PLANS + "role_id=" + roleId;

        GetRequest(url, true, (status, response) => {

            OnResponse(status, response);
        });
    }
}

[Serializable]
public class Feature
{
    public int id;
    public string name;
    public string description;
    public int status;
    public object created_date_time;
    public object updatedAt;
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
    public List<Feature> features;
}


[Serializable]
public class SubscriptionResponse : BaseResponse
{
    public List<SubscriptionModel> data;
}

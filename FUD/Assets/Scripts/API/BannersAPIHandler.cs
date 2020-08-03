using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void GetBanners(int type, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("type", type);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_HOME_BANNERS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetHomeAlerts(int type, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("type", type);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_HOME_BANNERS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }
}

[Serializable]
public class HomeBannerModel : HomeAlertModel
{
    public string sub_title;
    public string image_url;
}

[Serializable]
public class HomeBannersResponse : BaseResponse
{
    public List<HomeBannerModel> data;
}


[Serializable]
public class HomeAlertsResponse : BaseResponse
{
    public List<HomeAlertModel> data;
}

[Serializable]
public class HomeAlertModel
{
    public int id;
    public string title;
    public string comments;
}

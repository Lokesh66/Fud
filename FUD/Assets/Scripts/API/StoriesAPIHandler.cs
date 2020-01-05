using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;

public partial class APIHandler
{
    public void GetAllStories(string phoneNumber, Action<bool> action)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        parameters.Add("phone", phoneNumber);

        PostRequest(APIConstants.CREATE_STORY, parameters, (bool status, string response) => {

            action?.Invoke(status);

        });
    }
}

[Serializable]
public class Story
{
    public int id;
    public int story_id;
    public int user_id;
    public string title;
    public int cost_estimation;
    public int estimated_time;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class StoriesResponse
{
    public string message;
    public List<Story> data;
    public int status;
}

[Serializable]
public class ProMultiMedia
{
    public int content_id;
    public string content_url;
    public string media_type;
}

public class attrbutes
{
public string title;
public string sub_title;
public string description;
public int genre_id;
public int price;
public int type_id;
public string image_url;
public List<ProMultiMedia> pro_multi_media;
public string producername;
public int ratings;
public string release_date;
public string capabilities;
public string redeem_code;
public string developers;
public string release_note;
public string report;
public string privacy_policy;
public string terms_and_conditions;
public string Information;
public string url;
public string producers;
}

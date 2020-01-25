using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using System;
using TMPro;


public partial class APIHandler
{
    public void GetAllStories( Action<bool, List<Story>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

/*        parameters.Add("phone", phoneNumber);*/
        gameManager.StartCoroutine(GetRequest(APIConstants.CREATE_STORY, true, (bool status, string response) => {
            if (status)
            {
                StoriesResponse stories = JsonUtility.FromJson<StoriesResponse>(response);
                action?.Invoke(true, stories.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void CreateStory(string title, string subTitle, string description,int genreId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("story_line", subTitle);

        parameters.Add("description", description);

        parameters.Add("genre_id", genreId);

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_STORY, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UploadFile(string filePath, TextMeshProUGUI statusText, TextMeshProUGUI contentTypeText, Action<bool> OnResposne)
    {
        gameManager.StartCoroutine(Upload(filePath, statusText, contentTypeText, ((status) => {

            OnResposne?.Invoke(status);
        })));
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

using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void GetAllAuditions(Action<bool, List<Audition>> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.CREATE_AUDITION, true, (bool status, string response) => {
            if (status)
            {
                AuditionsResponse auditions = JsonUtility.FromJson<AuditionsResponse>(response);
                action?.Invoke(true, auditions.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void CreateAudition(Dictionary<string, object> parameters, Action<bool, string> action)
    {    
        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_AUDITION, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void ModifyAudition(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_AUDITION, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void FetchAuditions(AuditionType type, Action<bool, string> action)
    {
        Debug.LogError("FetchAuditions Called");

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        if (type == AuditionType.Live)
            parameters.Add("fetch_live", 1);
        else if (type == AuditionType.Joined)
            parameters.Add("fetch_joined", 1);
        else if (type == AuditionType.Created)
            parameters.Add("fetch_created", 1);

        gameManager.StartCoroutine(PostRequest(APIConstants.SEARCH_USER_AUDITION, true, parameters, action));
    }

    public void GetHomeAuditions(Action<bool, string> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.CREATE_AUDITION, true, action));
    }

    public void JoinAudition(Dictionary<string, object> parameters, Action<bool, string> action)
    { 
        gameManager.StartCoroutine(PostRequest(APIConstants.USER_AUDITION, true, parameters, (bool status, string response) => {

            if (status)
            {
                DataManager.Instance.UpdateFeaturedData(EFeatureType.AuditionJoining);

                action?.Invoke(true, response);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void UpdateJoinedAudition(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PutRequest(APIConstants.USER_AUDITION, true, parameters, action));
    }


    public void SearchAuditions(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PostRequest(APIConstants.ALL_ACTIVE_AUDITIONS, true, parameters, action));
    }
    public void AcceptOrRejectAudition(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        //"audition_id": 13,
        //"user_audition_id": 13,
        //"status": “selected“ or “rejected”
        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_AUDITION_STATUS, true, parameters, action));
    }
}

[Serializable]
public class Audition
{
    public int id;
    public object project_cast_id;
    public string topic;
    public int project_id;
    public int user_id;
    public int rate_of_pay;
    public string status;
    public string title;
    public string image_url;
    public string description;
    public int age_from;
    public int age_to;
    public int no_of_persons_req;
    public int no_of_persons_joined;
    public string type;
    public int end_date;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class AuditionsResponse
{
    public string message;
    public List<Audition> data;
    public int status;
}

[Serializable]
public class JoinedAudition
{
    public int id;
    public int audition_id;
    public int user_id;
    public string status;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public Audition Audition;

    public EAuditionStatus GetAuditonStatus()
    {
        EAuditionStatus auditionStatus = EAuditionStatus.Review;

        switch (status.ToLower())
        {
            case "joined":
                auditionStatus = EAuditionStatus.Review;
                break;
            case "selected":
                auditionStatus = EAuditionStatus.ShortListed;
                break;
            case "rejected":
                auditionStatus = EAuditionStatus.Rejected;
                break;
        }

        return auditionStatus;
    }
}

[Serializable]
public class JoinedAuditionsResponse
{
    public string message;
    public List<JoinedAudition> data;
    public int status;
}

[Serializable]
public class UserAuditionMultimedia
{
    public int id;
    public int source_id;
    public string source_type;
    public string media_type;
    public string content_url;
    public string status;
    public int content_id;
    public object related_content_id;
    public DateTime created_date_time;
    public DateTime updated_date_time;


    public EMediaType GetMediaType(string _mediaType)
    {
        EMediaType mediaType = EMediaType.Image;

        switch (_mediaType)
        {
            case "image":
                mediaType = EMediaType.Image;
                break;
            case "audio":
                mediaType = EMediaType.Audio;
                break;
            case "video":
                mediaType = EMediaType.Video;
                break;
        }

        return mediaType;
    }
}

[Serializable]
public class SearchAudition
{
    public int id;
    public int audition_id;
    public int user_id;
    public string status;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public List<UserAuditionMultimedia> UserAuditionMultimedia;
}

[Serializable]
public class SearchAuditionResponse
{
    public string message;
    public List<SearchAudition> data;
    public int status;
}

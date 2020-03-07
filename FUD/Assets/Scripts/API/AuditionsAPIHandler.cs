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

    public void DeleteAudition(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_AUDITION, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void SearchAuditions(bool isJoined, Action<bool, List<Audition>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        if (isJoined)
        {
            parameters.Add("fetch_joined", 1);            
        }
        else
        {
            parameters.Add("fetch_live", 1);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.SEARCH_USER_AUDITION, true, parameters, (bool status, string response) => {
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

    public void JoinAudition(int auditionId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("audition_id", auditionId);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_AUDITION, true, parameters, (bool status, string response) => {
            if (status)
            {
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
        /*"id":1,
    "user_id":"user_id",
    "audition_id": "audition_id",
    "status": "yes"*/

        parameters.Add("id", 1);
        parameters.Add("user_id", "user_id");
        parameters.Add("audition_id", "audition_id");
        parameters.Add("status", "yes");

        gameManager.StartCoroutine(PutRequest(APIConstants.USER_AUDITION, true, parameters, (bool status, string response) => {
            if (status)
            {
                action?.Invoke(true, response);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
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
    public DateTime end_date;
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

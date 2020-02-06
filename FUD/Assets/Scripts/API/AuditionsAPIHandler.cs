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
        /*"topic":"act as pawan",
       "rate_of_pay": 100000,
       "end_date": "2020-03-23",
       "title": "For all the young  fresh talent",
       "description" : "are you young, talented and looking for a platform to showcase your amazing talents?",
       "age_from": 21,
       "age_to" : 25,
       "type": "group"*/        

        gameManager.StartCoroutine(PostRequest(APIConstants.CREATE_AUDITION, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateCreatedAudition(Dictionary<string, object> parameters, Action<bool, string> action)
    {
        /*"id": 1,
        "user_id"::"user_id",
        "project_cast_id": "project_cast_id",
        "topic":"topic",
        "end_date":"end_date",
        "rate_of_pay":"rate_of_pay",
        "status":"yes"*/

        parameters.Add("id", 1);
        parameters.Add("user_id", 1);
        parameters.Add("project_cast_id", 1);
        parameters.Add("topic", "Act as Sunil");
        parameters.Add("end_date", "2020-03-23");
        parameters.Add("rate_of_pay", 10000);
        parameters.Add("status", "yes");

        gameManager.StartCoroutine(PutRequest(APIConstants.CREATE_AUDITION, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void SearchAuditions(bool isJoined, Action<bool, List<Audition>> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("fetch_live", isJoined ? 0 : 1);

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
    public int user_id;
    public int rate_of_pay;
    public string status;
    public string title;
    public string description;
    public int age_from;
    public int age_to;
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

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

    public void GetOfferedAuditions(int pageNo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_OFFERED_AUDITIONS;

        url += "?page=" + pageNo +  "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("fetch_live", 1);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, action));
    }

    public void GetAlteredAuditions(int pageNo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_ALTERED_AUDITIONS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("fetch_joined", 1);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, action));
    }

    public void ApplyAudtionOfferedFilter(int sortId, int orderId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_OFFERED_AUDITIONS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("sortBy", sortId);

        parameters.Add("sortOrder", orderId);


        gameManager.StartCoroutine(PostRequest(url, true, parameters, action));
    }

    public void ApplyAudtionAlteredFilter(int sortId, int statusId, int orderId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_ALTERED_AUDITIONS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("sortBy", sortId);

        parameters.Add("status", statusId);

        parameters.Add("sortOrder", orderId);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, action));
    }

    public void ApplyAudtionCreateFilter(int sortId, int projectId, int orderId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_CREATED_AUDITIONS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("sortBy", sortId);

        parameters.Add("project_id", projectId);

        parameters.Add("sortOrder", orderId);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, action));
    }

    public void GetCreatedAuditions(int pageNo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_CREATED_AUDITIONS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("fetch_created", 1);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, action));
    }

    public void GetProjectAuditions(Action<bool, List<AuditionProjectModel>> OnResponse)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_CREATED_AUDITIONS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            CreatedAuditionProjects responseModel = JsonUtility.FromJson<CreatedAuditionProjects>(response);

            if (status)
            {
                OnResponse?.Invoke(true, responseModel.data);
            }
        }));
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

    public void SearchAuditions(int pageNo, Dictionary<string, object> parameters, Action<bool, string> action)
    {
        string url = APIConstants.ALL_ACTIVE_AUDITIONS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(PostRequest(url, true, parameters, action));
    }

    public void AcceptOrRejectAudition(int auditionId, int userAuditionId, int status, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("audition_id", auditionId);

        parameters.Add("user_audition_id", userAuditionId);

        parameters.Add("status", status);

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
    public int type;
    public int end_date;
    public DateTime created_date_time;
    public DateTime updatedAt;
}

[Serializable]
public class AuditionsResponse : BaseResponse
{
    public List<Audition> data;
}

[Serializable]
public class JoinedAudition
{
    public int id;
    public int audition_id;
    public int user_id;
    public int creater_id;
    public int sender_status;
    public int reciever_status;
    public int status;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public Audition Audition;

    public EAuditionStatus GetAuditonStatus()
    {
        EAuditionStatus auditionStatus = EAuditionStatus.Review;

        bool isOwnAudition = DataManager.Instance.userInfo.id == user_id;

        int requiredStatus = isOwnAudition ? sender_status : reciever_status;

        switch (requiredStatus)
        {
            case 0:
            case 2:
                auditionStatus = EAuditionStatus.Review;
                break;
            case 3:
            case 5:
                auditionStatus = EAuditionStatus.ShortListed;
                break;
            case 8:
                auditionStatus = EAuditionStatus.Rejected;
                break;
        }

        return auditionStatus;
    }
}

[Serializable]
public class JoinedAuditionsResponse : BaseResponse
{
    public List<JoinedAudition> data;
}

[Serializable]
public class UserAuditionMultimedia : MultimediaModel
{   
    
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
    public List<MultimediaModel> UserAuditionMultimedia;

    public MultimediaModel onScreenModel = null;
}

[Serializable]
public class SearchAuditionResponse : BaseResponse
{
    public List<SearchAudition> data;
}

[Serializable]
public class CreatedAuditionProjects : BaseResponse
{
    public List<AuditionProjectModel> data;
}

[Serializable]
public class AuditionProjectModel
{
    public int id;
    public string title;
}

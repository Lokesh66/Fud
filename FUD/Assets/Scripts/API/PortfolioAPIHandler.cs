using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public partial class APIHandler
{
    public void CreatePortfolio(string title, string description, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);

        parameters.Add("title", title);

        parameters.Add("description", description);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("port_multi_media", multimediaModels);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_PORTFOLIO, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdatePortfolio(string title, string description,  int id, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        /*List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);*/

        parameters.Add("title", title);

        parameters.Add("id", id);

        parameters.Add("description", description);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("port_multi_media", multimediaModels);
        }

        gameManager.StartCoroutine(PutRequest(APIConstants.USER_PORTFOLIO, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemovePortfolio(int id, int status, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        //parameters.Add("port_album_id", albumId);

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.USER_PORTFOLIO, true, parameters, (apiStatus, response) => {

            action(apiStatus);
        }));
    }

    public void CreateWorkExperiance(CreateExperianceModel experianceModel, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);

        parameters.Add("description", experianceModel.description);

        parameters.Add("start_date", experianceModel.startDate);

        parameters.Add("end_date", experianceModel.endDate);

        parameters.Add("industry_id", experianceModel.industryId);

        parameters.Add("role_id", experianceModel.roleId);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("work_exp_media", multimediaModels);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.UPDATE_EXPERIANCE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateWorkExperiance(CreateExperianceModel experianceModel, int id, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);

        parameters.Add("id", id);

        parameters.Add("description", experianceModel.description);

        parameters.Add("start_date", experianceModel.startDate);

        parameters.Add("end_date", experianceModel.endDate);

        parameters.Add("industry_id", experianceModel.industryId);

        parameters.Add("role_id", experianceModel.roleId);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("work_exp_media", multimediaModels);
        }

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_EXPERIANCE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemovePortfolioExperiance(int id, int status, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        //parameters.Add("port_album_id", albumId);

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_EXPERIANCE, true, parameters, (apiStatus, response) => {

            action(apiStatus);
        }));
    }

    public void GetAllAlbums(Action<bool, List<PortfolioModel>> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.USER_PORTFOLIO, true, (bool status, string response) => {

            if (status)
            {
                PortfolioResponseModel responseModel = JsonUtility.FromJson<PortfolioResponseModel>(response);

                if (responseModel.data.Count > 0)
                {
                    action?.Invoke(true, responseModel.data);
                }
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void GetIndustries(Action<bool, List<IndustryModel>> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.GET_INDUSTRIES, true, (bool status, string response) => {

            if (status)
            {
                IndustriesResponse responseModel = JsonUtility.FromJson<IndustriesResponse>(response);
                action?.Invoke(true, responseModel.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void GetAllExperiances(Action<bool, List<WorkExperianceModel>> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.UPDATE_EXPERIANCE, true, (bool status, string response) => {

            if (status)
            {
                WorkExperianceResponseModel responseModel = JsonUtility.FromJson<WorkExperianceResponseModel>(response);

                action?.Invoke(true, responseModel.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void PostPortfolio(int id, string comment, int postedTo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("portfolio_id", id);

        parameters.Add("shared_to", postedTo);

        parameters.Add("comments", comment);

        parameters.Add("access_modifier", 0);

        gameManager.StartCoroutine(PostRequest(APIConstants.PORTFOLIO_SHARE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetPortfolioPosts(Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("status", 0);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_PORTFOLIO_POSTS, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdatePortfolioPostStatus(int postId, string postStatus, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", postId);

        parameters.Add("status", postStatus);

        gameManager.StartCoroutine(PutRequest(APIConstants.PORTFOLIO_SHARE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateProfileInfo(ProfileInfoModel infoModel, Action<bool, UserData> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);

        //parameters.Add("phone", infoModel.number);

        //parameters.Add("role_id", infoModel.actor);

        UserData user = DataManager.Instance.userInfo;

        parameters.Add("agree_terms_condition", 1);
        if(!infoModel.mail.Equals(user.email_id))
            parameters.Add("email_id", infoModel.mail);

        if (!infoModel.name.Equals(user.name))
            parameters.Add("name", infoModel.name);
        if (infoModel.dob != user.dob)
            parameters.Add("dob", infoModel.dob);
        if (!infoModel.memberId.Equals(user.maa_membership_id))
            parameters.Add("maa_membership_id", infoModel.memberId);
        if (!infoModel.currentLocation.Equals(user.current_location))
            parameters.Add("current_location", infoModel.currentLocation);
        if (!infoModel.nativeLocation.Equals(user.native_location))
            parameters.Add("native_location", infoModel.nativeLocation);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_USER_PROFILE, true, parameters, (bool status, string response) =>
        {
            Debug.Log("UpdateProfileInfo : "+response);
            if (status)
            {
                string userFilePath = APIConstants.PERSISTENT_PATH + "UserInfo";

                string fileName = Path.GetDirectoryName(userFilePath);

                if (!Directory.Exists(userFilePath))
                {
                    Directory.CreateDirectory(userFilePath);
                }

                userFilePath += "/Userinfo";

                File.WriteAllText(userFilePath, response);

                UserDataObject responseModel = JsonUtility.FromJson<UserDataObject>(response);

                action?.Invoke(true, responseModel.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }
}

[Serializable]
public class PortMultiMediaModel
{
    public int content_id;
    public string content_url;
    public string media_type;
}

[Serializable]
public class PortMultimediaModels
{
    public List<PortMultiMediaModel> port_multi_media = new List<PortMultiMediaModel>();
}


[Serializable]
public class PortfolioAlbumModel
{
    public int id;
    public int port_album_id;
    public string source_type;
    public string media_type;
    public string content_url;
    public string status;
    public int content_id;
    public object related_content_id;
    public DateTime created_date_time;
    public DateTime updated_date_time;
}

[Serializable]
public class PortfolioModel
{
    public int id;
    public int user_id;
    public string title;
    public int status;
    public string description;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public List<PortfolioAlbumModel> PortfolioMedia;
}


[Serializable]
public class PortfolioResponseModel : BaseResponse
{
    public List<PortfolioModel> data;
}

[Serializable]
public class WorkExpMedia : PortfolioAlbumModel
{
    public int work_exp_id;    
}


[Serializable]
public class WorkExperianceModel
{
    public int id;
    public int role_id;
    public int user_id;
    public string description;
    public int status;
    public int industry_id;
    public DateTime date_exp;
    public string start_date;
    public string end_date;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public List<WorkExpMedia> WorkExpMedia;
}


[Serializable]
public class WorkExperianceResponseModel : BaseResponse
{
    public List<WorkExperianceModel> data;
}

[Serializable]
public class CreateExperianceModel
{
    public string description;
    public int industryId;
    public int roleId;
    public string startDate;
    public string endDate;
    public List<Dictionary<string, object>> multimediaModels;
}

[Serializable]
public class IndustryModel
{
    public int id;
    public string name;
    public string type;
    public DateTime created_date_time;
    public DateTime update_date_time;
    public int status;
}

[Serializable]
public class IndustriesResponse : BaseResponse
{
    public List<IndustryModel> data;
}

[Serializable]
public class ProfileInfoModel
{
    public string name;
    public string dob;
    public string mail;
    public string memberId;
    public string currentLocation;
    public string nativeLocation;
}

[Serializable]
public class PortfolioMediaModel
{
    public int id;
    public int user_id;
    public string title;
    public int status;
    public object likes;
    public string description;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public List<object> PortfolioMedia;
}

[Serializable]
public class PortfolioActivityModel
{
    public int id;
    public int user_id;
    public int shared_to;
    public string status;
    public string reciever_status;
    public int portfolio_id;
    public int access_modifier;
    public string comments;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public PortfolioMediaModel Portfolio;
}

[Serializable]
public class PortfolioPostResponse : BaseResponse
{
    public List<PortfolioActivityModel> data;
}



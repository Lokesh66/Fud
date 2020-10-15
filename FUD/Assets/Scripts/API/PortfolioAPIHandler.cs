using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public partial class APIHandler
{
    public void CreatePortfolio(string title, string description, int accessValue, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("port_multi_media", multimediaModels);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_PORTFOLIO, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdatePortfolio(string title, string description, int id, int accessValue, List<Dictionary<string, object>> multimediaModels, List<int> deletedMedia, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("id", id);

        parameters.Add("description", description);

        parameters.Add("access_modifier", accessValue);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("port_multi_media", multimediaModels);
        }

        if (deletedMedia.Count > 0)
        {
            parameters.Add("remove_media", deletedMedia);
        }

        gameManager.StartCoroutine(PutRequest(APIConstants.USER_PORTFOLIO, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemovePortfolio(int id, int status, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.USER_PORTFOLIO, true, parameters, (apiStatus, response) => {

            action(apiStatus);
        }));
    }

    public void GetPortfolioProfileInfo(int portfolioId, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("portfolio_id", portfolioId);

        gameManager.StartCoroutine(PostRequest(APIConstants.GET_PORTFOLIO_INFO, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void CreateWorkExperiance(CreateExperianceModel experianceModel, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("description", experianceModel.description);

        parameters.Add("start_date", experianceModel.startDate);

        parameters.Add("end_date", experianceModel.endDate);

        parameters.Add("industry_id", experianceModel.industryId);

        parameters.Add("role_id", experianceModel.roleId);

        parameters.Add("role_category_id", experianceModel.roleCategoryId);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("work_exp_media", multimediaModels);
        }

        gameManager.StartCoroutine(PostRequest(APIConstants.UPDATE_EXPERIANCE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateWorkExperiance(CreateExperianceModel experianceModel, int id, List<int> deletedMedia, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        parameters.Add("description", experianceModel.description);

        parameters.Add("start_date", experianceModel.startDate);

        parameters.Add("end_date", experianceModel.endDate);

        parameters.Add("industry_id", experianceModel.industryId);

        parameters.Add("role_id", experianceModel.roleId);

        parameters.Add("role_category_id", experianceModel.roleCategoryId);

        if (multimediaModels.Count > 0)
        {
            parameters.Add("work_exp_media", multimediaModels);
        }

        if (deletedMedia.Count > 0)
        {
            parameters.Add("remove_media", deletedMedia);
        }

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_EXPERIANCE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void RemovePortfolioExperiance(int id, int status, Action<bool> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", id);

        parameters.Add("status", status);

        gameManager.StartCoroutine(PutRequest(APIConstants.UPDATE_EXPERIANCE, true, parameters, (apiStatus, response) => {

            action(apiStatus);
        }));
    }

    public void GetAllAlbums(int pageNo, Action<bool, List<PortfolioModel>> action)
    {
        string url = APIConstants.USER_PORTFOLIO;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        gameManager.StartCoroutine(GetRequest(url, true, (bool status, string response) => {

            if (status)
            {
                PortfolioResponseModel responseModel = JsonUtility.FromJson<PortfolioResponseModel>(response);

                action?.Invoke(true, responseModel.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }));
    }

    public void GetIndustries(Action<bool, List<IndustryModel>> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.GET_INDUSTRIES, true, (bool status, string response) =>
        {
            if (status)
            {
                IndustriesResponse responseModel = JsonUtility.FromJson<IndustriesResponse>(response);
                action?.Invoke(true, responseModel.data);
            }
            else
            {
                action?.Invoke(false, null);
            }

        }, false));
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

    public void PostPortfolio(int id, int postedTo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("portfolio_id", id);

        parameters.Add("shared_to", postedTo);

        parameters.Add("access_modifier", 0);

        gameManager.StartCoroutine(PostRequest(APIConstants.PORTFOLIO_SHARE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetAlteredPortfolios(int pageNo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_ALTERED_PORTFOLIOS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("tab_name", "altered");

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetPortfolioPosts(int pageNo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_PORTFOLIO_POSTS;

        url += "?page=" + pageNo + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        parameters.Add("status", 0);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void ApplyPortfolioAlteredFilter(int roleId, int roleCategeryId, int statusId, int sortId, int orderId,  Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_ALTERED_PORTFOLIOS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        if (!roleId.Equals(-1))
        {
            parameters.Add("role_id", roleId);
        }

        if (!roleCategeryId.Equals(-1))
        {
            parameters.Add("role_category_id", roleCategeryId);
        }

        if (!sortId.Equals(-1))
        {
            parameters.Add("sortBy", sortId);
        }

        if (!statusId.Equals(-1))
        {
            parameters.Add("status", statusId);
        }

        if (!orderId.Equals(-1))
        {
            parameters.Add("sortOrder", orderId);
        }

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void ApplyPortfolioOfferedFilter(int sortId, int orderById, int roleId, int roleCategeryId, int ageFrom, int ageTo, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        string url = APIConstants.GET_PORTFOLIO_POSTS;

        url += "?page=" + 1 + "&limit=" + APIConstants.API_ITEM_LIMIT + "&count=" + APIConstants.API_ITEM_LIMIT;

        if (roleId.Equals(-1))
        {
            parameters.Add("role_id", roleId);
        }

        if (!roleCategeryId.Equals(-1))
        {
            parameters.Add("role_category_id", roleCategeryId);
        }

        if (!sortId.Equals(-1))
        {
            parameters.Add("sortBy", sortId);
        }
        if (!orderById.Equals(-1))
        {
            parameters.Add("sortOrder", orderById);
        }

        parameters.Add("age_from", ageFrom);

        parameters.Add("age_to", ageTo);

        gameManager.StartCoroutine(PostRequest(url, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdatePortfolioPostStatus(int postId, int postStatus, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", postId);

        parameters.Add("status", postStatus);

        gameManager.StartCoroutine(PutRequest(APIConstants.PORTFOLIO_SHARE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateProfileInfo(ProfileInfoModel infoModel, string idProof, string frontAddressProof, string backAddressProof, Action<bool, UserData> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        UserData user = DataManager.Instance.userInfo;

        parameters.Add("agree_terms_condition", 1);
        if(!infoModel.mail.Equals(user.email_id))
            parameters.Add("email_id", infoModel.mail);

        if (!infoModel.name.Equals(user.name))
            parameters.Add("name", infoModel.name);
        if (infoModel.dob != user.dob)
            parameters.Add("dob", infoModel.dob);
        if (!infoModel.contactNumber.Equals(user.current_location))
            parameters.Add("current_location", infoModel.currentLocation);
        if (!infoModel.nativeLocation.Equals(user.native_location))
            parameters.Add("native_location", infoModel.nativeLocation);
        if (!infoModel.contactNumber.Equals(user.phone))
            parameters.Add("phone", infoModel.contactNumber);

        parameters.Add("role_id", infoModel.roleId);

        parameters.Add("role_category_id", infoModel.categeryId);

        if (idProof.IsNOTNullOrEmpty())
        {
            parameters.Add("id_proof", idProof);
        }

        if (frontAddressProof.IsNOTNullOrEmpty())
        {
            parameters.Add("add_proof_front", frontAddressProof);
        }

        if (backAddressProof.IsNOTNullOrEmpty())
        {
            parameters.Add("add_proof_back", backAddressProof);
        }

        Debug.Log("profile_image = " + infoModel.profile_image);

        if (infoModel.profile_image.IsNOTNullOrEmpty())
        {
            parameters.Add("profile_image", infoModel.profile_image);
        }

        if (infoModel.aadherNumber.IsNOTNullOrEmpty())
        {
            parameters.Add("add_proof_identity", infoModel.aadherNumber);
        }

        if (infoModel.faceId.IsNOTNullOrEmpty())
        {
            parameters.Add("faceId", infoModel.faceId);
        }

        parameters.Add("isCeleb", infoModel.isCeleb);

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

    public void GetAvailableActvities(Action<bool, List<FeaturedModel>> OnResponse)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.UPDATE_USER_PROFILE, true, (status, response) => {

            if (status)
            {
                UserData userInfo = JsonUtility.FromJson<UserDataObject>(response).data;

                DataManager.Instance.UpdateUserAvailableActvities(userInfo.UserFeatures);

                OnResponse?.Invoke(status, userInfo.UserFeatures);
            }
        }));
    }
}

[Serializable]
public class PortfolioModel
{
    public int id;
    public int user_id;
    public string title;
    public int status;
    public int likes;
    public int access_modifier;
    public string description;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public List<MultimediaModel> PortfolioMedia;

    public MultimediaModel onScreenModel;
}


[Serializable]
public class PortfolioResponseModel : BaseResponse
{
    public List<PortfolioModel> data;
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
    public int start_date;
    public int end_date;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public List<MultimediaModel> WorkExpMedia;
    public IndustryModel MasterData;
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
    public int roleCategoryId;
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
    public string contactNumber;
    public string nativeLocation;
    public int roleId;
    public int categeryId;
    public string profile_image = "https://dummyimage.com/300.png/09f/fff";
    public string aadherNumber;
    public string faceId;
    public bool isCeleb;
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
    public ActivityOwnerModel Users;
    public List<MultimediaModel> PortfolioMedia;
}

[Serializable]
public class PortfolioActivityModel
{
    public int id;
    public int user_id;
    public int shared_to;
    public string status;
    public int sender_status;
    public int reciever_status;
    public int portfolio_id;
    public int access_modifier;
    public string comments;
    public DateTime created_date_time;
    public DateTime updatedAt;
    public UserData Users;
    public PortfolioMediaModel Portfolio;
}

[Serializable]
public class PortfolioPostResponse : BaseResponse
{
    public List<PortfolioActivityModel> data;
}

[Serializable]
public class ActivityOwnerModel
{
    public int id;
    public string name;
    public string profile_image;
}



using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void CreatePortfolio(string title, string description, List<Dictionary<string, object>> multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("port_multi_media", multimediaModels);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_PORTFOLIO, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void UpdateWorkExperiance(CreateExperianceModel experianceModel, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        List<PortMultiMediaModel> portMultimedias = new List<PortMultiMediaModel>();

        string jsonData = JsonUtility.ToJson(portMultimedias);

        parameters.Add("description", experianceModel.description);

        parameters.Add("start_date", experianceModel.startDate);

        parameters.Add("end_date", experianceModel.endDate);

        parameters.Add("industry_id", experianceModel.industryId);

        parameters.Add("role_id", experianceModel.roleId);

        parameters.Add("work_exp_media", experianceModel.multimediaModels);

        gameManager.StartCoroutine(PostRequest(APIConstants.UPDATE_EXPERIANCE, true, parameters, (status, response) => {

            action(status, response);
        }));
    }

    public void GetAllAlbums(Action<bool, List<PortfolioAlbumModel>> action)
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.USER_PORTFOLIO, true, (bool status, string response) => {

            if (status)
            {
                PortfolioResponseModel responseModel = JsonUtility.FromJson<PortfolioResponseModel>(response);
                action?.Invoke(true, responseModel.data[0].PortfolioMedia);
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
    public DateTime date_exp;
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
    public DateTime startDate;
    public DateTime endDate;
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



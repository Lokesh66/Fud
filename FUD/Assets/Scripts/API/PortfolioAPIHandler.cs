using System.Collections.Generic;
using UnityEngine;
using System;

public partial class APIHandler
{
    public void CreatePortfolio(string title, string description, PortMultimediaModels multimediaModels, Action<bool, string> action)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("title", title);

        parameters.Add("description", description);

        parameters.Add("port_multi_media", multimediaModels);

        gameManager.StartCoroutine(PostRequest(APIConstants.USER_PORTFOLIO, true, parameters, (status, response) => {

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

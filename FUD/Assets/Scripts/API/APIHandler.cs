using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;



public partial class APIHandler
{
    GameManager gameManager;

    public APIHandler(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public enum EHeaderType
    { 
        Login,
        Generic,
        Refresh
    }

    public GameObject o854252G;


    #region GET CraftRoles
    public void GetCraftRoles()
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.GET_CRATFS, false, (bool status, string response) => {
            if (status)
            {
                CraftsResponse data = JsonUtility.FromJson<CraftsResponse>(response);
                DataManager.Instance.UpdateCrafts(data.data);
            }
        }, false));
    }

    public void GetCastRoles(int id)
    {
        string url = APIConstants.GET_CRATFS + "?type=" + id;

        gameManager.StartCoroutine(GetRequest(url, false, (bool status, string response) =>
        {
            Debug.Log("response = " + response);

            if (status)
            {
                CraftsResponse data = JsonUtility.FromJson<CraftsResponse>(response);

                DataManager.Instance.UpdateCastCrafts(data.data);
            }
        }));
    }

    public void GetCrewRoles(int id)
    {
        string url = APIConstants.GET_CRATFS + "?type=" + id;

        gameManager.StartCoroutine(GetRequest(url, false, (bool status, string response) =>
        {
            Debug.Log("response = " + response);

            if (status)
            {
                CraftsResponse data = JsonUtility.FromJson<CraftsResponse>(response);

                DataManager.Instance.UpdateCrewCrafts(data.data);
            }
        }));
    }

    #endregion

    #region GET Genres
    public void GetGenres()
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.GET_GENRES, false, (bool status, string response) =>
        {
            GenreResponse data = JsonUtility.FromJson<GenreResponse>(response);

            if (status)
            {
                DataManager.Instance.UpdateGenres(data.data);
            }
            else {
                CreateAlert(data.message);
            }
        }, false));

    }
    #endregion

    public void GetMediaEndPoint()
    {
        gameManager.StartCoroutine(GetRequest(APIConstants.MEDIA_KEY, false, (bool status, string response) =>
        {
            if (status)
            {
                MediaEndPointResponse responseModel = JsonUtility.FromJson<MediaEndPointResponse>(response);

                DataManager.Instance.UpdateMediaKey(responseModel.data.name);
            }
        }, false));

    }

    public enum ImageFilterMode : int
    {
        Nearest = 0,
        Biliner = 1,
        Average = 2
    }

    #region Get Headers

    public Dictionary<string, string> GetHeaders(EHeaderType headerType, string bodyParameters = "")
    {
        Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

        keyValuePairs["Content-Type"] = "application/json";

        switch (headerType)
        {
            case EHeaderType.Login:

                keyValuePairs["signature"] = CalculateMD5Hash(o854252G.name + bodyParameters);

                keyValuePairs["appversion"] = Application.version;

#if UNITY_IOS
      keyValuePairs["os_type"] = "iOS";
#elif UNITY_ANDROID
                keyValuePairs["os_type"] = "Android";
#endif

                break;

            case EHeaderType.Generic:
                keyValuePairs["signature"] = CalculateMD5Hash(o854252G.name + bodyParameters);

                keyValuePairs["appversion"] = Application.version;

                keyValuePairs["token"] = GetToken();

#if UNITY_IOS
      keyValuePairs["os_type"] = "iOS";
#elif UNITY_ANDROID
                keyValuePairs["os_type"] = "Android";
#endif
                break;

            case EHeaderType.Refresh:
                keyValuePairs["signature"] = CalculateMD5Hash(o854252G.name + bodyParameters);

                keyValuePairs["appversion"] = Application.version;

                keyValuePairs["token"] = GetToken();

#if UNITY_IOS
      keyValuePairs["os_type"] = "iOS";
#elif UNITY_ANDROID
                keyValuePairs["os_type"] = "Android";
#endif
                break;
        }

        keyValuePairs["appid"] = string.Empty;

        return keyValuePairs;
    }

    public static string CalculateMD5Hash(string s)
    {
        // Form hash
        System.Security.Cryptography.MD5 h = System.Security.Cryptography.MD5.Create();
        byte[] data = h.ComputeHash(System.Text.Encoding.Default.GetBytes(s));
        // Create string representation
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < data.Length; ++i)
        {
            sb.Append(data[i].ToString("x2"));
        }
        return sb.ToString();
    }

    #endregion

    #region TOKEN
    public string GetToken()
    {
        if (File.Exists(APIConstants.TOKEN_PATH))
        {
            return "Bearer " + File.ReadAllText(APIConstants.TOKEN_PATH);
        }
        return string.Empty;
    }

    public void SetToken(string token)
    {
        File.WriteAllText(APIConstants.TOKEN_PATH, token);
    }
    #endregion

    IEnumerator GetRequest(string url, bool isAuth, Action<bool, string> OnResponse, bool canShowLoader = true)
    {
        if (canShowLoader)
        {
            Loader.Instance.StartLoading();
        }

        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        if (isAuth)
        {
            webRequest.SetRequestHeader("Authorization", GetToken());
        }

        Debug.LogFormat("URL ({0}) ", url);

        Debug.Log("GetToken() = " + GetToken());

        yield return webRequest.SendWebRequest();

        Loader.Instance.StopLoading();
        
        if (webRequest.responseCode.Equals(401))
        {
            GameManager.Instance.SessionExpired();       
        }
        else if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogErrorFormat("<APIManager/GetRequest> Error ({0})", webRequest.error);

            OnResponse?.Invoke(false, webRequest.downloadHandler.text);

            BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(webRequest.downloadHandler.text);

            CreateAlert(responseModel.message);
        }
        else
        {
            Debug.LogFormat("<APIManager/GetRequest> Response ({0} ------ {1})",url, webRequest.downloadHandler.text);
            string response = webRequest.downloadHandler.text;
            OnResponse?.Invoke(true, response);
        }
    }

    IEnumerator PostRequest(string url, bool isAuth, Dictionary<string, object> parameters, Action<bool, string> callback)
    {
        Loader.Instance.StartLoading();

        Dictionary<string, Dictionary<string, object>> attributes = new Dictionary<string, Dictionary<string, object>>();

        attributes.Add("attributes", parameters);

        string jsonData = MiniJSON.Json.Serialize(attributes);

        Debug.LogFormat("URL ({0}) Data ({1})", url, jsonData);

        UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonData);

        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));

        webRequest.SetRequestHeader("Content-Type", "application/json");

        if (isAuth)
        {
            Debug.Log("GetToken() = " + GetToken());

            webRequest.SetRequestHeader("Authorization", GetToken());
        }

        yield return webRequest.SendWebRequest();

        Debug.Log("webRequest.downloadHandler.text  = " + webRequest.downloadHandler.text);

        Loader.Instance.StopLoading();

        if (webRequest.responseCode.Equals(401))
        {
            GameManager.Instance.SessionExpired();
        }
        else if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogErrorFormat("<APIManager/ POST/ ({0})> Error ({1})", webRequest.downloadHandler.text, url);
            callback?.Invoke(false, webRequest.downloadHandler.text);

            BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(webRequest.downloadHandler.text);

            CreateAlert(responseModel.message);
        }
        else
        {
            Debug.LogFormat("<APIManager/ POST/ ({0})> Response ({1})", webRequest.downloadHandler.text, url);
            callback?.Invoke(true, webRequest.downloadHandler.text);
        }
    }

    IEnumerator PutRequest(string url, bool isAuth, Dictionary<string, object> parameters, Action<bool, string> callback)
    {
        Loader.Instance.StartLoading();
        Dictionary<string, Dictionary<string, object>> attributes = new Dictionary<string, Dictionary<string, object>>();

        attributes.Add("attributes", parameters);

        string jsonData = MiniJSON.Json.Serialize(attributes);

        Debug.LogFormat("URL ({0}) Data ({1})", url, jsonData);

        UnityWebRequest webRequest = UnityWebRequest.Put(url, jsonData);

        webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));

        webRequest.SetRequestHeader("Content-Type", "application/json");

        if (isAuth)
        {
            webRequest.SetRequestHeader("Authorization", GetToken());
        }

        /*Dictionary<string, string> headers = GetHeaders(EHeaderType.Generic, jsonData);

        foreach (KeyValuePair<string, string> header in headers)
        {
            webRequest.SetRequestHeader(header.Key, header.Value);
        }*/

        yield return webRequest.SendWebRequest();

        Loader.Instance.StopLoading();

        Debug.Log("@@@@@ : "+webRequest.downloadHandler.text);

        if (webRequest.responseCode.Equals(401))
        {
            GameManager.Instance.SessionExpired();
        }
        else if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogErrorFormat("<APIManager/ PUT/ ({0})> Error ({1})", webRequest.downloadHandler.text, url);
            callback?.Invoke(false, webRequest.downloadHandler.text);

            BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(webRequest.downloadHandler.text);

            CreateAlert(responseModel.message);
        }
        else
        {
            Debug.LogFormat("<APIManager/ PUT/ ({0})> Response ({1})", webRequest.downloadHandler.text, url);
            callback?.Invoke(true, webRequest.downloadHandler.text);
        }
    }

    IEnumerator Upload(string filePath, EMediaType mediaType, string mediaSource, string faceId, Action<bool, string> responseCallBack)
    {
        byte[] img = File.ReadAllBytes(filePath);

        WWWForm form = new WWWForm();

        if (mediaType == EMediaType.Image)
        {
            form.AddBinaryData("file", img, filePath, "image/png");
        }
        else if (mediaType == EMediaType.Audio)
        {
            form.AddBinaryData("file", img, filePath, "audio/*");
        }
        else if (mediaType == EMediaType.Video)
        {
            form.AddBinaryData("file", img, filePath, "video/*");
        }
        else if (mediaType == EMediaType.Document)
        {
            form.AddBinaryData("file", img, filePath, "text/pdf");
        }

        form.AddField("image_source", mediaSource);

        string url = string.Empty;

        Debug.Log("mediaSource = " + mediaSource);

        if (mediaSource.Equals("profile"))
        {
            if (faceId.IsNOTNullOrEmpty())
            {
                form.AddField("face_id", faceId);
            }

            url = APIConstants.PROFILE_MEDIA_URL;
        }
        else {
            url = APIConstants.MEDIA_URL;
        }

        Debug.Log("url = " + url);

        Debug.Log("Data = " + MiniJSON.Json.Serialize(form));

        UnityWebRequest request = UnityWebRequest.Post(url, form);

        request.SetRequestHeader("Authorization", GetToken());

        yield return request.SendWebRequest();

        Debug.Log("request.downloadHandler.text = " + request.downloadHandler.text);

        BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(request.downloadHandler.text);

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            responseCallBack?.Invoke(false, request.error);
            CreateAlert(responseModel.message);
            //statusText.text = "StATUS Failed " + request.error;
        }
        else
        {
            Debug.Log("Form upload complete!");
            //statusText.text = "sTatus Success = " + request.downloadHandler.text;
            responseCallBack?.Invoke(true, request.downloadHandler.text);
        }
    }

    public void DownloadImage(string imageurl, Action<Sprite> CallBack)
    {
        Debug.Log("DownloadImage Called");

        if (string.IsNullOrEmpty(imageurl))
        {
            CallBack?.Invoke(null);
        }

        string path = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, Path.GetFileName(imageurl));


        if (File.Exists(path))
        {
            LoadPNG(path, CallBack);
        }
        else
        {
            gameManager.StartCoroutine(DownloadImageAndSave(imageurl, CallBack));
        }
    }

    public void CreateAlert(string message)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = message;

        UIManager.Instance.ShowAlert(alertModel);
    }

    IEnumerator DownloadImageAndSave(string imageURL, Action<Sprite> OnResponse)
    {
        Loader.Instance.StartLoading();

        //string url = APIConstants.MEDIA_UPLOAD_BASE_URL + "adam/v1/downloadFile?" + "key_name=" + imageURL;

        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageURL);

        //webRequest.SetRequestHeader("Authorization", GetToken());

        Debug.LogFormat("URL ({0}) ", imageURL);

        yield return webRequest.SendWebRequest();

        Loader.Instance.StopLoading();

        if (webRequest.responseCode.Equals(401))
        {
            GameManager.Instance.SessionExpired();
        }
        if (webRequest.error.IsNOTNullOrEmpty())
        {
            Debug.LogErrorFormat("<APIManager/GetRequest> Error ({0})", webRequest.error);

            OnResponse?.Invoke(null);
        }
        else
        {
            string path = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, Path.GetFileName(imageURL));

            Texture2D myTexture = DownloadHandlerTexture.GetContent(webRequest);

            Sprite profileSprite = Sprite.Create(myTexture, new Rect(0f, 0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0f));//creates sprite from the texture of the image

            byte[] texByte = myTexture.EncodeToPNG();//to convert texture to png

            File.WriteAllBytes(path, texByte);

            OnResponse?.Invoke(profileSprite);
        }
    }

    void LoadPNG(string filePath, Action<Sprite> CallBack)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(1024, 1024, TextureFormat.ETC2_RGB, false);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        Sprite ProfilePic = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        CallBack(ProfilePic);
    }
}

[Serializable]
public class MediaEndPointResponse : BaseResponse
{
    public MediaEndPointModel data;
}

public class MediaEndPointModel
{
    public int id;
    public string name;
    public string type;
    public object image_url;
    public object created_date_time;
    public object update_date_time;
    public int status;
}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;


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

    IEnumerator GetCrafts(string url, Action<bool, string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            //webRequest.SetRequestHeader("Authorization", "Bearer " + APIConstants.TOKEN);

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogErrorFormat("<APIManager/GetRequest> Error ({0})", webRequest.error);

                callback?.Invoke(false, webRequest.error);
            }
            else
            {
                Debug.LogFormat("<APIManager/GetRequest> Response ({0})", webRequest.downloadHandler.text);
                string response = webRequest.downloadHandler.text;
                callback?.Invoke(true, response);
            }
        }
    }

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
    #endregion

    #region GET Genres
    public void GetGenres()
    {
        /*        string path = Application.streamingAssetsPath + "/GenreResponse";
                string response = File.ReadAllText(path);

                GenreResponse data = JsonUtility.FromJson<GenreResponse>(response);
                DataManager.Instance.UpdateGenres(data.data);*/

        gameManager.apiCallingText.text = "API Calling";

        gameManager.StartCoroutine(GetRequest(APIConstants.GET_GENRES, false, (bool status, string response) =>
        {
            GenreResponse data = JsonUtility.FromJson<GenreResponse>(response);

            gameManager.starGameText.text = string.Format("status ({0}), responseModel {1}, Count {2}", status, data, data?.data?.Count);


            if (status)
            {
                DataManager.Instance.UpdateGenres(data.data);
            }
            else {
                AlertModel alertModel = new AlertModel();

                alertModel.message = data.message;

                UIManager.Instance.ShowAlert(alertModel);
            }
        }, false));

    }
    #endregion

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

        /*Dictionary<string, string> headers = GetHeaders(EHeaderType.Generic, jsonData);

        foreach (KeyValuePair<string, string> header in headers)
        {
            webRequest.SetRequestHeader(header.Key, header.Value);
        }*/

        yield return webRequest.SendWebRequest();

        Loader.Instance.StopLoading();

        if (webRequest.responseCode.Equals(401))
        {
            GameManager.Instance.SessionExpired();
        }
        else if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogErrorFormat("<APIManager/ POST/ ({0})> Error ({1})", webRequest.downloadHandler.text, url);
            callback?.Invoke(false, webRequest.downloadHandler.text);
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
        }
        else
        {
            Debug.LogFormat("<APIManager/ PUT/ ({0})> Response ({1})", webRequest.downloadHandler.text, url);
            callback?.Invoke(true, webRequest.downloadHandler.text);
        }
    }

    IEnumerator Upload(string filePath, EMediaType mediaType, string mediaSource, Action<bool, string> responseCallBack)
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

        form.AddField("image_source", mediaSource);

        UnityWebRequest request = UnityWebRequest.Post(APIConstants.MEDIA_URL, form);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            responseCallBack?.Invoke(false, request.error);
            //statusText.text = "StATUS Failed " + request.error;
        }
        else
        {
            Debug.Log("Form upload complete!");
            //statusText.text = "sTatus Success = " + request.downloadHandler.text;
            responseCallBack?.Invoke(true, request.downloadHandler.text);
        }
    }
}

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
        }));
    }
    #endregion

    #region GET Genres
    public void GetGenres()
    {
        /*        string path = Application.streamingAssetsPath + "/GenreResponse";
                string response = File.ReadAllText(path);

                GenreResponse data = JsonUtility.FromJson<GenreResponse>(response);
                DataManager.Instance.UpdateGenres(data.data);*/

        gameManager.StartCoroutine(GetRequest(APIConstants.GET_GENRES, false, (bool status, string response) =>
        {
            if (status)
            {
                GenreResponse data = JsonUtility.FromJson<GenreResponse>(response);
                DataManager.Instance.UpdateGenres(data.data);
            }
        }));
    }
    #endregion

    #region Imagedownload

    public void DownloadImage(string imageurl, Action<Sprite> CallBack)
    {
        if (string.IsNullOrEmpty(imageurl))//.IsNullOrEmpty())
        {
            CallBack(null);
            return;
        }
        var m = Regex.Match(imageurl, ".+(/.+)$");
        string imageName = m.Groups[1].Value.Remove(0, 1);
        string path = Application.persistentDataPath + "/" + imageName;
        //path = "file://" + path;
        if (File.Exists(path))
        {
            //path = "file://" + path;
            //StartCoroutine(GetImage(path, CallBack));
            LoadPNG(path, CallBack);
        }
        else
        {
            gameManager.StartCoroutine(DownloadAndSave(imageurl, CallBack));
        }
    }

    IEnumerator DownloadAndSave(string imageurl, Action<Sprite> CallBack)
    {
        if (!string.IsNullOrEmpty(imageurl))//.IsNOTNullOrEmpty())
        {
            WWW www = new WWW(imageurl);
            yield return www;
            string response = www.text;
            if (!string.IsNullOrEmpty(response) && MiniJSON.Json.Deserialize(response) != null)
            {
                Dictionary<string, object> jsResponse = MiniJSON.Json.Deserialize(response) as Dictionary<string, object>;
                object error = jsResponse["error"];
                if (error != null)
                {
                    CallBack(null);
                }
            }
            else if (www.texture != null)
            {
                var m = Regex.Match(imageurl, ".+(/.+)$");
                string imageName = m.Groups[1].Value.Remove(0, 1);

                string path = Application.persistentDataPath + "/" + imageName;
                Sprite profileSprite = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));//creates sprite from the texture of the image
                CallBack(profileSprite);
                byte[] texByte = www.texture.EncodeToPNG();//to convert texture to png 
                File.WriteAllBytes(path, texByte);
            }
            else
            {
                CallBack(null);
            }
            www.Dispose();
            www = null;
        }
        else
        {
            CallBack(null);
        }
    }

    void LoadPNG(string filePath, Action<Sprite> CallBack)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        Sprite ProfilePic = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        CallBack(ProfilePic);
    }

    IEnumerator GetImage(string path, Action<Sprite> CallBack)
    {
        WWW www = new WWW(path);
        yield return www;
        Texture2D texTmp = new Texture2D(1024, 1024, TextureFormat.DXT1, false);
        //LoadImageIntoTexture compresses JPGs by DXT1 and PNGs by DXT5     
        www.LoadImageIntoTexture(texTmp);

        Sprite ProfilePic = Sprite.Create(www.texture, new Rect(0f, 0f, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
        CallBack(ProfilePic);
    }

    public enum ImageFilterMode : int
    {
        Nearest = 0,
        Biliner = 1,
        Average = 2
    }

    public static Texture2D ResizeTexture(Texture2D pSource, ImageFilterMode pFilterMode, float pScale)
    {

        //			byte[] response = null;


        //*** Variables
        int i;

        //*** Get All the source pixels
        Color[] aSourceColor = pSource.GetPixels(0);
        Vector2 vSourceSize = new Vector2(pSource.width, pSource.height);

        //*** Calculate New Size
        float xWidth = pSource.width / 2;//Mathf.RoundToInt ((float)pSource.width / 2 * pScale);                     
        float xHeight = pSource.height / 2;//Mathf.RoundToInt ((float)pSource.height / 2 * pScale);

        //*** Make New
        Texture2D oNewTex = new Texture2D((int)xWidth, (int)xHeight, TextureFormat.RGBA32, false);

        //*** Make destination array
        int xLength = (int)xWidth * (int)xHeight;
        Color[] aColor = new Color[xLength];

        Vector2 vPixelSize = new Vector2(vSourceSize.x / xWidth, vSourceSize.y / xHeight);

        //*** Loop through destination pixels and process
        Vector2 vCenter = new Vector2();
        for (i = 0; i < xLength; i++)
        {

            //*** Figure out x&y
            float xX = (float)i % xWidth;
            float xY = Mathf.Floor((float)i / xWidth);

            //*** Calculate Center
            vCenter.x = (xX / xWidth) * vSourceSize.x;
            vCenter.y = (xY / xHeight) * vSourceSize.y;

            //*** Do Based on mode
            //*** Nearest neighbour (testing)
            if (pFilterMode == ImageFilterMode.Nearest)
            {

                //*** Nearest neighbour (testing)
                vCenter.x = Mathf.Round(vCenter.x);
                vCenter.y = Mathf.Round(vCenter.y);

                //*** Calculate source index
                int xSourceIndex = (int)((vCenter.y * vSourceSize.x) + vCenter.x);

                //*** Copy Pixel
                aColor[i] = aSourceColor[xSourceIndex];
            }

            //*** Bilinear
            else if (pFilterMode == ImageFilterMode.Biliner)
            {

                //*** Get Ratios
                float xRatioX = vCenter.x - Mathf.Floor(vCenter.x);
                float xRatioY = vCenter.y - Mathf.Floor(vCenter.y);

                //*** Get Pixel index's
                int xIndexTL = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
                int xIndexTR = (int)((Mathf.Floor(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));
                int xIndexBL = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Floor(vCenter.x));
                int xIndexBR = (int)((Mathf.Ceil(vCenter.y) * vSourceSize.x) + Mathf.Ceil(vCenter.x));

                //*** Calculate Color
                aColor[i] = Color.Lerp(
                    Color.Lerp(aSourceColor[xIndexTL], aSourceColor[xIndexTR], xRatioX),
                    Color.Lerp(aSourceColor[xIndexBL], aSourceColor[xIndexBR], xRatioX),
                    xRatioY
                );
            }

            //*** Average
            else if (pFilterMode == ImageFilterMode.Average)
            {

                //*** Calculate grid around point
                int xXFrom = (int)Mathf.Max(Mathf.Floor(vCenter.x - (vPixelSize.x * 0.5f)), 0);
                int xXTo = (int)Mathf.Min(Mathf.Ceil(vCenter.x + (vPixelSize.x * 0.5f)), vSourceSize.x);
                int xYFrom = (int)Mathf.Max(Mathf.Floor(vCenter.y - (vPixelSize.y * 0.5f)), 0);
                int xYTo = (int)Mathf.Min(Mathf.Ceil(vCenter.y + (vPixelSize.y * 0.5f)), vSourceSize.y);

                //*** Loop and accumulate
                //					Vector4 oColorTotal = new Vector4 ();
                Color oColorTemp = new Color();
                float xGridCount = 0;
                for (int iy = xYFrom; iy < xYTo; iy++)
                {
                    for (int ix = xXFrom; ix < xXTo; ix++)
                    {

                        //*** Get Color
                        oColorTemp += aSourceColor[(int)(((float)iy * vSourceSize.x) + ix)];

                        //*** Sum
                        xGridCount++;
                    }
                }

                //*** Average Color
                aColor[i] = oColorTemp / (float)xGridCount;
            }
        }

        //*** Set Pixels
        oNewTex.SetPixels(aColor);
        oNewTex.Apply();
        return oNewTex;


        //*** Return
    }

    #endregion

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

    IEnumerator GetRequest(string url, bool isAuth, Action<bool, string> OnResponse)
    {
        Loader.Instance.StartLoading();
        UnityWebRequest webRequest = UnityWebRequest.Get(url);

        if (isAuth)
        {
            webRequest.SetRequestHeader("Authorization", GetToken());
        }

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

            OnResponse?.Invoke(false, webRequest.error);
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
            callback?.Invoke(false, webRequest.error);
        }
        else
        {
            Debug.LogFormat("<APIManager/ PUT/ ({0})> Response ({1})", webRequest.downloadHandler.text, url);
            callback?.Invoke(true, webRequest.downloadHandler.text);
        }
    }

    IEnumerator Upload(string filePath, EMediaType mediaType, Action<bool, string> responseCallBack)
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

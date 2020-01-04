using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Text.RegularExpressions;

public class APIManager : MonoBehaviour
{
    #region Singleton

    private static APIManager instance = null;
    private APIManager()
    {

    }

    public static APIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<APIManager>();
            }
            return instance;
        }
    }

    #endregion

    public const string SECRET_KEY = "SSht6KyxvwKZnsmbbbQUCm2va";
    public const string BASE_URL = "http://3.136.106.58:7000/";

    public const string USER = BASE_URL + "v1/users/";
    public const string USER_LOGIN = BASE_URL + "v1/login/";
    public const string USER_OTP = BASE_URL + "v1/sendOtp";
    public const string STORY = BASE_URL + "v1/story/";

    public const string GET_CRATFS = BASE_URL + "v1/24_craftRoles/";
    public const string GET_GENRES = BASE_URL + "v1/genres/";

    IEnumerator PostRequest(string url, Dictionary<string, string> parameters, Action<bool, string> callback)
    {
        Dictionary<string, Dictionary<string, string>> attributes = new Dictionary<string, Dictionary<string, string>>();

        attributes.Add("attributes", parameters);

        string jsonData = MiniJSON.Json.Serialize(attributes);

        Debug.LogFormat("URL ({0}) Data ({1})", url, jsonData);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, jsonData))
        {
            webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));

            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogErrorFormat("<APIManager/({0})> Error ({1})", webRequest.error, url);
                callback?.Invoke(false, webRequest.error);
            }
            else
            {
                Debug.LogFormat("<APIManager/ ({0})> Response ({1})", webRequest.downloadHandler.text, url);
                callback?.Invoke(true, webRequest.downloadHandler.text);
            }
        }
    }

    IEnumerator GetRequest(string url, Action<bool, string> callback)
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
        StartCoroutine(GetRequest(GET_CRATFS, (bool status, string response) => {
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

        StartCoroutine(GetRequest(GET_GENRES, (bool status, string response) =>
        {
            if (status)
            {
                GenreResponse data = JsonUtility.FromJson<GenreResponse>(response);
                DataManager.Instance.UpdateGenres(data.data);
            }
        }));
    }
    #endregion

    #region OTP
    public void SendOTP(string phoneNumber, Action<bool> action)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("phone",phoneNumber);
        StartCoroutine(PostRequest(USER_OTP, parameters, (bool status, string response) => {
            action?.Invoke(status);
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
            StartCoroutine(DownloadAndSave(imageurl, CallBack));
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

}

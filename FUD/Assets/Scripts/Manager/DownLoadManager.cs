using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class DownLoadManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    #region Imagedownload

    public void DownloadImage(string imageurl, Action<Sprite> CallBack)
    {
        if (string.IsNullOrEmpty(imageurl))
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
        if (!string.IsNullOrEmpty(imageurl))
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

    public Texture2D GetLocalTexture(string imageurl)
    {
        if (string.IsNullOrEmpty(imageurl))
        {
            return null;
        }

        Texture2D texture = null;

        var m = Regex.Match(imageurl, ".+(/.+)$");
        string imageName = m.Groups[1].Value.Remove(0, 1);
        string path = Application.persistentDataPath + "/" + imageName;

        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);

            texture = new Texture2D(1024, 1024, TextureFormat.DXT1, false);

            texture.LoadImage(fileData);
        }

        return texture;
    }

    #endregion
}

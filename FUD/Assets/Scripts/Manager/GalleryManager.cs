using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using SimpleFileBrowser;


public class GalleryManager : MonoBehaviour
{
    public TextMeshProUGUI loadingCountText;


    Action<bool, List<string>> OnUploaded;

    private List<string> uploadedURLs = new List<string>();

    string[] loadedFiles = new string[10];

    private int selectedImagesCount;

    #region Singleton

    private static GalleryManager instance = null;
    private GalleryManager()
    {

    }

    public static GalleryManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GalleryManager>();
            }
            return instance;
        }
    }

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void GetImageFromGallaery(string mediaSource, Action<bool, List<string>> OnImageUploaded)
    {
        //string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "banner2.png");

        //uploadedURLs.Clear();

        //this.OnUploaded = OnImageUploaded;

        //Array.Clear(loadedFiles, 0, loadedFiles.Length);

        //loadedFiles[0] = _filePath;

        //selectedImagesCount = 1;

        //for (int i = 0; i < 1; i++)
        //{
        //    UploadFile(_filePath, EMediaType.Image, mediaSource);
        //}

        //return;

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((imagesPath) => {
            if (imagesPath != null)
            {
                uploadedURLs.Clear();

                this.OnUploaded = OnImageUploaded;
                selectedImagesCount = 1;
                UploadFile(imagesPath, EMediaType.Image, mediaSource);
            }
            else
            {
                OnUploaded?.Invoke(false, null);
            }
        }, "Select a PNG image");

    }

    public void TakeSelfie(string mediaSource,Action<bool, List<string>> OnImageUploaded)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((imagePath) => {

            if (imagePath != null)
            {
                uploadedURLs.Clear();

                this.OnUploaded = OnImageUploaded;

                loadedFiles[0] = imagePath;

                selectedImagesCount = 1;
                UploadFile(imagePath, EMediaType.Image, mediaSource);
            }
            else
            {
                OnUploaded?.Invoke(false, null);
            }

        }, preferredCamera: NativeCamera.PreferredCamera.Front);
    }

    public void PickImages(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
        string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "banner2.png");

        uploadedURLs.Clear();

        this.OnUploaded = OnUploaded;

        Array.Clear(loadedFiles, 0, loadedFiles.Length);

        loadedFiles[0] = _filePath;

        selectedImagesCount = 1;

        for (int i = 0; i < 1; i++)
        {
            UploadFile(_filePath, EMediaType.Image, mediaSource);
        }

        return;

#if UNITY_ANDROID
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((imagesPath) =>
        {
            if (loadedFiles != null && loadedFiles.Length > 0)
            {
                Array.Clear(loadedFiles, 0, loadedFiles.Length);
            }

            if (imagesPath != null && imagesPath.Length > 0)
            {
                uploadedURLs.Clear();

                loadedFiles = imagesPath;

                this.OnUploaded = OnUploaded;

                selectedImagesCount = imagesPath.Length;

                if (imagesPath != null && imagesPath.Length > 0)
                {
                    selectedImagesCount = imagesPath.Length;

                    for (int i = 0; i < imagesPath.Length; i++)
                    {
                        UploadFile(imagesPath[i], EMediaType.Image, mediaSource);
                    }
                }
            }
            else {
                OnUploaded?.Invoke(false, null);
            }
        }, "Select a PNG image");
        AlertMessage.Instance.SetText("Permission result: " + permission);
        Debug.Log("Permission result: " + permission);
#elif UNITY_IOS
        PickImage(mediaSource, OnUploaded);
#endif
    }

    public void GetAudiosFromGallery(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
#if UNITY_ANDROID
        NativeGallery.Permission permission = NativeGallery.GetAudiosFromGallery((audiosPaths) =>
        {
            if (audiosPaths != null && audiosPaths.Length > 0)
            {
                this.OnUploaded = OnUploaded;

                selectedImagesCount = audiosPaths.Length;

                loadedFiles = audiosPaths;

                for (int i = 0; i < audiosPaths.Length; i++)
                {
                    UploadFile(audiosPaths[i], EMediaType.Audio, mediaSource);
                }
            }
        });
#elif UNITY_IOS
        GetAudioFromGallery(mediaSource, OnUploaded);
#endif
    }

    public void GetVideosFromGallery(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
#if UNITY_ANDROID
        NativeGallery.Permission permission = NativeGallery.GetVideosFromGallery((videoPaths) =>
        {
            if (videoPaths != null && videoPaths.Length > 0)
            {
                this.OnUploaded = OnUploaded;

                selectedImagesCount = videoPaths.Length;

                loadedFiles = videoPaths;

                for (int i = 0; i < videoPaths.Length; i++)
                {
                    UploadFile(videoPaths[i], EMediaType.Video, mediaSource);
                }
            }
        });
#elif UNITY_IOS
    GetVideoFromGallery(mediaSource, OnUploaded);
#endif
    }

    public void UploadVideoFile(string filePath, string mediaSource, Action<bool, List<string>> OnUploaded)
    {
        this.OnUploaded = OnUploaded;

        selectedImagesCount = 1;

        UploadFile(filePath, EMediaType.Video, mediaSource);
    }

    public void RecordVideo()
    { 
    
    }

#region Upload File

    void UploadFile(string filePath, EMediaType mediaType, string mediaSource)
    {
        Loader.Instance.StartLoading();

        GameManager.Instance.apiHandler.UploadFile(filePath, mediaType, mediaSource, (status, response) => {

            if (status) 
            {
                FileUploadResponseModel responseModel = JsonUtility.FromJson<FileUploadResponseModel>(response);

                uploadedURLs.Add(responseModel.data.s3_file_path);

                //Debug.Log("Upoaded URL = " + responseModel.data.s3_file_path);

                //Debug.Log("Upoaded Count = " + uploadedURLs.Count + " selectedImagesCount = " + selectedImagesCount);

                if (uploadedURLs.Count == selectedImagesCount)
                {
                    UpdateLocalData(uploadedURLs, mediaType);

                    OnUploaded?.Invoke(true, uploadedURLs);

                    selectedImagesCount = 0;

                    AlertModel alertModel = new AlertModel();

                    alertModel.message = responseModel.message;

                    UIManager.Instance.ShowAlert(alertModel);

                    uploadedURLs.Clear();

                    OnUploaded = null;
                }
                else {
                   /* List<string> responses = new List<string>();

                    responses.Add("imageUrls.Count, selectedImagesCount are not equal");

                    AlertModel alertModel = new AlertModel();

                    alertModel.message = "imageUrls.Count, selectedImagesCount are not equal";

                    //alertModel.okayButtonAction = AlertDismissAction;

                    UIManager.Instance.ShowAlert(alertModel);

                    OnUploaded?.Invoke(false, responses);*/
                }
            }
            else
            {
                List<string> responses = new List<string>();

                responses.Add(response);

                OnUploaded?.Invoke(false, responses);

                OnUploaded = null;
            }

            Loader.Instance.StopLoading();

        });
    }

    void UpdateLocalData(List<string> imageURls, EMediaType mediaType)
    {
        string filePath = string.Empty;

        switch (mediaType)
        {
            case EMediaType.Image:
                filePath = Path.Combine(Application.persistentDataPath, "GalleryImages");
                break;
            case EMediaType.Audio:
                filePath = Path.Combine(Application.persistentDataPath, "GalleryAudios");
                break;
            case EMediaType.Video:
                filePath = Path.Combine(Application.persistentDataPath, "GalleryVideos");
                break;
        }

        if (!string.IsNullOrEmpty(filePath))
        {
            //string directoryName = Path.GetDirectoryName(filePath);

            /*if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(directoryName);
            }

            for (int i = 0; i < imageURls.Count; i++)
            { 
                
            }*/
        }
    }

    public string[] GetLoadedFiles()
    {
        return loadedFiles;
    }

    public void PickImage(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
        //string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "banner2.png");

        //uploadedURLs.Clear();

        //this.OnUploaded = OnUploaded;

        //Array.Clear(loadedFiles, 0, loadedFiles.Length);

        //loadedFiles[0] = _filePath;

        //selectedImagesCount = 1;

        //for (int i = 0; i < 1; i++)
        //{
        //    UploadFile(_filePath, EMediaType.Image, mediaSource);
        //}

        //return;

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((imagePath) =>
        {
            if (imagePath != null)
            {
                uploadedURLs.Clear();

                loadedFiles[0] = imagePath;

                this.OnUploaded = OnUploaded;

                if (imagePath != null)
                {
                    selectedImagesCount = 1;

                    UploadFile(imagePath, EMediaType.Image, mediaSource);
                }
            }
        });
    }

    public void GetVideoFromGallery(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
        NativeGallery.Permission permission = NativeGallery.GetVideoFromGallery((imagePath) =>
        {
            if (imagePath != null)
            {
                uploadedURLs.Clear();

                loadedFiles[0] = imagePath;

                this.OnUploaded = OnUploaded;

                if (imagePath != null)
                {
                    selectedImagesCount = 1;

                    UploadFile(imagePath, EMediaType.Video, mediaSource);
                }
            }
        });
    }

    public void GetAudioFromGallery(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
        NativeGallery.Permission permission = NativeGallery.GetAudioFromGallery((imagePath) =>
        {
            if (imagePath != null)
            {
                uploadedURLs.Clear();

                loadedFiles[0] = imagePath;

                this.OnUploaded = OnUploaded;

                if (imagePath != null)
                {
                    selectedImagesCount = 1;

                    UploadFile(imagePath, EMediaType.Audio, mediaSource);
                }
            }
        });
    }

    #endregion
}

public enum EMediaType
{ 
    Image,
    Audio,
    Video
}

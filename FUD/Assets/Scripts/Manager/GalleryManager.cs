using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;


public class GalleryManager : MonoBehaviour
{
    public TextMeshProUGUI loadingCountText;


    Action<bool, List<string>> OnUploaded;

    private List<string> uploadedURLs = new List<string>();

    List<MultimediaModel> mediaList = new List<MultimediaModel>();

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

    public void GetImageFromGallaery(string mediaSource, Action<bool, List<string>> OnImageUploaded)
    {
#if UNITY_EDITOR
        string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "Banner+3.png");

        this.OnUploaded = OnImageUploaded;

        mediaList.Clear();

        selectedImagesCount = 1;

        for (int i = 0; i < 1; i++)
        {
            UploadFile(_filePath, EMediaType.Image, mediaSource);
        }

#else

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((imagesPath) => {
            if (imagesPath != null)
            {
                mediaList.Clear();

                this.OnUploaded = OnImageUploaded;
                selectedImagesCount = 1;
                UploadFile(imagesPath, EMediaType.Image, mediaSource);
            }
            else
            {
                OnUploaded?.Invoke(false, null);
            }
        }, "Select a PNG image");

#endif

    }

    public void TakeSelfie(string mediaSource,Action<bool, List<string>> OnImageUploaded)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((imagePath) => {

            if (imagePath != null)
            {
                mediaList.Clear();

                this.OnUploaded = OnImageUploaded;

                selectedImagesCount = 1;
                UploadFile(imagePath, EMediaType.Image, mediaSource);
            }
            else
            {
                OnUploaded?.Invoke(false, null);
            }

        }, preferredCamera: NativeCamera.PreferredCamera.Front);
    }

    public void GetProfilePic(string mediaSource, string faceId, Action<bool, ProfileFileUploadModel> OnImageUploaded)
    {
#if UNITY_EDITOR
        string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "Photo.jpeg");

        //this.OnUploaded = OnImageUploaded;

        selectedImagesCount = 1;

        for (int i = 0; i < 1; i++)
        {
            UploadProfileImage(_filePath, faceId, mediaSource, OnImageUploaded);
        }

#else

        NativeCamera.Permission permission = NativeCamera.TakePicture((imagePath) => {

            if (imagePath != null)
            {
                mediaList.Clear();

                selectedImagesCount = 1;

                UploadProfileImage(imagePath, faceId, mediaSource, OnImageUploaded);
            }
            else
            {
                OnUploaded?.Invoke(false, null);
            }

        }, preferredCamera: NativeCamera.PreferredCamera.Front);
#endif
    }

    public void PickImages(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
#if UNITY_EDITOR
        string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "Banner+1.png");

        mediaList.Clear();

        this.OnUploaded = OnUploaded;

        selectedImagesCount = 1;

        for (int i = 0; i < 1; i++)
        {
            UploadFile(_filePath, EMediaType.Image, mediaSource);
        }

#elif UNITY_ANDROID
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((imagesPath) =>
        {
            if (imagesPath != null && imagesPath.Length > 0)
            {
                mediaList.Clear();

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
#if UNITY_EDITOR
        string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "AudioFile.mp3");

        mediaList.Clear();

        this.OnUploaded = OnUploaded;

        selectedImagesCount = 1;

        for (int i = 0; i < 1; i++)
        {
            UploadFile(_filePath, EMediaType.Image, mediaSource);
        }

#elif UNITY_ANDROID
        NativeGallery.Permission permission = NativeGallery.GetAudiosFromGallery((audiosPaths) =>
        {
            if (audiosPaths != null && audiosPaths.Length > 0)
            {
                this.OnUploaded = OnUploaded;

                mediaList.Clear();

                selectedImagesCount = audiosPaths.Length;

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
#if UNITY_EDITOR
        string _filePath = Path.Combine(Application.persistentDataPath, APIConstants.TEMP_IMAGES_PATH, "ForBiggerMeltdowns.mp4");

        mediaList.Clear();

        this.OnUploaded = OnUploaded;

        selectedImagesCount = 1;

        for (int i = 0; i < 1; i++)
        {
            UploadFile(_filePath, EMediaType.Video, mediaSource);
        }

#elif UNITY_ANDROID
        NativeGallery.Permission permission = NativeGallery.GetVideosFromGallery((videoPaths) =>
        {
            if (videoPaths != null && videoPaths.Length > 0)
            {
                this.OnUploaded = OnUploaded;

                mediaList.Clear();

                selectedImagesCount = videoPaths.Length;

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

    public void GetDocuments(string mediaSource, Action<bool, List<string>> OnDocumentsUploaded)
    {

#if UNITY_EDITOR

        this.OnUploaded = OnDocumentsUploaded;

        mediaList.Clear();

        selectedImagesCount = 1;

        UploadFile(Path.Combine(Application.persistentDataPath, "Dummy.pdf"), EMediaType.Document, mediaSource);

#else
        NativeFilePicker.Permission permission = NativeFilePicker.PickMultipleFiles((documentPaths) =>
        {
            if (documentPaths != null && documentPaths.Length > 0)
            {
                this.OnUploaded = OnDocumentsUploaded;

                mediaList.Clear();

                selectedImagesCount = documentPaths.Length;

                for (int i = 0; i < documentPaths.Length; i++)
                {
                    UploadFile(documentPaths[i], EMediaType.Document, mediaSource);
                }
            }

        }, new string[] { NativeFilePicker.ConvertExtensionToFileType("pdf") });
#endif
    }

    public void UploadVideoFile(string filePath, string mediaSource, Action<bool, List<string>> OnUploaded)
    {
        this.OnUploaded = OnUploaded;

        selectedImagesCount = 1;

        UploadFile(filePath, EMediaType.Video, mediaSource);
    }

#region Upload File

    void UploadFile(string filePath, EMediaType mediaType, string mediaSource)
    {
        Loader.Instance.StartLoading();

        GameManager.Instance.apiHandler.UploadFile(filePath, mediaType, mediaSource, (status, response) => {

            if (status)
            {
                FileUploadResponseModel responseModel = JsonUtility.FromJson<FileUploadResponseModel>(response);

                uploadedURLs.Add(responseModel.data);

                mediaList.Add(GetMediaModel(responseModel.data, mediaType));

                if (uploadedURLs.Count == selectedImagesCount)
                {
                    OnUploaded?.Invoke(true, uploadedURLs);

                    selectedImagesCount = 0;

                    uploadedURLs.Clear();

                    OnUploaded = null;
                }
                else {
                }
            }
            else
            {
                List<string> responses = new List<string>();

                responses.Add(response);

                OnUploaded?.Invoke(false, responses);

                OnUploaded = null;

                uploadedURLs.Clear();
            }

            Loader.Instance.StopLoading();

        });
    }

    void UploadProfileImage(string filePath, string faceId, string mediaSource, Action<bool, ProfileFileUploadModel> OnImageUploaded)
    {
        Loader.Instance.StartLoading();

        GameManager.Instance.apiHandler.UploadFile(filePath, EMediaType.Image, mediaSource, (status, response) => {

            if (status)
            {
                ProfileUploadResponseModel responseModel = JsonUtility.FromJson<ProfileUploadResponseModel>(response);

                uploadedURLs.Add(responseModel.data.Key);

                mediaList.Add(GetMediaModel(responseModel.data.Key, EMediaType.Image));

                if (uploadedURLs.Count == selectedImagesCount)
                {
                    OnImageUploaded?.Invoke(true, responseModel.data);

                    selectedImagesCount = 0;

                    OnUploaded = null;

                    uploadedURLs.Clear();
                }
            }
            else
            {
                List<string> responses = new List<string>();

                responses.Add(response);

                OnImageUploaded?.Invoke(false, null);

                OnUploaded = null;
            }

            Loader.Instance.StopLoading();

        }, faceId);
    }

    public List<MultimediaModel> GetLoadedFiles()
    {
        return mediaList;
    }

    public void PickImage(string mediaSource, Action<bool, List<string>> OnUploaded)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((imagePath) =>
        {
            if (imagePath != null)
            {
                uploadedURLs.Clear();

                mediaList.Clear();

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
                mediaList.Clear();

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
                mediaList.Clear();

                this.OnUploaded = OnUploaded;

                if (imagePath != null)
                {
                    selectedImagesCount = 1;

                    UploadFile(imagePath, EMediaType.Audio, mediaSource);
                }
            }
        });
    }

    public void ClearData()
    {
        loadingCountText.text = string.Empty;
    }

    MultimediaModel GetMediaModel(string mediaURL, EMediaType mediaType)
    {
        MultimediaModel multimediaModel = new MultimediaModel();

        multimediaModel.content_url = DataManager.Instance.GetMediaKey() + mediaURL;

        multimediaModel.media_type = mediaType.ToString().ToLower();

        return multimediaModel;
    }

#endregion
}

public enum EMediaType
{ 
    Image,
    Audio,
    Video,
    Document
}

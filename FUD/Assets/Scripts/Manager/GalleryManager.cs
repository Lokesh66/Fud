using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    Action<bool, List<string>> OnUploaded;

    private List<string> uploadedURLs = new List<string>();

    string[] loadedFiles;

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

    public void GetImageFromGallaery(Action<bool, List<string>> OnImageUploaded)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((imagesPath) => {
            if (imagesPath != null)
            {
                uploadedURLs.Clear();

                this.OnUploaded = OnImageUploaded;
                selectedImagesCount = 1;
                UploadFile(imagesPath, EMediaType.Image);
            }
            else
            {
                OnUploaded?.Invoke(false, null);
            }
        }, "Select a PNG image");

    }
    public void PickImages(Action<bool, List<string>> OnUploaded, TMP_InputField countText = null, TMP_InputField uploadedCount = null)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((imagesPath) =>
        {
            if (loadedFiles != null && loadedFiles.Length > 0)
            {
                Array.Clear(loadedFiles, 0, loadedFiles.Length);
            }

            //countText.text = imagesPath.Length.ToString();

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
                        UploadFile(imagesPath[i], EMediaType.Image, uploadedCount);
                    }
                }
            }
            else {
                OnUploaded?.Invoke(false, null);
            }
        }, "Select a PNG image");
        AlertMessage.Instance.SetText("Permission result: " + permission);
        Debug.Log("Permission result: " + permission);
    }

    public void GetAudiosFromGallery(Action<bool, List<string>> OnUploaded)
    {
        NativeGallery.Permission permission = NativeGallery.GetAudiosFromGallery((audiosPaths) =>
        {
            if (audiosPaths != null && audiosPaths.Length > 0)
            {
                this.OnUploaded = OnUploaded;

                selectedImagesCount = audiosPaths.Length;

                for (int i = 0; i < audiosPaths.Length; i++)
                {
                    UploadFile(audiosPaths[i], EMediaType.Audio);
                }
            }
        });
    }

    public void GetVideosFromGallery(Action<bool, List<string>> OnUploaded)
    {
        NativeGallery.Permission permission = NativeGallery.GetVideosFromGallery((videoPaths) =>
        {
            if (videoPaths != null && videoPaths.Length > 0)
            {
                this.OnUploaded = OnUploaded;

                selectedImagesCount = videoPaths.Length;

                for (int i = 0; i < videoPaths.Length; i++)
                {
                    UploadFile(videoPaths[i], EMediaType.Video);
                }
            }
        });
    }

    public void UploadVideoFile(string filePath, Action<bool, List<string>> OnUploaded)
    {
        this.OnUploaded = OnUploaded;

        selectedImagesCount = 1;

        UploadFile(filePath, EMediaType.Video);
    }

    public void RecordVideo()
    { 
    
    }

    #region Upload File

    void UploadFile(string filePath, EMediaType mediaType, TMP_InputField uploadedCount = null)
    {
        Loader.Instance.StartLoading();

        GameManager.Instance.apiHandler.UploadFile(filePath, mediaType, (status, response) => {

            if (status)
            {
                FileUploadResponseModel responseModel = JsonUtility.FromJson<FileUploadResponseModel>(response);

                uploadedURLs.Add(responseModel.data.s3_file_path);

                //uploadedCount.text = uploadedURLs.Count.ToString();

                if (uploadedURLs.Count == selectedImagesCount)
                {
                    UpdateLocalData(uploadedURLs, mediaType);

                    OnUploaded?.Invoke(true, uploadedURLs);

                    selectedImagesCount = 0;

                    AlertModel alertModel = new AlertModel();

                    alertModel.message = responseModel.message;

                    alertModel.okayButtonAction = AlertDismissAction;

                    UIManager.Instance.ShowAlert(alertModel);

                    uploadedURLs.Clear();

                    Loader.Instance.StopLoading();
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
            }

            OnUploaded = null;

        });
    }

    void AlertDismissAction()
    {
        UIManager.Instance.gameObject.SetActive(false);
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

    #endregion
}

public enum EMediaType
{ 
    Image,
    Audio,
    Video
}

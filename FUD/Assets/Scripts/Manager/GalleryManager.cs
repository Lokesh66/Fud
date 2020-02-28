using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GalleryManager : MonoBehaviour
{
    Action<bool, List<string>> OnImagesUploaded;

    private List<string> imageUrls = new List<string>();

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

    public void PickImages(Action<bool, List<string>> OnImagesUploaded)
    {
        NativeGallery.Permission permission = NativeGallery.GetImagesFromGallery((imagesPath) =>
        {
            if (imagesPath != null && imagesPath.Length > 0)
            {
                imageUrls.Clear();

                this.OnImagesUploaded = OnImagesUploaded;

                selectedImagesCount = imagesPath.Length;

                for (int i = 0; i < selectedImagesCount; i++)
                {
                    NativeGallery.LoadImageAtPath(imagesPath[i], SystemInfo.maxTextureSize, true, true, false, null);
                }

                if (imagesPath != null && imagesPath.Length > 0)
                {
                    selectedImagesCount = imagesPath.Length;

                    for (int i = 0; i < imagesPath.Length; i++)
                    {
                        /*AlertModel alertModel = new AlertModel();

                        alertModel.message = audiosPaths[i];

                        alertModel.okayButtonAction = AlertDismissAction;

                        CanvasManager.Instance.alertView.ShowAlert(alertModel);*/

                        UploadFile(imagesPath[i], EMediaType.Image);
                    }
                }
            }
            else {
                OnImagesUploaded?.Invoke(false, null);
            }
        }, "Select a PNG image");

        Debug.Log("Permission result: " + permission);
    }

    public void GetAudiosFromGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetAudiosFromGallery((audiosPaths) =>
        {
            if (audiosPaths != null && audiosPaths.Length > 0)
            {
                selectedImagesCount = audiosPaths.Length;

                for (int i = 0; i < audiosPaths.Length; i++)
                {
                    /*AlertModel alertModel = new AlertModel();

                    alertModel.message = audiosPaths[i];

                    alertModel.okayButtonAction = AlertDismissAction;

                    CanvasManager.Instance.alertView.ShowAlert(alertModel);*/

                    UploadFile(audiosPaths[i], EMediaType.Audio);
                }
            }
        });
    }

    public void GetVideosFromGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetVideosFromGallery((videoPaths) =>
        {
            if (videoPaths != null && videoPaths.Length > 0)
            {
                selectedImagesCount = videoPaths.Length;

                for (int i = 0; i < videoPaths.Length; i++)
                {
                    /*AlertModel alertModel = new AlertModel();

                    alertModel.message = videoPaths[i];

                    alertModel.okayButtonAction = AlertDismissAction;

                    CanvasManager.Instance.alertView.ShowAlert(alertModel);*/

                    UploadFile(videoPaths[i], EMediaType.Video);
                }
            }
        });
    }

    #region Upload File

    void UploadFile(string filePath, EMediaType mediaType)
    {
        GameManager.Instance.apiHandler.UploadFile(filePath, mediaType, (status, response) => {

            if (status)
            {
                FileUploadResponseModel responseModel = JsonUtility.FromJson<FileUploadResponseModel>(response);

                imageUrls.Add(responseModel.data.s3_file_path);

                //OnImagesUploaded?.Invoke(true, imageUrls);

                if (imageUrls.Count == selectedImagesCount)
                {
                    UpdateLocalData(imageUrls, mediaType);

                    OnImagesUploaded?.Invoke(true, imageUrls);

                    selectedImagesCount = 0;

                    AlertModel alertModel = new AlertModel();

                    alertModel.message = responseModel.message;

                    alertModel.okayButtonAction = AlertDismissAction;

                    CanvasManager.Instance.alertView.ShowAlert(alertModel);

                    imageUrls.Clear();
                }
                else {
                    List<string> responses = new List<string>();

                    responses.Add("imageUrls.Count, selectedImagesCount are not equal");

                    OnImagesUploaded?.Invoke(false, responses);
                }
            }
            else
            {
                List<string> responses = new List<string>();

                responses.Add(response);

                OnImagesUploaded?.Invoke(false, responses);
            }           

        });
    }

    void AlertDismissAction()
    {
        CanvasManager.Instance.alertView.gameObject.SetActive(false);
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

    #endregion
}

public enum EMediaType
{ 
    Image,
    Audio,
    Video
}

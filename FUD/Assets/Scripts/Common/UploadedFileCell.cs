﻿using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UMP;


public class UploadedFileCell : MonoBehaviour
{
    public RawImage selectedImage;

    public TextMeshProUGUI size1Text;

    public TextMeshProUGUI size2Text;


    public UniversalMediaPlayer mediaPlayer;


    public GameObject pauseObject;

    Sprite selectedSprite;

    //Action<MultimediaModel> OnDeleteAction;


    public MultimediaModel multimediaModel;

    Texture2D imageTexture;

    EMediaType mediaType;

    Action<object> _OnDeleteButtonAction;


    public void Load(MultimediaModel multimediaModel, EMediaType mediaType, Action<object> OnDeleteAction = null)
    {
        this._OnDeleteButtonAction = OnDeleteAction;

        pauseObject.SetActive(mediaType == EMediaType.Video);

        this.mediaType = mediaType;

        this.multimediaModel = multimediaModel;

        if (mediaType == EMediaType.Image)
        {
            SetImageView();
        }
        else if (mediaType == EMediaType.Audio)
        {
            SetAudioView();
        }
        else if (mediaType == EMediaType.Video)
        {
            SetVideoView();
        }
        else if (mediaType == EMediaType.Document)
        {
            SetDocumentView();
        }
    }

    public void OnButtonAction()
    {
        Debug.Log("OnButtonAction : Media Type = " + mediaType + " content_url = " + multimediaModel.content_url);

        if (mediaType == EMediaType.Image)
        {
            UIManager.Instance.ShowBigScreen(multimediaModel.content_url);
        }
        else if (mediaType == EMediaType.Video || mediaType == EMediaType.Audio)
        {
            UIManager.Instance.topCanvas.PlayVideo(selectedImage, mediaPlayer, mediaType);
        }
        else if (mediaType == EMediaType.Document)
        {
            //Open Pdf document with imageURL
            PDFManager.Instance.LoadDocument(multimediaModel.content_url);
        }
    }

    public void OnDeleteButtonAction()
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = "Are you sure, you want to delete this media";

        alertModel.canEnableCancelButton = true;

        alertModel.okayButtonAction = OnDeleteMedia;

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnDeleteMedia()
    {
        _OnDeleteButtonAction?.Invoke(multimediaModel);
    }

    #region Update View

    void SetImageView()
    {
        GameManager.Instance.apiHandler.DownloadImage(multimediaModel.content_url, (sprite) =>
        {
            if (this != null && sprite != null)
            {
                selectedImage.texture = sprite.texture;
            }
        });
    }

    void SetVideoView()
    {
        mediaPlayer.Prepare();

        mediaPlayer.Path = multimediaModel.content_url;

        selectedImage.texture = DataManager.Instance.GetVideoThumbnailSprite().texture;

        mediaPlayer.AddImageReadyEvent((texture) =>
        {
            imageTexture = texture;

            selectedSprite = Sprite.Create(imageTexture, selectedImage.rectTransform.rect, new Vector2(0.5f, 0.5f));

            selectedImage.texture = texture;
        });
    }

    void SetAudioView()
    {
        selectedImage.texture = DataManager.Instance.GetAudioThumbnailSprite().texture;

        mediaPlayer.Prepare();

        mediaPlayer.Path = multimediaModel.content_url;
    }

    void SetDocumentView()
    {
        selectedImage.texture = DataManager.Instance.GetPDFThumbnailSprite().texture;
    }

    #endregion
}

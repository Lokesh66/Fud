using UnityEngine.UI;
using UnityEngine;
using System;
using UMP;


public class PortfolioAlbumCell : MonoBehaviour
{
    public RawImage rawImage;

    public UniversalMediaPlayer mediaPlayer;

    public GameObject pauseObject;


    MultimediaModel albumModel;

    EMediaType mediaType;

    Action<PortfolioAlbumCell> OnButtonAction;
 

    public void SetView(MultimediaModel model, Action<PortfolioAlbumCell> OnButtonAction)
    {
        this.albumModel = model;

        this.OnButtonAction = OnButtonAction;

        SetView();
    }

    void SetView()
    {
        mediaType = DataManager.Instance.GetMediaType(albumModel.media_type);

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

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(this);
    }

    public MultimediaModel GetMediaModel()
    {
        return albumModel;
    }

    #region Update View

    void SetImageView()
    {
        GameManager.Instance.apiHandler.DownloadImage(albumModel.content_url, (sprite) =>
        {
            if (this != null && sprite != null)
            {
                rawImage.texture = sprite.texture;
            }
        });
    }

    void SetVideoView()
    {
        mediaPlayer.Path = albumModel.content_url;

        mediaPlayer.Prepare();

        rawImage.texture = DataManager.Instance.GetVideoThumbnailSprite().texture;

        mediaPlayer.AddImageReadyEvent((texture) =>
        {
            rawImage.texture = texture;
        });
    }

    void SetAudioView()
    {
        rawImage.texture = DataManager.Instance.GetAudioThumbnailSprite().texture;

        mediaPlayer.Prepare();

        mediaPlayer.Path = albumModel.content_url;
    }

    void SetDocumentView()
    {
        rawImage.texture = DataManager.Instance.GetPDFThumbnailSprite().texture;
    }

    #endregion
}

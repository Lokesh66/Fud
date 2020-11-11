using UnityEngine.UI;
using UnityEngine;
using System;
using UMP;


public class CreatedPortfolioMediaCell : MonoBehaviour
{
    public UniversalMediaPlayer mediaPlayer;

    public RawImage albumImage;

    public GameObject pauseObject;

    public GameObject deleteObject;


    MultimediaModel albumModel;

    Action<MultimediaModel> OnDeleteAction;

    EMediaType mediaType;


    public void SetView(MultimediaModel albumModel, Action<MultimediaModel> OnDeleteAction, bool canDelete = false)
    {
        this.albumModel = albumModel;

        this.OnDeleteAction = OnDeleteAction;

        deleteObject.SetActive(canDelete);

        mediaType = DataManager.Instance.GetMediaType(albumModel.media_type);

        pauseObject.SetActive(mediaType == EMediaType.Video);

        if (mediaType == EMediaType.Image)
        {
            GameManager.Instance.apiHandler.DownloadImage(albumModel.content_url, (sprite) =>
            {
                if (this != null && sprite != null)
                {
                    albumImage.texture = sprite.texture;
                }
            });
        }
        else if (mediaType == EMediaType.Video || mediaType == EMediaType.Audio)
        {
            mediaPlayer.Path = albumModel.content_url;

            albumImage.texture = mediaType == EMediaType.Audio ? DataManager.Instance.audioThumbnailSprite.texture : DataManager.Instance.videoThumbnailSprite.texture;

            if (mediaType == EMediaType.Video)
            {
                mediaPlayer.Prepare();

                mediaPlayer.AddImageReadyEvent((texture) =>
                {
                    albumImage.texture = texture;
                });
            }
        }
        else
        {
            albumImage.texture = DataManager.Instance.pdfThumbnailSprite.texture;
        }
    }

    public void OnButtonAction()
    {
        if (mediaType == EMediaType.Image)
        {
            UIManager.Instance.ShowBigScreen(albumModel.content_url);
        }
        else if (mediaType == EMediaType.Audio)
        {
            UIManager.Instance.topCanvas.PlayVideo(albumImage, mediaPlayer, EMediaType.Audio);
        }
        else if (mediaType == EMediaType.Video)
        {
            UIManager.Instance.topCanvas.PlayVideo(albumImage, mediaPlayer);
        }
        else
        {
            PDFManager.Instance.LoadDocument(albumModel.content_url);
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
        OnDeleteAction?.Invoke(albumModel);
    }
}

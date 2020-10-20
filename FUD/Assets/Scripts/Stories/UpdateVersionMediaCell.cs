using UnityEngine;
using UnityEngine.UI;
using System;
using UMP;


public class UpdateVersionMediaCell : MonoBehaviour
{
    public UniversalMediaPlayer mediaPlayer;

    public RawImage albumImage;

    public GameObject pauseObject;

    MultimediaModel albumModel;

    EMediaType mediaType = EMediaType.Image;

    Action<MultimediaModel> OnDeleteAction;


    Sprite selectedSprite;

    Texture2D imageTexture;


    public void SetView(MultimediaModel model, Action<MultimediaModel> OnDeleteAction)
    {
        this.albumModel = model;

        this.OnDeleteAction = OnDeleteAction;

        mediaType = DataManager.Instance.GetMediaType(albumModel.media_type);

        pauseObject.SetActive(mediaType == EMediaType.Video);

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

    #region Update View

    void SetImageView()
    {
        GameManager.Instance.apiHandler.DownloadImage(albumModel.content_url, (sprite) =>
        {
            if (this != null && sprite != null)
            {
                albumImage.texture = sprite.texture;
            }
        });
    }

    void SetVideoView()
    {
        mediaPlayer.Prepare();

        mediaPlayer.Path = albumModel.content_url;

        albumImage.texture = DataManager.Instance.GetVideoThumbnailSprite().texture;

        mediaPlayer.AddImageReadyEvent((texture) =>
        {
            imageTexture = texture;

            selectedSprite = Sprite.Create(imageTexture, albumImage.rectTransform.rect, new Vector2(0.5f, 0.5f));

            albumImage.texture = texture;
        });
    }

    void SetAudioView()
    {
        albumImage.texture = DataManager.Instance.GetAudioThumbnailSprite().texture;

        mediaPlayer.Prepare();

        mediaPlayer.Path = albumModel.content_url;
    }

    void SetDocumentView()
    {
        albumImage.texture = DataManager.Instance.GetPDFThumbnailSprite().texture;
    }

    #endregion
}

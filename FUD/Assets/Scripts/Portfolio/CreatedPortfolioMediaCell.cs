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


    PortfolioAlbumModel albumModel;

    Action<PortfolioAlbumModel> OnDeleteAction;

    EMediaType mediaType;


    public void SetView(PortfolioAlbumModel albumModel, Action<PortfolioAlbumModel> OnDeleteAction, bool canDelete = false)
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
        else if (mediaType == EMediaType.Video)
        {
            mediaPlayer.Path = albumModel.content_url;

            mediaPlayer.Prepare();

            mediaPlayer.AddEndReachedEvent(() =>
            {
                VideoStreamer.Instance.updatedRawImage.gameObject.SetActive(false);
            });

            mediaPlayer.AddImageReadyEvent((texture) =>
            {
                albumImage.texture = texture;
            });
        }
    }

    public void OnButtonAction()
    {
        if (mediaType == EMediaType.Image)
        {
            UIManager.Instance.ShowBigScreen(albumModel.content_url);
        }
        else if (mediaType == EMediaType.Video)
        {
            UIManager.Instance.topCanvas.PlayVideo(albumImage, mediaPlayer);
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

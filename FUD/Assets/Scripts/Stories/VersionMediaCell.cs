using UnityEngine;
using UnityEngine.UI;
using System;
using UMP;


public class VersionMediaCell : MonoBehaviour
{
    public UniversalMediaPlayer mediaPlayer;

    public RawImage albumImage;

    public GameObject pauseObject;

    MultimediaModel albumModel;

    EMediaType mediaType = EMediaType.Image;



    public void SetView(MultimediaModel model)
    {
        this.albumModel = model;

        Debug.Log("url = " + model.content_url);

        mediaType = DataManager.Instance.GetMediaType(albumModel.media_type);

        pauseObject.SetActive(mediaType == EMediaType.Video);

        if (mediaType == EMediaType.Image)
        {
            GameManager.Instance.apiHandler.DownloadImage(model.content_url, (sprite) =>
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

            //GalleryManager.Instance.loadingCountText.text += "\n Story Creation : " + imagePath;

            albumImage.texture = mediaType == EMediaType.Audio ? DataManager.Instance.audioThumbnailSprite.texture : DataManager.Instance.videoThumbnailSprite.texture;

            if (mediaType == EMediaType.Video)
            {
                mediaPlayer.Prepare();

                mediaPlayer.AddImageReadyEvent((texture) =>
                {
                    GalleryManager.Instance.loadingCountText.text += "\n AddImageReadyEvent = texture : " + texture;

                    if (texture != null)
                    {
                        albumImage.texture = texture;
                    }
                });
            }
        }
        else {
            albumImage.texture = DataManager.Instance.pdfThumbnailSprite.texture;
        }
    }

    public void OnButtonAction()
    {
        switch (mediaType)
        {
            case EMediaType.Image:
                OpenBigScreen();
                break;
            case EMediaType.Video:
                PlayVideo();
                break;
            case EMediaType.Audio:
                PlayAudio();
                break;
            case EMediaType.Document:
                OpenDocument();
                break;
        }
    }

    void PlayVideo()
    {
        UIManager.Instance.topCanvas.PlayVideo(albumImage, mediaPlayer);
    }

    void PlayAudio()
    {
        UIManager.Instance.topCanvas.PlayVideo(albumImage, mediaPlayer, EMediaType.Audio);
    }

    void OpenBigScreen()
    {
        UIManager.Instance.ShowBigScreen(albumModel.content_url);
    }

    void OpenDocument()
    {
        PDFManager.Instance.LoadDocument(albumModel.content_url);
    }
}

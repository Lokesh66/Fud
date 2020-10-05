using UnityEngine;
using UnityEngine.UI;
using System;
using UMP;


public class  VersionMediaCell : MonoBehaviour
{
    public UniversalMediaPlayer mediaPlayer;

    public RawImage albumImage;

    public GameObject pauseObject;

    MultimediaModel albumModel;

    EMediaType mediaType = EMediaType.Image;



    public void SetView(MultimediaModel model)
    {
        this.albumModel = model;

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
        else if (mediaType == EMediaType.Video)
        {
            mediaPlayer.Path = albumModel.content_url;

            //GalleryManager.Instance.loadingCountText.text += "\n Story Creation : " + imagePath;

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
        }
    }

    void PlayVideo()
    {
        UIManager.Instance.topCanvas.PlayVideo(albumImage, mediaPlayer);
        //VideoStreamer.Instance.StreamVideo(albumModel.content_url, null);
    }

    void PlayAudio()
    {
        AudioStreamer.Instance.AudioStream(albumModel.content_url, null);
    }

    void OpenBigScreen()
    {
        UIManager.Instance.ShowBigScreen(albumModel.content_url);
    }
}

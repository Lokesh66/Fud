using UnityEngine;
using UnityEngine.UI;
using System;
using UMP;


public class BrowserMediaCell : MonoBehaviour
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
                albumImage.texture = sprite.texture;
            });
        }
    }

    public void OnShareAction()
    {

    }

    public void OnButtonAction()
    {
        //switch (mediaType)
        //{
        //    case EMediaType.Image:
        //        OpenBigScreen();
        //        break;
        //    case EMediaType.Video:
        //        PlayVideo();
        //        break;
        //    case EMediaType.Audio:
        //        PlayAudio();
        //        break;
        //}
    }

    void PlayVideo()
    {
        VideoStreamer.Instance.Play(albumImage, mediaPlayer, EMediaType.Video);
    }

    void PlayAudio()
    {
        VideoStreamer.Instance.Play(albumImage, mediaPlayer, EMediaType.Audio);
    }

    void OpenBigScreen()
    {
        UIManager.Instance.ShowBigScreen(albumModel.content_url);
    }
}

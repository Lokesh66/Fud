using UnityEngine;
using UnityEngine.UI;
using System;


public class  VersionMediaCell : MonoBehaviour
{
    public Image albumImage;

    public RawImage videoIcon;

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
            GameManager.Instance.downLoadManager.DownloadImage(model.content_url, (sprite) =>
            {
                albumImage.sprite = sprite;
            });
        }
    }

    public void SetVideoThumbnail(Action OnNext)
    {
        if (mediaType == EMediaType.Video)
        {
            VideoStreamer.Instance.GetThumbnailImage(albumModel.content_url, (texture) =>
            {
                Rect rect = new Rect(0, 0, albumImage.rectTransform.rect.width, albumImage.rectTransform.rect.height);

                Sprite sprite = Sprite.Create(texture.ToTexture2D(), rect, new Vector2(0.5f, 0.5f));

                albumImage.sprite = sprite;

                OnNext?.Invoke();
            });
        }
        else
        {
            OnNext?.Invoke();
        }
    }

    public void OnShareAction()
    {

    }

    public void OnButtonAction()
    {
        switch (mediaType)
        {
            case EMediaType.Image:
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
        VideoStreamer.Instance.StreamVideo(albumModel.content_url, null);
    }

    void PlayAudio()
    {
        AudioStreamer.Instance.AudioStream(albumModel.content_url, null);
    }
}

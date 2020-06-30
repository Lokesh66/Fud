using UnityEngine;
using UnityEngine.UI;
using System;


public class UpdateVersionMediaCell : MonoBehaviour
{
    public Image albumImage;

    public RawImage videoIcon;

    public GameObject pauseObject;

    MultimediaModel albumModel;

    EMediaType mediaType = EMediaType.Image;

    Action<MultimediaModel> OnDeleteAction;



    public void SetView(MultimediaModel model, Action<MultimediaModel> OnDeleteAction)
    {
        this.albumModel = model;

        this.OnDeleteAction = OnDeleteAction;

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
                if (this != null)
                {
                    Rect rect = new Rect(0, 0, albumImage.rectTransform.rect.width, albumImage.rectTransform.rect.height);

                    Sprite sprite = Sprite.Create(texture.ToTexture2D(), rect, new Vector2(0.5f, 0.5f));

                    albumImage.sprite = sprite;

                    OnNext?.Invoke();
                }
            });
        }
        else
        {
            OnNext?.Invoke();
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

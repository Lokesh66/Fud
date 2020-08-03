using UnityEngine.UI;
using UnityEngine;
using System;

public class CreatedPortfolioMediaCell : MonoBehaviour
{
    public Image albumImage;

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

        if (mediaType == EMediaType.Image)
        {
            GameManager.Instance.downLoadManager.DownloadImage(albumModel.content_url, (sprite) =>
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

    public void OnButtonAction()
    {
        if (mediaType == EMediaType.Image)
        {
            UIManager.Instance.ShowBigScreen(albumModel.content_url);
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

using UnityEngine.UI;
using UnityEngine;
using System;
using System.IO;

public class BrowserSelectedCell : MonoBehaviour
{
    public Image albumImage;


    PortfolioModel portfolioModel;


    public void Load(PortfolioModel albumModel)
    {
        EMediaType mediaType = DataManager.Instance.GetMediaType(albumModel.onScreenModel.media_type);

        portfolioModel = albumModel;

        if (mediaType == EMediaType.Image)
        {
            UpdateMediaImage();

            //Texture2D texture = NativeGallery.LoadImageAtPath(albumModel.onScreenModel.content_url, markTextureNonReadable: false);

            //TextureScale.ThreadedScale(texture, 180, 180, true);

            //albumImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    void UpdateMediaImage()
    {
        GameManager.Instance.downLoadManager.DownloadImage(portfolioModel.onScreenModel.content_url, (sprite) => {

            Texture2D texture = sprite.texture.ToTexture2D();

            String extension = Path.GetExtension(portfolioModel.onScreenModel.content_url).ToLowerInvariant();

            TextureFormat format = (extension == ".jpg" || extension == ".jpeg") ? TextureFormat.RGB24 : TextureFormat.RGBA32;

            Texture2D _texture = new Texture2D(2, 2, format, true, false);

            TextureScale.ThreadedScale(_texture, 180, 180, true);

            albumImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        });
    }
}

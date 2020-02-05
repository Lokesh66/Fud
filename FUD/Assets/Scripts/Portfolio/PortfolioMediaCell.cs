using UnityEngine;
using UnityEngine.UI;

public class PortfolioMediaCell : MonoBehaviour
{
    public Image albumImage;


    PortfolioAlbumModel albumModel;
    public void SetView(PortfolioAlbumModel albumModel)
    {
        this.albumModel = albumModel;

        GameManager.Instance.downLoadManager.DownloadImage(albumModel.content_url, (sprite) => {

            sprite.texture.width = (int)albumImage.rectTransform.rect.width;

            sprite.texture.height = (int)albumImage.rectTransform.rect.height;

            albumImage.sprite = sprite;
        });
    }

    public void OnShareAction()
    { 
    
    }
}

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

           /* sprite.texture.width = 300;

            sprite.texture.height = 400;*/

            albumImage.sprite = sprite;
        });
    }

    public void OnShareAction()
    { 
    
    }
}

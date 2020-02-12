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

            albumImage.sprite = sprite;
        });
    }

    public void OnShareAction()
    { 
    
    }
}

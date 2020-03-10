using UnityEngine.UI;
using UnityEngine;
using System;

public class CreatedPortfolioMediaCell : MonoBehaviour
{
    public Image albumImage;


    public void SetView(string imageURL)
    {
        GameManager.Instance.downLoadManager.DownloadImage(imageURL, (sprite) =>
        {
            albumImage.sprite = sprite;
        });
    }
}

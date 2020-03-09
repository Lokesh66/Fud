using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class UploadedFileCell : MonoBehaviour
{
    public Image selectedImage;

    public TextMeshProUGUI size1Text;

    public TextMeshProUGUI size2Text;


    public void Load(string imagePath, bool isDownloadedImage)
    {
        if (isDownloadedImage)
        {
            GameManager.Instance.downLoadManager.DownloadImage(imagePath, (sprite) => {

                selectedImage.sprite = sprite;
            });
        }
        else {
            Texture2D texture = NativeGallery.LoadImageAtPath(imagePath, markTextureNonReadable: false);

            TextureScale.ThreadedScale(texture, 300, 400, true);

            selectedImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}

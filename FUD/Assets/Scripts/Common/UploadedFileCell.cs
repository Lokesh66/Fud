using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class UploadedFileCell : MonoBehaviour
{
    public Image selectedImage;

    public TextMeshProUGUI size1Text;

    public TextMeshProUGUI size2Text;


    public GameObject pauseObject;


    string imageURL;

    EMediaType mediaType;


    public void Load(string imagePath, bool isDownloadedImage, EMediaType mediaType)
    {
        pauseObject.SetActive(mediaType == EMediaType.Video);

        this.mediaType = mediaType;

        if (isDownloadedImage)
        {
            this.imageURL = imagePath;

            GameManager.Instance.downLoadManager.DownloadImage(imagePath, (sprite) => {

                if (this != null)
                {
                    selectedImage.sprite = sprite;
                }
            });
        }
        else {
            if (mediaType == EMediaType.Image)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(imagePath, markTextureNonReadable: false);

                TextureScale.ThreadedScale(texture, 300, 400, true);

                selectedImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    public void OnButtonAction()
    {
        if (imageURL.IsNOTNullOrEmpty())
        {
            UIManager.Instance.ShowBigScreen(imageURL);
        }
    }

    public void OnDeleteButtonAction()
    {

    }
}

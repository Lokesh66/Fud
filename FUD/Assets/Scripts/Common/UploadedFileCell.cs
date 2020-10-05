using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UMP;


public class UploadedFileCell : MonoBehaviour
{
    public RawImage selectedImage;

    public TextMeshProUGUI size1Text;

    public TextMeshProUGUI size2Text;


    public UniversalMediaPlayer mediaPlayer;


    public GameObject pauseObject;

    Sprite selectedSprite;

    //Action<MultimediaModel> OnDeleteAction;


    string imageURL;

    Texture2D imageTexture;

    EMediaType mediaType;

    Action<object> _OnDeleteButtonAction;


    public void Load(string imagePath, bool isDownloadedImage, EMediaType mediaType, Action<object> OnDeleteAction = null)
    {
        this._OnDeleteButtonAction = OnDeleteAction;

        pauseObject.SetActive(mediaType == EMediaType.Video);

        this.mediaType = mediaType;

        this.imageURL = imagePath;
        
        Debug.Log("imageURL = " + imageURL + " isDownloadedImage = " + isDownloadedImage);

        if (isDownloadedImage)
        {
            GameManager.Instance.apiHandler.DownloadImage(imageURL, (sprite) =>
            {
                if (this != null)
                {
                    selectedImage.texture = sprite.texture;
                }
            });
        }
        else {
            if (mediaType == EMediaType.Image)
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(imagePath, markTextureNonReadable: false);

                TextureScale.ThreadedScale(texture, 300, 400, true);

                selectedImage.texture = texture;

                //selectedImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else if (mediaType == EMediaType.Video)
            {
                //mediaPlayer.Path = imagePath;

                //GalleryManager.Instance.loadingCountText.text += "\n Story Creation : " + imagePath;

                //mediaPlayer.Path = "https://d3d51uhmmm6dej.cloudfront.net/90/cc-stories/1601562494893.mp4";

                mediaPlayer.Prepare();

                mediaPlayer.AddEndReachedEvent(() =>
                {
                    VideoStreamer.Instance.updatedRawImage.gameObject.SetActive(false);

                    Invoke("UpdateVideoTexture", 2.0f);
                });

                mediaPlayer.AddImageReadyEvent((texture) =>
                {
                    imageTexture = texture;

                    selectedSprite = Sprite.Create(imageTexture, selectedImage.rectTransform.rect, new Vector2(0.5f, 0.5f));

                    Debug.Log("AddImageReadyEvent : imageTexture " + imageTexture);

                    selectedImage.texture = texture;
                });
            }
        }
    }

    public void OnButtonAction()
    {
        Debug.Log("OnButtonAction : Media Type = " + mediaType);

        if (mediaType == EMediaType.Image)
        {
            if (imageURL.IsNOTNullOrEmpty())
            {
                UIManager.Instance.ShowBigScreen(imageURL);
            }
        }
        else if (mediaType == EMediaType.Video)
        {
            UIManager.Instance.topCanvas.PlayVideo(selectedImage, mediaPlayer);
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
        _OnDeleteButtonAction?.Invoke(imageURL);
    }

    void UpdateVideoTexture()
    {
        Debug.Log("UpdateVideoTexture : selectedSprite.texture " + selectedSprite.texture);

        selectedImage.texture = selectedSprite.texture;
    }
}

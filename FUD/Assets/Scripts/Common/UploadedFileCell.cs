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


    public void Load(string imageURL, EMediaType mediaType, Action<object> OnDeleteAction = null)
    {
        this._OnDeleteButtonAction = OnDeleteAction;

        pauseObject.SetActive(mediaType == EMediaType.Video);

        this.mediaType = mediaType;

        this.imageURL = imageURL;

        if (mediaType == EMediaType.Image)
        {
            SetImageView();
        }
        else if (mediaType == EMediaType.Video)
        {
            SetVideoView();
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
        else if (mediaType == EMediaType.Document)
        {
            //Open Pdf document with imageURL
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

    #region Update View

    void SetImageView()
    {
        Debug.Log("imageURL = " + imageURL);

        GameManager.Instance.apiHandler.DownloadImage(this.imageURL, (sprite) =>
        {
            if (this != null && sprite != null)
            {
                selectedImage.texture = sprite.texture;
            }
        });
    }

    void SetVideoView()
    {
        mediaPlayer.Prepare();

        mediaPlayer.AddImageReadyEvent((texture) =>
        {
            imageTexture = texture;

            selectedSprite = Sprite.Create(imageTexture, selectedImage.rectTransform.rect, new Vector2(0.5f, 0.5f));

            selectedImage.texture = texture;
        });
    }

    void SetAudioView()
    {
        
    }

    void SetDocumentView()
    {
        
    }

    #endregion
}

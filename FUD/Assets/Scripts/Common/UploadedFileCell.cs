using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using UMP;
using Paroxe.PdfRenderer;

public class UploadedFileCell : MonoBehaviour
{
    public RawImage selectedImage;

    public TextMeshProUGUI size1Text;

    public TextMeshProUGUI size2Text;


    public UniversalMediaPlayer mediaPlayer;


    public GameObject pauseObject;

    private PDFViewer viewer;

    Sprite selectedSprite;

    //Action<MultimediaModel> OnDeleteAction;


    MultimediaModel multimediaModel;

    Texture2D imageTexture;

    EMediaType mediaType;

    Action<object> _OnDeleteButtonAction;


    public void Load(MultimediaModel multimediaModel, EMediaType mediaType, Action<object> OnDeleteAction = null, PDFViewer pdfViewer = null)
    {
        this._OnDeleteButtonAction = OnDeleteAction;

        if (pdfViewer)
            viewer = pdfViewer;

        pauseObject.SetActive(mediaType == EMediaType.Video);

        this.mediaType = mediaType;

        this.multimediaModel = multimediaModel;

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
        Debug.Log("OnButtonAction : Media Type = " + mediaType + " content_url = " + multimediaModel.content_url);

        if (mediaType == EMediaType.Image)
        {
            UIManager.Instance.ShowBigScreen(multimediaModel.content_url);
        }
        else if (mediaType == EMediaType.Video)
        {
            UIManager.Instance.topCanvas.PlayVideo(selectedImage, mediaPlayer);
        }
        else if (mediaType == EMediaType.Document)
        {
            //Open Pdf document with imageURL
            if (!string.IsNullOrEmpty(multimediaModel.content_url))
            {
                viewer.gameObject.SetActive(true);
                viewer.LoadDocumentFromWeb(multimediaModel.content_url);
            }
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
        _OnDeleteButtonAction?.Invoke(multimediaModel);
    }

    #region Update View

    void SetImageView()
    {
        Debug.Log("imageURL = " + multimediaModel);

        GameManager.Instance.apiHandler.DownloadImage(multimediaModel.content_url, (sprite) =>
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

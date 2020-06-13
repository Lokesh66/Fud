using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UpdatePortfolioView : MonoBehaviour
{
    public TMP_InputField titleField;

    public TMP_InputField descriptionField;

    public TMP_Dropdown accessDropDown;

    public RectTransform galleryPanel;

    public UploadedFilesHandler filesHandler;

    public GameObject mediaCell;

    public RectTransform content;


    PortfolioModel portfolioModel;

    bool isShowingGalleryPanel = false;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    List<CreatedPortfolioMediaCell> mediaCells = new List<CreatedPortfolioMediaCell>();

    List<PortfolioAlbumModel> mediaList = new List<PortfolioAlbumModel>();


    public void Load(PortfolioModel portfolioModel)
    {
        gameObject.SetActive(true);

        this.portfolioModel = portfolioModel;

        SetView();
    }

    void SetView()
    {
        titleField.text = portfolioModel.title;

        descriptionField.text = portfolioModel.description;

        accessDropDown.value = portfolioModel.access_modifier;

        mediaList = portfolioModel.PortfolioMedia;

        for (int i = 0; i < mediaList.Count; i++)
        {
            GameObject mediaObject = Instantiate(mediaCell, content);

            CreatedPortfolioMediaCell _mediaCell = mediaObject.GetComponent<CreatedPortfolioMediaCell>();

            _mediaCell.SetView(portfolioModel.PortfolioMedia[i], OnDeleteMediaAction);

            mediaCells.Add(_mediaCell);

            UpdateMediaDict(portfolioModel.PortfolioMedia[i]);
        }
    }

    void UpdateMediaDict(PortfolioAlbumModel model)
    {
        Dictionary<string, object> kvp = new Dictionary<string, object>();

        kvp.Add("content_id", 1);

        kvp.Add("content_url", model.content_url);

        kvp.Add("media_type", model.media_type);

        uploadedDict.Add(kvp);
    }

    void SetVideoThumbnails(int index)
    {
        mediaCells[index].SetVideoThumbnail(() => {

            index++;

            if (index >= portfolioModel.PortfolioMedia.Count)
            {
                return;
            }

            SetVideoThumbnails(index);
        });
    }

    void OnDeleteMediaAction(PortfolioAlbumModel multimediaModel)
    {
        string url = string.Empty;

        bool isItemRemoved = false;

        foreach (var item in uploadedDict)
        {
            Dictionary<string, object> mediaItem = item as Dictionary<string, object>;

            foreach (var kvp in mediaItem)
            {
                if (kvp.Key.Equals("content_url"))
                {
                    url = kvp.Value as string;

                    if (url.Equals(multimediaModel.content_url))
                    {
                        uploadedDict.Remove(mediaItem);

                        isItemRemoved = true;

                        break;
                    }
                }
            }

            if (isItemRemoved)
            {
                break;
            }
        }
    }

    public void OnUploadButtonAction()
    {
        if (!isShowingGalleryPanel)
        {
            ShowGalleryPanel();
        }
    }

    public void OnMediaButtonAction(int mediaType)
    {
        EMediaType selectedType = (EMediaType)mediaType;

        SlideGalleryView(false);

        switch (selectedType)
        {
            case EMediaType.Image:
                GalleryManager.Instance.PickImages(OnImagesUploaded);
                break;
            case EMediaType.Audio:
                GalleryManager.Instance.GetAudiosFromGallery(OnAudiosUploaded);
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery(OnVideosUploaded);
                break;
        }
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            PortfolioAlbumModel albumModel;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false);

            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new PortfolioAlbumModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

                albumModel.content_url = imageUrls[i];

                albumModel.media_type = "image";

                mediaList.Add(albumModel);

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            PortfolioAlbumModel albumModel;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false, EMediaType.Audio);

            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new PortfolioAlbumModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", audioUrls[i]);

                kvp.Add("media_type", "audio");

                albumModel.content_url = audioUrls[i];

                albumModel.media_type = "audio";

                mediaList.Add(albumModel);

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            PortfolioAlbumModel albumModel;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false, EMediaType.Video);

            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new PortfolioAlbumModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "video");

                albumModel.content_url = videoUrls[i];

                albumModel.media_type = "audio";

                mediaList.Add(albumModel);

                uploadedDict.Add(kvp);
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    public void OnSaveButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        GameManager.Instance.apiHandler.UpdatePortfolio(titleField.text, descriptionField.text, portfolioModel.id, accessDropDown.value, uploadedDict, (status, response) => {

            if (status)
            {
                
            }
            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Update portfolio Success" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        OnBackButtonAction();

        uploadedDict.Clear();
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(titleField.text))
        {
            errorMessage = "Title should not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Description should not be empty";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            UIManager.Instance.ShowAlert(alertModel);
            return false;
        }

        return true;
    }

    public void OnCancelAction()
    {
        SlideGalleryView(false);
    }

    void ShowGalleryPanel()
    {
        SlideGalleryView(true);
    }

    void SlideGalleryView(bool canShow)
    {
        isShowingGalleryPanel = canShow;

        float panelPosition = galleryPanel.anchoredPosition.y;

        float targetPostion = panelPosition += canShow ? galleryPanel.rect.height : -galleryPanel.rect.height;

        galleryPanel.DOAnchorPosY(targetPostion, 0.4f);
    }

    public void OnBackButtonAction()
    {
        if (isShowingGalleryPanel)
        {
            SlideGalleryView(false);
        }

        gameObject.SetActive(false);
    }
}

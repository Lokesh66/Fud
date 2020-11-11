using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


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

    List<MultimediaModel> mediaList = new List<MultimediaModel>();

    List<int> deletedMedia = new List<int>();


    private string mediaSource = "portfolio";


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
        }
    }

    void OnDeleteMediaAction(MultimediaModel multimediaModel)
    {
        string url = string.Empty;

        bool isItemRemoved = false;

        int modelIndex = mediaList.IndexOf(multimediaModel);

        Destroy(content.GetChild(modelIndex).gameObject);

        if (multimediaModel.id != -1)
        {
            deletedMedia.Add(multimediaModel.id);
        }
        else
        {
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

        mediaList.Remove(multimediaModel);
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
                GalleryManager.Instance.PickImages(mediaSource, OnImagesUploaded);
                break;
            case EMediaType.Audio:
                GalleryManager.Instance.GetAudiosFromGallery(mediaSource, OnAudiosUploaded);
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery(mediaSource, OnVideosUploaded);
                break;
        }
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            MultimediaModel albumModel;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles());

            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

                albumModel.content_url = imageUrls[i];

                albumModel.media_type = "image";

                mediaList.Add(albumModel);

                uploadedDict.Add(kvp);
            }
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            MultimediaModel albumModel;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Audio);

            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", audioUrls[i]);

                kvp.Add("media_type", "audio");

                albumModel.content_url = audioUrls[i];

                albumModel.media_type = "audio";

                mediaList.Add(albumModel);

                uploadedDict.Add(kvp);
            }
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            MultimediaModel albumModel;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), EMediaType.Video);

            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                albumModel = new MultimediaModel();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "video");

                albumModel.content_url = videoUrls[i];

                albumModel.media_type = "video";

                mediaList.Add(albumModel);

                uploadedDict.Add(kvp);
            }
        }
    }

    public void OnSaveButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        int accessStatus = accessDropDown.value == 0 ? 2 : 1;

        GameManager.Instance.apiHandler.UpdatePortfolio(titleField.text, descriptionField.text, portfolioModel.id, accessStatus, uploadedDict, deletedMedia, (status, response) => {

            OnAPIResponse(status, response);
        });
    }

    void OnAPIResponse(bool status, string apiResponse)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(apiResponse);

        alertModel.message = status ? "Update portfolio Success" : responseModel.message;

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

        panelPosition += canShow ? galleryPanel.rect.height : -galleryPanel.rect.height;

        galleryPanel.DOAnchorPosY(panelPosition, 0.4f);
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

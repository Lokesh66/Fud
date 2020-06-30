using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System;

public class CreateCharacterView : MonoBehaviour
{
    public RectTransform searchContent;

    public TMP_InputField castField;

    public TMP_InputField titleField;

    public TMP_Dropdown genderDropDown;

    public TMP_InputField suitableField;

    public TMP_InputField descriptionField;

    public GameObject searchCell;

    public GameObject scrollObject;

    public RectTransform galleryPanel;

    public UploadedFilesHandler filesHandler;


    StoryDetailsModel detailsModel;

    UserSearchModel selectedModel = null;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;

    string apiResponse = string.Empty;

    bool isShowingGalleryPanel = false;

    List<string> imageUrls;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    Action<StoryCharacterModel> OnCreateCharacter;


    public void SetView(StoryDetailsModel detailsModel, Action<StoryCharacterModel> action)
    {
        this.detailsModel = detailsModel;

        OnCreateCharacter = action;
    }

    void PopulateDropdown(List<UserSearchModel> searchModels)
    {
        searchContent.DestroyChildrens();

        GameObject cellObject = null;

        if (searchModels.Count > 0)
        {
            scrollObject.SetActive(true);

            for (int i = 0; i < searchModels.Count; i++)
            {
                cellObject = Instantiate(searchCell, searchContent);

                cellObject.GetComponent<UserSearchCell>().SetView(searchModels[i], OnSelectMember);
            }
        }
    }

    public void OnValueChange()
    {
        if (selectedModel == null)
        {
            if (suitableField.text.Length > 2 && !isSearchAPICalled)
            {
                //Call Search API
                isSearchAPICalled = true;

                keyword = suitableField.text;

                GetSearchedUsers();
            }
            else {
                searchContent.DestroyChildrens();
            }
        }
        else
        {
            if (!suitableField.text.Equals(selectedModel.name))
            {
                selectedModel = null;

                OnValueChange();
            }
        }
    }

    public void OnCastButtonAction()
    {
        castField.Select();
    }

    public void OnTitleButtonAction()
    {
        titleField.Select();
    }

    public void OnGenderButtonAction()
    {
        genderDropDown.Select();
    }

    public void OnCharacterButtonAction()
    {
        suitableField.Select();
    }

    public void OnDescriptionButtonAction()
    {
        descriptionField.Select();
    }

    public void OnButtonAction()
    {
        if (!CanCallAPI())
        {
            return;
        }

        if (selectedModel == null)
        {
            UserSearchModel searchModel = new UserSearchModel();

            searchModel.id = 0;

            selectedModel = searchModel;
        }

        GameManager.Instance.apiHandler.CreateCharacter(detailsModel.id, titleField.text, castField.text, descriptionField.text, genderDropDown.captionText.text, selectedModel.id, uploadedDict, (status, response) => {

            if (status)
            {
                apiResponse = response;
            }

            OnAPIResponse(status);
        });
    }

    public void OnCancelButtonAction()
    {
        SlideGalleryView(false);
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

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Character Creation Success" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        UpdatedCharaterModel responseModel = JsonUtility.FromJson<UpdatedCharaterModel>(apiResponse);

        StoryCharacterModel characterModel = responseModel.data;

        OnCreateCharacter?.Invoke(characterModel);

        Destroy(gameObject);

        apiResponse = string.Empty;
    }

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(castField.text))
        {
            errorMessage = "Character name should not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Story description should not be empty";
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

    void OnSelectMember(object _selectedModel)
    {
        this.selectedModel = _selectedModel as UserSearchModel;

        suitableField.text = selectedModel.name;

        searchContent.DestroyChildrens();

        scrollObject.SetActive(false);
    }

    void GetSearchedUsers()
    {
        GameManager.Instance.apiHandler.SearchTeamMember(keyword, (status, response) =>
        {
            if (status)
            {
                UserSearchResponse searchResponse = JsonUtility.FromJson<UserSearchResponse>(response);

                PopulateDropdown(searchResponse.data);

                isSearchAPICalled = false;
            }
        });
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            this.imageUrls = imageUrls;

            filesHandler.Load(GalleryManager.Instance.GetLoadedFiles(), false);

            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

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
            this.imageUrls = audioUrls;

            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", audioUrls[i]);

                kvp.Add("media_type", "audio");

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

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            this.imageUrls = videoUrls;

            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "video");

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

    public void OnUploadAction()
    {
        ShowGalleryPanel();
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
}

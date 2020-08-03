using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using System.Collections;

public class UpdateCharacterView : MonoBehaviour
{
    public RectTransform searchContent;

    public TMP_Dropdown castDropdown;

    public TMP_InputField titleField;

    public TMP_Dropdown genderDropdown;

    public TMP_InputField suitableField;

    public TMP_InputField descriptionField;

    public TMP_Dropdown craftDropdown;

    public TMP_Dropdown roleCatageryDropdown;

    public GameObject searchCell;

    public GameObject scrollObject;

    public UploadedFilesHandler filesHandler;

    public GameObject mediaCell;

    public RectTransform galleryPanel;


    StoryDetailsModel detailsModel;

    UserSearchModel selectedModel = null;

    string keyword = string.Empty;

    bool isSearchAPICalled = false;



    StoryCharacterModel characterModel;

    string suitable_performer = string.Empty;

    string apiResponse = string.Empty;

    bool isShowingGalleryPanel = false;

    List<string> imageUrls;

    List<Craft> craftRoles;

    Craft selectedCraft;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    Action<StoryCharacterModel> OnUpdateCharacter;

    List<RoleCategeryModel> categeryModels;

    private string mediaSource = "characters";


    public void Load(StoryCharacterModel characterModel, Action<StoryCharacterModel> OnUpdateCharacter)
    {
        gameObject.SetActive(true);

        this.characterModel = characterModel;

        this.OnUpdateCharacter = OnUpdateCharacter;

        StartCoroutine(UpdateView());

        PopulateRoleDropdown();
    }

    IEnumerator UpdateView()
    {
        yield return new WaitForEndOfFrame();

        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.GetOtherUserInfo(characterModel.id, storyId, (status, response) => {

            if (status)
            {
                PerformerResponse reponseModel = JsonUtility.FromJson<PerformerResponse>(response);

                suitable_performer = reponseModel.data.UserInfo.name;

                UserSearchModel userSearchModel = new UserSearchModel();

                userSearchModel.id = reponseModel.data.UserInfo.id;

                userSearchModel.name = reponseModel.data.UserInfo.name;

                selectedModel = userSearchModel;

                suitableField.text = suitable_performer;
            }
        });

        SetView();
    }

    public void SetView()
    {
        titleField.text = characterModel.title;

        genderDropdown.captionText.text = characterModel.gender;

        descriptionField.text = characterModel.description;

        castDropdown.value = characterModel.type;
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

    void PopulateRoleDropdown()
    {
        craftRoles = DataManager.Instance.crafts;

        Craft selectedCraft = craftRoles.Find(item => item.id == characterModel.role_id);

        List<string> options = new List<string>();

        foreach (var option in craftRoles)
        {
            options.Add(option.name);
        }

        craftDropdown.ClearOptions();

        craftDropdown.AddOptions(options);

        craftDropdown.value = craftRoles.IndexOf(selectedCraft);
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
        else {
            if (!suitableField.text.Equals(selectedModel.name))
            {
                selectedModel = null;

                OnValueChange();
            }
        }
    }

    public void OnRoleValueChange()
    {
        selectedCraft = craftRoles[craftDropdown.value];

        GameManager.Instance.apiHandler.GetRoleCategeries(selectedCraft.id, (status, response) => {

            RoleCategeryResponse responseModel = JsonUtility.FromJson<RoleCategeryResponse>(response);

            if (status)
            {
                categeryModels = responseModel.data;

                UpdateRoleCategeryDropdown();
            }
        });
    }

    void UpdateRoleCategeryDropdown()
    {
        List<string> options = new List<string>();

        foreach (var option in categeryModels)
        {
            options.Add(option.name);
        }

        roleCatageryDropdown.ClearOptions();

        roleCatageryDropdown.AddOptions(options);
    }

    public void OnCastButtonAction()
    {
        castDropdown.Select();
    }

    public void OnTitleButtonAction()
    {
        titleField.Select();
    }

    public void OnCraftRoleButtonAction()
    {
        craftDropdown.Select();
    }

    public void OnGenderButtonAction()
    {
        genderDropdown.Select();
    }

    public void OnCharacterButtonAction()
    {
        suitableField.Select();
    }

    public void OnDescriptionButtonAction()
    {
        descriptionField.Select();
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);

        ClearData();
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

        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.UpdateCharacter(characterModel.id, storyId, titleField.text, castDropdown.value, descriptionField.text, selectedModel.id, genderDropdown.captionText.text, (status, response) => {

            if (status)
            {
                apiResponse = response;
            }
            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        UpdatedCharaterModel responseModel = JsonUtility.FromJson<UpdatedCharaterModel>(apiResponse);

        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Character Updation Success" : responseModel.message;

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

        OnUpdateCharacter?.Invoke(characterModel);

        OnBackButtonAction();
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

    bool CanCallAPI()
    {
        string errorMessage = string.Empty;

        if (string.IsNullOrEmpty(titleField.text))
        {
            errorMessage = "Character Title should not be empty";
        }
        else if (string.IsNullOrEmpty(descriptionField.text))
        {
            errorMessage = "Story description should not be empty";
        }
        else if (string.IsNullOrEmpty(suitableField.text))
        {
            errorMessage = "Please Select Suitable Character";
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

    void ClearData()
    {
        searchContent.DestroyChildrens();

        suitableField.text = string.Empty;

        descriptionField.text = string.Empty;

        apiResponse = keyword = string.Empty;

        castDropdown.value = genderDropdown.value = 0;

        isSearchAPICalled = false;
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

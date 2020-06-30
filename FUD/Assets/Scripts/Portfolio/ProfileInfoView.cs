using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileInfoView : MonoBehaviour
{
    public enum EProofType
    {
        IdProof = 0,
        FrontAddress,
        BackAddress,
    }

    public Image profileImage;

    public TMP_InputField nameField;

    public TMP_Text dobText;

    public TMP_InputField mailField;

    public TMP_InputField contactField;

    public TMP_Dropdown roleDropDown;


    public Image idProof;

    public Image frontAddressProof;

    public Image backAddressProof;



    private string defaultDobText = "Date of Birth";

    DateTime dateOfBirth;

    UserData data;

    Action<bool> OnCloseAction;

    List<Genre> genres;

    EProofType idProofType;

    string idProofURL = string.Empty;

    string frontProofURL = string.Empty;

    string backProofURL = string.Empty;

    string profileImageURL = string.Empty;

    bool isUserDataUpdated = false;


    public void Load(Action<bool> action)
    {
        isUserDataUpdated = false;
        OnCloseAction = action;
        SetView();
        dateOfBirth = DateTime.Now;

        gameObject.SetActive(true);
    }

    void SetView()
    {
        data = DataManager.Instance.userInfo;

        if (data != null)
        { 
            nameField.text = data.name;
            mailField.text = data.email_id;
            //memberIdField.text = data.maa_membership_id;
            contactField.text = data.phone.ToString();

            if (string.IsNullOrEmpty(data.dob))
            {
                dobText.text = defaultDobText;
                dobText.color = Color.grey;
            }
            else
            {
                dobText.text = data.dob;
                dobText.color = Color.white;
                string[] date = dobText.text.Split('-');
                dateOfBirth = DateTime.MinValue;
                dateOfBirth.AddYears(Convert.ToInt16(date[0]));
                dateOfBirth.AddMonths(Convert.ToInt16(date[1]));
                dateOfBirth.AddDays(Convert.ToInt16(date[2]));
            }

            PopulateDropdown();
        }
    }

    void PopulateDropdown()
    {
        genres = DataManager.Instance.genres;

        Genre requiredGenre = genres.Find(genre => genre.id == data.role_id);

        List<string> options = new List<string>();

        foreach (var option in genres)
        {
            options.Add(option.name);
        }

        roleDropDown.ClearOptions();

        roleDropDown.AddOptions(options);

        roleDropDown.value = genres.IndexOf(requiredGenre);
    }

    public void OnProofButtonAction(int isIdProof)
    {
        this.idProofType = (EProofType)isIdProof;

        GalleryManager.Instance.PickImage(OnImagesUploaded);
    }

    public void OnProfileImageAction()
    {
        GalleryManager.Instance.PickImage(OnProfileImageUploaded);
    }

    public void DateOfBirthButtonAction()
    {
        DatePicker.Instance.GetDate(dateOfBirth, DateTime.MinValue, DateTime.Now, (date, dateString) => {
            if (!string.IsNullOrEmpty(dateString))
            {
                dobText.text = dateString;
                dobText.color = Color.white;
                dateOfBirth = date;
            }
            else
            {
                dobText.text = defaultDobText;
                dobText.color = Color.grey;
            }
        });
    }

    public void OnSaveButtonAction()
    {   
        string errorMessage = string.Empty;
        if (string.IsNullOrEmpty(nameField.text)){
            errorMessage = "Name should not be empty";
        }
        else if (string.IsNullOrEmpty(mailField.text)){
            errorMessage = "Mail should not be empty";
        }
        else if (string.IsNullOrEmpty(dobText.text)){
            errorMessage = "Date of Birth should not be empty";
        }
        else if (string.IsNullOrEmpty(contactField.text)){
            errorMessage = "Current Location should not be empty";
        }


        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            UIManager.Instance.ShowAlert(alertModel);
            return;
        }

        string selectedGenreText = roleDropDown.options[roleDropDown.value].text;

        Genre selectedGenre = genres.Find(genre => genre.name.Equals(selectedGenreText));

        ProfileInfoModel infoModel = new ProfileInfoModel();

        infoModel.name = nameField.text;
        infoModel.mail = mailField.text;
        infoModel.dob = dobText.text;
        infoModel.contactNumber = contactField.text;
        infoModel.currentLocation = data.current_location;
        infoModel.nativeLocation = data.native_location;
        infoModel.roleId = selectedGenre.id;
        infoModel.profile_image = profileImageURL;


        GameManager.Instance.apiHandler.UpdateProfileInfo(infoModel, idProofURL, frontProofURL, backProofURL, (status, model) => {
            AlertModel alertModel = new AlertModel();
            if (status)
            {
                isUserDataUpdated = true;
                if(model != null)
                {
                    DataManager.Instance.UpdateUserInfo(model);
                }
                alertModel.message = "User data updated successfully";
                alertModel.canEnableTick = true;
                alertModel.okayButtonAction = OnBackButtonAction;
            }
            else
            {
                alertModel.message = "User data updated failed";
            }
            UIManager.Instance.ShowAlert(alertModel);
        });
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
        idProofURL = frontProofURL = backProofURL = string.Empty;
        OnCloseAction?.Invoke(isUserDataUpdated);
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            string[] imagePaths = GalleryManager.Instance.GetLoadedFiles();

            switch (idProofType)
            {
                case EProofType.IdProof:
                    idProofURL = imageUrls[0];
                    break;
                case EProofType.FrontAddress:
                    frontProofURL = imageUrls[0];
                    break;
                case EProofType.BackAddress:
                    backProofURL = imageUrls[0];
                    break;
            }

            UpdateProofImage(idProofType, imagePaths[0]);
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnProfileImageUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            string[] imagePaths = GalleryManager.Instance.GetLoadedFiles();

            profileImageURL = imageUrls[0];

            UpdateProfileImage(imagePaths[0]);
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void UpdateProfileImage(string imagePath)
    {
        Texture2D texture = NativeGallery.LoadImageAtPath(imagePath, markTextureNonReadable: false);

        TextureScale.ThreadedScale(texture, 250, 250, true);

        profileImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    void UpdateProofImage(EProofType proofType, string imagePath)
    {
        Texture2D texture = NativeGallery.LoadImageAtPath(imagePath, markTextureNonReadable: false);

        TextureScale.ThreadedScale(texture, 200, 200, true);

        switch (proofType)
        {
            case EProofType.IdProof:
                idProof.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                idProof.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case EProofType.FrontAddress:
                frontAddressProof.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                frontAddressProof.transform.GetChild(0).gameObject.SetActive(false);
                break;

            case EProofType.BackAddress:
                backAddressProof.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                backAddressProof.transform.GetChild(0).gameObject.SetActive(false);
                break;
        }
    }
}

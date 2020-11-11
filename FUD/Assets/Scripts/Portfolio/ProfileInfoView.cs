using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using System;
using TMPro;


public class ProfileInfoView : MonoBehaviour
{
    public enum EProofType
    {
        IdProof = 0,
        FrontAddress,
        BackAddress,
    }

    public Image profileImage;

    public TMP_Text aadherNumberText;

    public TMP_InputField nameField;

    public TMP_Text dobText;

    public TMP_InputField mailField;

    public TMP_InputField contactField;

    public TMP_InputField aadherField;

    public TMP_Dropdown craftDropdown;

    public TMP_Dropdown roleCatageryDropdown;


    public Image idProof;

    public Image frontAddressProof;

    public Image backAddressProof;


    public GameObject frontAddObject;

    public GameObject backAddObject;


    public Sprite addressBGSprite;


    private string defaultDobText = "Date of Birth";

    private string mediaSource = "profile";

    DateTime dateOfBirth;

    UserData data;

    Action<bool> OnCloseAction;

    List<Craft> craftRoles;

    List<RoleCategeryModel> categeryModels;

    Craft selectedCraft;

    ProfileFileUploadModel profilePicUploadModel;

    EProofType idProofType;

    int currentAadherCount = 0;

    string idProofURL = string.Empty;

    string frontProofURL = string.Empty;

    string backProofURL = string.Empty;

    string aadherString = string.Empty;

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

            SetImage();

            UpdateAddressProofs();
        }
    }

    void PopulateDropdown()
    {
        craftRoles = DataManager.Instance.crafts;

        Craft requiredGenre = craftRoles.Find(genre => genre.id == data.role_id);

        List<string> options = new List<string>();

        foreach (var option in craftRoles)
        {
            options.Add(option.name);
        }

        craftDropdown.ClearOptions();

        craftDropdown.AddOptions(options);

        craftDropdown.value = craftRoles.IndexOf(requiredGenre);

        OnRoleValueChange();

        if (data.role_category_name.IsNOTNullOrEmpty())
        {
            roleCatageryDropdown.captionText.text = data.role_category_name;
        }
    }

    void SetImage()
    {
        if (data.profile_image.IsNOTNullOrEmpty())
        {
            GameManager.Instance.apiHandler.DownloadImage(data.profile_image, sprite => {

                profileImage.sprite = sprite;
            });
        }
    }

    void UpdateAddressProofs()
    {
        GameManager.Instance.apiHandler.DownloadImage(data.add_proof_front, (sprite) =>
        {
            frontAddressProof.sprite = sprite != null ? sprite : addressBGSprite;

            frontAddObject.SetActive(sprite == null);
        });

        GameManager.Instance.apiHandler.DownloadImage(data.add_proof_back, (sprite) =>
        {
            backAddressProof.sprite = sprite != null ? sprite : addressBGSprite;

            backAddObject.SetActive(sprite == null);
        });
    }

    public void OnNameSelection()
    {
        nameField.Select();
    }

    public void OnMailSelection()
    {
        mailField.Select();
    }

    public void OnContactSelection()
    {
        contactField.Select();
    }

    public void OnAadherSelection()
    {
        aadherField.Select();
    }

    public void OnProofButtonAction(int isIdProof)
    {
        this.idProofType = (EProofType)isIdProof;

        string mediaSource = idProofType == EProofType.IdProof ? "Id_Proof" : idProofType == EProofType.FrontAddress ? "Front_Address" : "Back_Address";

        GalleryManager.Instance.PickImage(mediaSource, OnImagesUploaded);
    }

    public void OnProfileImageAction()
    {
        string faceId = data.face_id;

        GalleryManager.Instance.GetProfilePic(mediaSource, faceId, OnProfileImageUploaded);
    }

    public void OnAadherEndEdit()
    {
        if (aadherString.Length <= 0 && aadherField.text.Length <= 0)
        {
            return;
        }

        if (currentAadherCount > aadherField.text.Length)
        {
            int remainderValue = aadherField.text.Length % 4;

            string appendstring = remainderValue == 3 ? aadherString.Substring(aadherString.Length - 2) : aadherString[aadherString.Length - 1].ToString();

            aadherString = aadherString.Remove(aadherString.Length - appendstring.Length);
        }
        else {

            int remainderValue = aadherField.text.Length % 4;

            string appendstring = remainderValue == 0 ? " " : string.Empty;

            if (remainderValue == 0)
            {
                aadherString += aadherField.text[aadherField.text.Length - 1] + appendstring;
            }
            else
            {
                int spaceCount = aadherString.Count(ch => ch == ' ');

                aadherString += aadherField.text.Substring(aadherString.Length - spaceCount);
            }
        }

        currentAadherCount = aadherField.text.Length;

        aadherNumberText.text = aadherString;
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
            errorMessage = "Contact Number should not be empty";
        }


        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            UIManager.Instance.ShowAlert(alertModel);
            return;
        }

        string selectedGenreText = craftDropdown.options[craftDropdown.value].text;

        Craft selectedGenre = craftRoles.Find(genre => genre.name.Equals(selectedGenreText));

        ProfileInfoModel infoModel = new ProfileInfoModel();

        infoModel.name = nameField.text;
        infoModel.mail = mailField.text;
        infoModel.dob = dobText.text;
        infoModel.contactNumber = contactField.text;
        infoModel.currentLocation = data.current_location;
        infoModel.nativeLocation = data.native_location;
        infoModel.roleId = selectedGenre.id;
        infoModel.categeryId = categeryModels[roleCatageryDropdown.value].id;
        infoModel.aadherNumber = aadherField.text.Replace(" ", string.Empty);

        if (profilePicUploadModel != null)
        {
            infoModel.profile_image = profilePicUploadModel.Key;
            infoModel.faceId = profilePicUploadModel.faceId;
            infoModel.isCeleb = profilePicUploadModel.isCeleb;
        }

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
        profilePicUploadModel = null;
        OnCloseAction?.Invoke(isUserDataUpdated);
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            List<MultimediaModel> modelsList = GalleryManager.Instance.GetLoadedFiles();

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

            UpdateProofImage(idProofType, modelsList[0]);
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString() + imageUrls[0];

            UIManager.Instance.ShowAlert(alertModel);
        }
    }

    void OnProfileImageUploaded(bool status, ProfileFileUploadModel fileUploadModel)
    {
        List<MultimediaModel> modelsList = GalleryManager.Instance.GetLoadedFiles();

        if (status)
        {
            profilePicUploadModel = fileUploadModel;

            UpdateProfileImage(modelsList[0]);
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

    void UpdateProfileImage(MultimediaModel model)
    {
        GameManager.Instance.apiHandler.DownloadImage(model.content_url, (sprite) => {

            if (sprite != null)
            {
                profileImage.sprite = sprite;
            }

        });
    }

    void UpdateProofImage(EProofType proofType, MultimediaModel model)
    {
        GameManager.Instance.apiHandler.DownloadImage(model.content_url, (sprite) =>
        {
            switch (proofType)
            {
                case EProofType.IdProof:
                    idProof.sprite = sprite;
                    idProof.transform.GetChild(0).gameObject.SetActive(false);
                    break;

                case EProofType.FrontAddress:
                    frontAddressProof.sprite = sprite;
                    frontAddressProof.transform.GetChild(0).gameObject.SetActive(false);
                    break;

                case EProofType.BackAddress:
                    backAddressProof.sprite = sprite;
                    backAddressProof.transform.GetChild(0).gameObject.SetActive(false);
                    break;
            }
        });
    }
}

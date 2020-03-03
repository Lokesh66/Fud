using System;
using UnityEngine;
using TMPro;

public class ProfileInfoView : MonoBehaviour
{
    public TMP_InputField nameField;

    public TMP_Text dobText;

    public TMP_InputField mailField;

    public TMP_InputField memberIdField;

    public TMP_InputField currentLocationField;

    public TMP_InputField nativeLocationField;

    private string defaultDobText = "Date of Birth";

    DateTime dateOfBirth;

    public void Load()
    {
        SetView();

        gameObject.SetActive(true);
    }

    void SetView()
    {
        UserData data = DataManager.Instance.userInfo;
        if (data != null)
        { 
            nameField.text = data.name;
            mailField.text = data.email_id;
            memberIdField.text = data.maa_membership_id;
            currentLocationField.text = data.current_location;
            nativeLocationField.text = data.native_location;
            if (string.IsNullOrEmpty(data.dob))
            {
                dobText.text = defaultDobText;
                dobText.color = Color.grey;
            }
            else
            {
                dobText.text = data.dob;
                dobText.color = Color.white;
            }
        }
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
        else if (string.IsNullOrEmpty(currentLocationField.text)){
            errorMessage = "Current Location should not be empty";
        }
        else if (string.IsNullOrEmpty(nativeLocationField.text)){
            errorMessage = "Native Location should not be empty";
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            AlertModel alertModel = new AlertModel();
            alertModel.message = errorMessage;
            CanvasManager.Instance.alertView.ShowAlert(alertModel);
            return;
        }

        ProfileInfoModel infoModel = new ProfileInfoModel();

        infoModel.name = nameField.text;
        infoModel.mail = mailField.text;
        infoModel.dob = dobText.text;
        infoModel.memberId = memberIdField.text;
        infoModel.currentLocation = currentLocationField.text;
        infoModel.nativeLocation = nativeLocationField.text;

        GameManager.Instance.apiHandler.UpdateProfileInfo(infoModel, (status, model) => {
            AlertModel alertModel = new AlertModel();
            if (status)
            {
                alertModel.message = "User data updated successfully";
                alertModel.okayButtonAction = OnBackButtonAction;
                if(model != null)
                {
                    DataManager.Instance.UpdateUserInfo(model);
                }
            }
            else
            {
                alertModel.message = "User data updated failed";
            }
            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        });
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

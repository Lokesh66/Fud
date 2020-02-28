using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileInfoView : MonoBehaviour
{
    public TMP_InputField nameField;

    public TMP_Dropdown ageDropDown;

    public TMP_InputField mailField;

    public TMP_InputField numberField;

    public TMP_Dropdown roleField;


    public void Load()
    {
        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    { 
    
    }

    public void OnSaveButtonAction()
    {
        ProfileInfoModel infoModel = new ProfileInfoModel();

        infoModel.name = nameField.text;

        infoModel.mail = mailField.text;

        infoModel.number = numberField.text;

        //infoModel.actor 

        //GameManager.Instance.apiHandler.UpdateProfileInfo(infoModel);
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

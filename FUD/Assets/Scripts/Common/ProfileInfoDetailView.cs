using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ProfileInfoDetailView : MonoBehaviour
{
    public Image profileIcon;

    public TextMeshProUGUI userNameText;

    public TextMeshProUGUI roleText;

    public TextMeshProUGUI subRoleText;

    public TextMeshProUGUI phoneNumberText;

    public TextMeshProUGUI experienceText;


    public void Load(UserData profileData)
    {
        gameObject.SetActive(true);

        SetView(profileData);
    }

    void SetView(UserData profileData)
    {
        GameManager.Instance.apiHandler.DownloadImage(profileData.profile_image, (sprite) =>
        {
            if (sprite != null)
            {
                profileIcon.sprite = sprite;
            }
        });

        userNameText.text = profileData.name;

        roleText.text = profileData.role_name;

        subRoleText.text = profileData.role_category_name;

        phoneNumberText.text = profileData.phone.ToString();

        experienceText.text = profileData.WorkExperience.Count > 0 ? profileData.WorkExperience[0].description : string.Empty;
    }

    public void OnCloseButtonAction()
    {
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;

public class LeftMenu : MonoBehaviour
{
    public SettingsPanel settingsPanel;
    public MyProfilePanel profilePanel;
    public SubscriptionController subscriptionController;

    public Image profileIcon;
    public TMP_Text nameText;

    System.Action OnCloseAction;

    UserData userData;

    public void SetView(System.Action OnClose)
    {
        gameObject.SetActive(true);

        userData = DataManager.Instance.userInfo;

        nameText.text = userData.name;

        SetProfileIcon();

        OnCloseAction = OnClose;
    }

    void SetProfileIcon()
    {
        if (userData.profile_image.IsNOTNullOrEmpty())
        {
            DownLoadManager downLoadManager = GameManager.Instance.downLoadManager;

            Texture2D texture = downLoadManager.GetLocalTexture(userData.profile_image);

            if (texture == null)
            {
                downLoadManager.DownloadImage(userData.profile_image, sprite => {

                    profileIcon.sprite = sprite;
                });
            }
        }
    }

    public void BackButtonAction()
    {
        gameObject.SetActive(false);
        OnCloseAction?.Invoke();
        OnCloseAction = null;
    }

    public void SettingsButtonAction()
    {
        settingsPanel.SetView();
    }

    public void ProfileButtonAction()
    {
        profilePanel.SetView();
    }

    public void OnBasicInfoButtonAction()
    { 
    
    }

    public void OnSubscrptionButtonAction()
    {
        subscriptionController.EnableView();
    }

    public void OnLogoutButtonAction()
    {
        GameManager.Instance.apiHandler.Logout((status, response) => {

            if (status)
            {
                System.IO.File.Delete(APIConstants.TOKEN_PATH);

                GameManager.Instance.sceneController.SwitchScene(ESceneType.LoginScene);
            }
        });
    }
}

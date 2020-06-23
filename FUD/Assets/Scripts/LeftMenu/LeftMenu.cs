using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeftMenu : MonoBehaviour
{
    public SettingsPanel settingsPanel;
    public MyProfilePanel profilePanel;
    public SubscriptionController subscriptionController;

    public Image profileIcon;
    public TMP_Text nameText;

    System.Action OnCloseAction;
    public void SetView(System.Action OnClose)
    {
        gameObject.SetActive(true);

        nameText.text = DataManager.Instance.userInfo.name;

        OnCloseAction = OnClose;
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

using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LeftMenu : MonoBehaviour
{
    public SettingsPanel settingsPanel;
    public ProfileView profileView;
    public HelpView helpView;
    public AboutUsView aboutUsView;
    public TermsAndConditionsView conditionsView;
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
            GameManager.Instance.apiHandler.DownloadImage(userData.profile_image, sprite => {

                profileIcon.sprite = sprite;
            });
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
        profileView.Load();
    }

    public void OnHelpButtonAction()
    {
        helpView.Load();
    }

    public void OnAboutButtonAction()
    {
        aboutUsView.Load();
    }

    public void OnTermsButtonAction()
    {
        conditionsView.Load();
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

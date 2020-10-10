using System.Collections.Generic;
using UnityEngine;

enum ELoginFlow
{
    None,
    Launch,
    SignUp,
    Login,
    OTP,
}

public class LoginHandler : MonoBehaviour
{

    public LaunchScreen launchScreen;
    public SignUpScreen signInScreen;
    public LoginScreen loginScreen;
    public LoginOTPScreen otpScreen;

    long contactNumber;
    bool isNewUser = false;

    private void Start()
    {
        SetView(ELoginFlow.Launch);
    }

    private void SetView(ELoginFlow eLoginFlow)
    {
        switch (eLoginFlow)
        {
            case ELoginFlow.Launch:
                launchScreen.SetView(OnLaunchScreen_CallBack);
                break;
            case ELoginFlow.SignUp:
                signInScreen.SetView(OnDetailsScreen_CallBack);
                break;
            case ELoginFlow.Login:
                loginScreen.SetView(OnDetailsScreen_CallBack);
                break;
            case ELoginFlow.OTP:
                otpScreen.SetView(contactNumber, isNewUser, OnLoginScreen_CallBack);
                break;
        }
    }

    void OnLaunchScreen_CallBack(ELoginButtonType loginType)
    {
        launchScreen.gameObject.SetActive(false);
        switch (loginType) {
            case ELoginButtonType.SignUp:
                isNewUser = true;
                SetView(ELoginFlow.SignUp);
                break;

            case ELoginButtonType.Login:
                isNewUser = false;
                SetView(ELoginFlow.Login);
                break;
        }
    }

    void OnDetailsScreen_CallBack(bool status, object body)
    {
        if (status)
        {
            Dictionary<string, object> _body = body as Dictionary<string, object>;

            contactNumber = long.Parse(_body["number"] as string);

            if (isNewUser)
            {
                GameManager.Instance.apiHandler.SignIn(_body, (apiStatus, userInfo) =>
                {
                    if (apiStatus)
                    {
                        signInScreen.gameObject.SetActive(false);

                        SetView(ELoginFlow.OTP);
                    }
                });
            }

            if (!isNewUser)
            {
                loginScreen.gameObject.SetActive(false);

                SetView(ELoginFlow.OTP);
            }
        }
        else
        {
            signInScreen.gameObject.SetActive(false);
            
            SetView(ELoginFlow.Launch);
        }
    }

    void OnLoginScreen_CallBack(bool status, UserData user)
    {
        otpScreen.gameObject.SetActive(false);

        if (status)
        {
            if(user != null)
            {
                isNewUser = false;

                DataManager.Instance.UpdateUserInfo(user);
            }
            GameManager.Instance.sceneController.SwitchScene(ESceneType.HomeScene);
        }
        else
        {
            SetView(ELoginFlow.SignUp);
        }
    }

    public void OnClickCreateNewAccount()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickLogin()
    {
        this.gameObject.SetActive(false);
        otpScreen.gameObject.SetActive(true);
    }
}

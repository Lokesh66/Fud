﻿using System.Collections.Generic;
using UnityEngine;

enum ELoginFlow
{
    None,
    Launch,
    RoleSelection,
    Details,
    Login
}

public class LoginHandler : MonoBehaviour
{

    public LaunchScreen launchScreen;
    public RoleSelectionScreen roleSelectionScreen;
    public DetailsScreen detailsScreen;
    public LoginScreen loginScreen;

    Craft selectedRole = new Craft();
    long contactNumber;
    string userName = string.Empty;
    UserData user;
    bool isNewUser = true;

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
            case ELoginFlow.RoleSelection:
                roleSelectionScreen.SetView(OnRoleSelection_CallBack);
                break;
            case ELoginFlow.Details:
                detailsScreen.SetView(isNewUser, OnDetailsScreen_CallBack);
                break;
            case ELoginFlow.Login:
                loginScreen.SetView(userName, contactNumber, selectedRole.id, isNewUser, OnLoginScreen_CallBack);
                break;
        }
    }

    void OnLaunchScreen_CallBack(ELoginButtonType loginType)
    {
        launchScreen.gameObject.SetActive(false);
        switch (loginType) {
            case ELoginButtonType.Login:
                isNewUser = false;
                SetView(ELoginFlow.Details);
                break;

            case ELoginButtonType.SignUp:
                isNewUser = true;
                SetView(ELoginFlow.RoleSelection);
                break;
        }
    }

    void OnRoleSelection_CallBack(Craft item)
    {
        roleSelectionScreen.gameObject.SetActive(false);

        if (item == null)
        {
            SetView(ELoginFlow.Launch);
        }
        else
        {
            selectedRole = item;
            SetView(ELoginFlow.Details);
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
                userName = _body["name"] as string;

                int countryId = (int)_body["country_id"];

                GameManager.Instance.apiHandler.SignIn(userName, contactNumber, selectedRole.id, countryId, (apiStatus, userInfo) =>
                {
                    if (apiStatus)
                    {
                        detailsScreen.gameObject.SetActive(false);

                        SetView(ELoginFlow.Login);
                    }
                });
            }

            if (!isNewUser)
            {
                SetView(ELoginFlow.Login);
            }
        }
        else
        {
            detailsScreen.gameObject.SetActive(false);

            if (isNewUser)
            {
                SetView(ELoginFlow.RoleSelection);
            }
            else
            {
                SetView(ELoginFlow.Launch);
            }
        }
    }

    void OnLoginScreen_CallBack(bool status, UserData user)
    {
        loginScreen.gameObject.SetActive(false);

        if (status)
        {
            if(user != null)
            {
                isNewUser = false;

                this.user = user;

                DataManager.Instance.UpdateUserInfo(user);
            }
            GameManager.Instance.sceneController.SwitchScene(ESceneType.HomeScene);
        }
        else
        {
            SetView(ELoginFlow.Details);
        }
    }

    public void OnClickCreateNewAccount()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickLogin()
    {
        this.gameObject.SetActive(false);
        loginScreen.gameObject.SetActive(true);
    }

}

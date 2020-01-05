using UnityEngine;
using TMPro;

public class LoginScreen : MonoBehaviour
{
    public TMP_InputField otpInputField;

    bool isSignIn = false;
    int roleId;
    long number;

    System.Action<bool, User> LoginAction;
    public void SetView(long number,int roleId ,bool isNewUser, System.Action<bool, User> action)
    {
        gameObject.SetActive(true);
        this.number = number;
        this.roleId = roleId;
        isSignIn = isNewUser;
        otpInputField.text = "";
        LoginAction = action;
    }

    public void OnClick_Login()
    {
        if (isSignIn)
        {
            GameManager.Instance.apiHandler.SignIn(number, otpInputField.text, roleId, (bool status, User userData) => {
                if (status)
                {
                    Login();
                }
                else
                {

                }
            });
        }
        else
        {
            Login();
        }
    }

    void Login()
    {
        GameManager.Instance.apiHandler.Login(number, otpInputField.text, (bool status, User userData) => {
            if (status)
            {
                LoginAction?.Invoke(true, userData);
            }
            else
            {

            }
        });
    }

    public void OnClick_ResendOTP()
    {
        GameManager.Instance.apiHandler.SendOTP(number, (bool status) => {
            if (status)
            {
               
            }
            else
            {
                //Erros message
            }
        });
    }

    public void OnClick_Back()
    {
        LoginAction?.Invoke(false, null);
    }
}

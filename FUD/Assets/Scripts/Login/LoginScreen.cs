using UnityEngine;
using TMPro;

public class LoginScreen : MonoBehaviour
{
    public TMP_InputField otpInputField;

    string number;

    System.Action<bool, User> LoginAction;
    public void SetView(string number, System.Action<bool, User> action)
    {
        gameObject.SetActive(true);
        this.number = number;
        otpInputField.text = "";
        LoginAction = action;
    }

    public void OnClick_Login()
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

using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class LoginScreen : MonoBehaviour
{
    public TextMeshProUGUI signInText;
    public TextMeshProUGUI errorText;
    public TMP_InputField otpInputField;
    public Toggle toggle;
    bool isSignIn = false;
    int roleId;
    long number;

    System.Action<bool, UserData> LoginAction;
    public void SetView(long number,int roleId ,bool isNewUser, System.Action<bool, UserData> action)
    {
        gameObject.SetActive(true);
        this.number = number;
        this.roleId = roleId;
        isSignIn = isNewUser;
        otpInputField.text = "";
        LoginAction = action;
        toggle.gameObject.SetActive(isNewUser);
        toggle.isOn = false;
        signInText.text = isSignIn ? "SignIn" : "Login";
    }

    public void OnClick_Login()
    {
        if (isSignIn)
        {
            if (isSignIn && !toggle.isOn)
            {
                ShowErrorText();
                return;
            }
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
        GameManager.Instance.apiHandler.Login(number, long.Parse(otpInputField.text), (bool status, UserData userData) => {
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

    void ShowErrorText()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetDelay(2f);
        sequence.SetEase(Ease.OutBounce);
        sequence.Append(errorText.DOFade(0.0f, 1f)).OnStart(() => {
            errorText.gameObject.SetActive(true);
            errorText.color = Color.red;
        }).OnComplete(() => {
            errorText.gameObject.SetActive(false);
        });
    }
}

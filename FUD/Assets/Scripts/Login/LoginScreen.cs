using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class LoginScreen : MonoBehaviour
{
    public TextMeshProUGUI signInText;
    public TextMeshProUGUI errorText;
    public TextMeshProUGUI timerText;
    public TMP_InputField otpInputField;
    public Button resendOtp;
    public Toggle toggle;
    bool isSignIn = false;
    int roleId;
    long number;
    string userName;
    float timer = 120; //2 Minutes
    bool updateTimer;

    System.Action<bool, UserData> LoginAction;

    public void SetView(string userName, long number,int roleId ,bool isNewUser, System.Action<bool, UserData> action)
    {
        gameObject.SetActive(true);
        this.number = number;
        this.roleId = roleId;
        this.userName = name;
        isSignIn = isNewUser;
        otpInputField.text = "";
        LoginAction = action;
        toggle.gameObject.SetActive(isNewUser);
        toggle.isOn = false;
        signInText.text = isSignIn ? "Sign In" : "Login";

        StartTimer();
    }

    float offset = 0;
    private void Update()
    {
        if (updateTimer)
        {
            offset += Time.deltaTime;
            if(offset >= 1)
            {
                offset -= 1;
                timer--;
                timerText.text = "Resend : "+timer;

                if(timer == 0)
                {
                    updateTimer = false;
                    offset = 0;
                    timer = 120;
                    timerText.text = "";
                    resendOtp.interactable = true;
                }
            }
        }
    }
    void StartTimer()
    {
        resendOtp.interactable = false;
        updateTimer = true;
        timer = 120;
    }
    public void OnClick_Login()
    {
        Login();
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
                StartTimer();
            }

            string message = status ? "Please enter OTP that we have sent to registered mobile number" : "Something went worng, please try again";

            ShowValidationMessage(message);
        });
    }

    public void OnClick_Back()
    {
        LoginAction?.Invoke(false, null);
    }

    void ShowValidationMessage(string message)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = message;

        UIManager.Instance.ShowAlert(alertModel);
    }
}

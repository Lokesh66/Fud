using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetailsScreen : MonoBehaviour
{
    public TextMeshProUGUI signInText;
    public TMP_InputField nameFieldText;
    public TMP_InputField numberFieldText;

    System.Action<bool, object> OnButtonAction;

    private bool isNewUser = false;


    public void SetView(bool isNewUser, System.Action<bool, object> action)
    {
        gameObject.SetActive(true);

        numberFieldText.text = "";
        nameFieldText.text = "";
        OnButtonAction = action;

        nameFieldText.gameObject.SetActive(isNewUser);

        this.isNewUser = isNewUser;

        signInText.text = isNewUser ? "SignIn" : "Login";
    }

    public void OnClick_SendOTP()
    {
        if (string.IsNullOrEmpty(numberFieldText.text))
        {
            return;
        }

        if (isNewUser && string.IsNullOrEmpty(nameFieldText.text))
        {
            return;
        }

        if (isNewUser)
        {
            Dictionary<string, string> body = new Dictionary<string, string>();

            body.Add("name", nameFieldText.text);

            body.Add("number", numberFieldText.text);

            OnButtonAction?.Invoke(true, body);
        }
        else
        {
            GameManager.Instance.apiHandler.SendOTP(long.Parse(numberFieldText.text), (bool status) =>
            {
                if (status)
                {
                    Dictionary<string, string> body = new Dictionary<string, string>();

                    body.Add("number", numberFieldText.text);

                    OnButtonAction?.Invoke(true, body);
                }
                else
                {
                    
                }

                string message = status ? "Please enter OTP that we have sent to registered mobile number" : "Something went worng, please try again";

                ShowValidationMessage(message);
            });
        }
    }

    public void OnClick_Back()
    {
        OnButtonAction?.Invoke(false, 0);
    }

    void ShowValidationMessage(string message)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = message;

        UIManager.Instance.ShowAlert(alertModel);
    }
}

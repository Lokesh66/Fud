using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginScreen : MonoBehaviour
{
    public TMP_InputField numberFieldText;


    System.Action<bool, object> OnButtonAction;


    public void SetView(System.Action<bool, object> action)
    {
        gameObject.SetActive(true);

        numberFieldText.text = string.Empty;
        OnButtonAction = action;
    }

    public void OnClick_SendOTP()
    {
        if (string.IsNullOrEmpty(numberFieldText.text))
        {
            return;
        }
 
        GameManager.Instance.apiHandler.SendOTP(long.Parse(numberFieldText.text), (bool status, string response) =>
        {
            if (status)
            {
                Dictionary<string, object> body = new Dictionary<string, object>();

                body.Add("number", numberFieldText.text);

                OnButtonAction?.Invoke(true, body);
            }
            else
            {
                    
            }

            BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

            string message = status ? "Please enter OTP that we have sent to registered mobile number" : baseResponse.message;

            ShowValidationMessage(message);
        });
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

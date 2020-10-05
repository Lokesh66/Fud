using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetailsScreen : MonoBehaviour
{
    public TextMeshProUGUI signInText;
    public TMP_InputField nameFieldText;
    public TMP_InputField numberFieldText;
    public TMP_Dropdown countryDropdown;

    System.Action<bool, object> OnButtonAction;

    private bool isNewUser = false;

    List<CountryModel> countryModels;


    public void SetView(bool isNewUser, System.Action<bool, object> action)
    {
        gameObject.SetActive(true);

        numberFieldText.text = "";
        nameFieldText.text = "";
        OnButtonAction = action;

        nameFieldText.gameObject.SetActive(isNewUser);

        this.isNewUser = isNewUser;

        signInText.text = isNewUser ? "Sign In" : "Login";

        countryDropdown.gameObject.SetActive(isNewUser);

        if (isNewUser)
        {
            SetCountryDropdown();
        }
    }

    void SetCountryDropdown()
    {
        GameManager.Instance.apiHandler.GetCountries((status, modelsList) => {

            if (status)
            {
                countryModels = modelsList;

                List<string> options = new List<string>();

                foreach (var option in countryModels)
                {
                    options.Add(option.name);
                }

                countryDropdown.ClearOptions();

                countryDropdown.AddOptions(options);
            }
        });
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
            Dictionary<string, object> body = new Dictionary<string, object>();

            CountryModel selectedModel = countryModels[countryDropdown.value];

            body.Add("name", nameFieldText.text);

            body.Add("number", numberFieldText.text);

            body.Add("country_id", selectedModel.id);

            OnButtonAction?.Invoke(true, body);
        }
        else
        {
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

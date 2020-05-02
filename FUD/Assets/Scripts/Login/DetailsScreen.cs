using UnityEngine;
using TMPro;

public class DetailsScreen : MonoBehaviour
{
    public TextMeshProUGUI signInText;
    public TMP_InputField mobileNumberTextField;

    System.Action<bool, long> OnButtonAction;

    public void SetView(bool isNewUser, System.Action<bool, long> action)
    {
        gameObject.SetActive(true);

        mobileNumberTextField.text = "";
        OnButtonAction = action;
        signInText.text = isNewUser ? "SignIn" : "Login";
    }

    public void OnClick_SendOTP()
    {
        if (string.IsNullOrEmpty(mobileNumberTextField.text))
        {
            return;
        }
        GameManager.Instance.apiHandler.SendOTP(long.Parse(mobileNumberTextField.text), (bool status) => {
            if (status)
            {
                OnButtonAction?.Invoke(true, long.Parse(mobileNumberTextField.text));
            }
            else
            {
                //Erros message
            }
        });
    }

    public void OnClick_Back()
    {
        OnButtonAction?.Invoke(false, 0);
    }
}

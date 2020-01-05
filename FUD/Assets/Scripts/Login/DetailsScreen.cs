using UnityEngine;
using TMPro;

public class DetailsScreen : MonoBehaviour
{
    public TMP_InputField mobileNumberTextField;

    System.Action<bool, string> OnButtonAction;

    public void SetView(System.Action<bool, string> action)
    {
        gameObject.SetActive(true);

        mobileNumberTextField.text = "";
        OnButtonAction = action;
    }

    public void OnClick_SendOTP()
    {
        GameManager.Instance.apiHandler.SendOTP(mobileNumberTextField.text, (bool status) => {
            if (status)
            {
                OnButtonAction?.Invoke(true, mobileNumberTextField.text);
            }
            else
            {
                //Erros message
            }
        });
    }

    public void OnClick_Back()
    {
        OnButtonAction?.Invoke(false, null);
    }
}

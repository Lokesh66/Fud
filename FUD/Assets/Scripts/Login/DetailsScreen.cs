using UnityEngine;
using TMPro;

public class DetailsScreen : MonoBehaviour
{
    public TMP_InputField mobileNumberTextField;

    System.Action<bool, long> OnButtonAction;

    public void SetView(System.Action<bool, long> action)
    {
        gameObject.SetActive(true);

        mobileNumberTextField.text = "";
        OnButtonAction = action;
    }

    public void OnClick_SendOTP()
    {
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

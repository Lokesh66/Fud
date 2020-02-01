using UnityEngine;
using System;
using TMPro;


public class UserSearchCell : MonoBehaviour
{
    public TextMeshProUGUI userNameText;


    Action<object> OnButtonPressed;

    public void SetView(UserSearchModel searchModel, Action<object> onButtonPressed)
    {
        this.OnButtonPressed = onButtonPressed;

        userNameText.text = "Dummy";
    }

    public void OnButtonAction()
    {
        OnButtonPressed?.Invoke(5);
    }
}

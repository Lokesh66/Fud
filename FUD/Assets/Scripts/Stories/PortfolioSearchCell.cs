using UnityEngine;
using System;
using TMPro;


public class PortfolioSearchCell : MonoBehaviour
{
    public TextMeshProUGUI userNameText;


    Action<object> OnButtonPressed;

    UserSearchModel searchModel;

    public void SetView(UserSearchModel searchModel, Action<object> onButtonPressed)
    {
        this.OnButtonPressed = onButtonPressed;

        this.searchModel = searchModel;

        userNameText.text = searchModel.name;
    }

    public void OnButtonAction()
    {
        OnButtonPressed?.Invoke(searchModel.id);
    }
}

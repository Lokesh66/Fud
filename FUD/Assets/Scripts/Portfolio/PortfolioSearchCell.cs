using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class PortfolioSearchCell : MonoBehaviour
{
    public GameObject selectObject;

    public GameObject selectBG;

    public TextMeshProUGUI userNameText;


    Action<PortfolioSearchCell, object> OnButtonPressed;

    UserSearchModel searchModel;

    public void SetView(UserSearchModel searchModel, Action<PortfolioSearchCell, object> onButtonPressed)
    {
        this.OnButtonPressed = onButtonPressed;

        this.searchModel = searchModel;

        userNameText.text = searchModel.name;
    }

    public void OnButtonAction()
    {
        selectObject.SetActive(true);

        OnButtonPressed?.Invoke(this, searchModel);
    }

    public void UpdateDeselectViw()
    {
        selectObject.SetActive(false);
    }
}

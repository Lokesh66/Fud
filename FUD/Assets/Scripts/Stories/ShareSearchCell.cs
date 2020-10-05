using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class ShareSearchCell : MonoBehaviour
{
    public GameObject selectObject;

    public GameObject selectBG;

    public TextMeshProUGUI userNameText;


    Action<ShareSearchCell, object> OnButtonPressed;

    UserSearchModel searchModel;



    public void SetView(UserSearchModel searchModel, Action<ShareSearchCell, object> onButtonPressed)
    {
        this.OnButtonPressed = onButtonPressed;

        this.searchModel = searchModel;

        userNameText.text = searchModel.name;
    }

    public void OnSelectButtonAction()
    {
        selectObject.SetActive(true);

        OnButtonPressed?.Invoke(this, searchModel);
    }

    public void UpdateDeselectViw()
    {
        selectObject.SetActive(false);
    }
}

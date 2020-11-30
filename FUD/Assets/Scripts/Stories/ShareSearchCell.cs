using UnityEngine;
using System;
using TMPro;


public class ShareSearchCell : MonoBehaviour
{
    public GameObject selectObject;

    public GameObject selectBG;

    public TextMeshProUGUI userNameText;


    Action<ShareSearchCell, object> OnButtonPressed;

    Action<object> OnButtonAtion;

    UserSearchModel searchModel;



    public void SetView(UserSearchModel searchModel, Action<ShareSearchCell, object> onButtonPressed, Action<object> OnButtonAtion)
    {
        this.OnButtonPressed = onButtonPressed;

        this.OnButtonAtion = OnButtonAtion;

        this.searchModel = searchModel;

        userNameText.text = searchModel.name;
    }

    public void OnSelectButtonAction()
    {
        selectObject.SetActive(OnButtonPressed != null);

        OnButtonPressed?.Invoke(this, searchModel);
    }

    public void UpdateDeselectViw()
    {
        selectObject.SetActive(false);
    }

    public void OnCellButtonAction()
    {
        OnButtonAtion?.Invoke(searchModel);
    }
}

using UnityEngine;
using System;
using TMPro;

public class FilterSubCell : MonoBehaviour
{
    public TextMeshProUGUI filterText;

    public Color disableColor;


    object model;

    Action<object, FilterSubCell> OnButtonAction;


    public void Load(object model, string filterMessage, Action<object, FilterSubCell> OnButtonAction)
    {
        this.model = model;

        filterText.text = filterMessage;

        this.OnButtonAction = OnButtonAction;
    }

    public void OnTapAction()
    {
        filterText.color = Color.white;

        OnButtonAction?.Invoke(model, this);
    }

    public void DeSelect()
    {
        filterText.color = disableColor;
    }
}

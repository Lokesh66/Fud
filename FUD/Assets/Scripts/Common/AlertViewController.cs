using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class AlertViewController : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    public Button okayClick;


    public void ShowAlert(AlertModel alertModel)
    {
        messageText.text = alertModel.message;

        gameObject.SetActive(true);

        okayClick.onClick.AddListener(delegate { alertModel.okayButtonAction?.Invoke(); });
    }
}

public class AlertModel
{
    public string message;

    public Action okayButtonAction;
}

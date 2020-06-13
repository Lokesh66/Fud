using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class AlertViewController : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    public GameObject cancelButtonObject;

    public GameObject tickObject;

    public Button okayClick;

    AlertModel alertModel;


    public void ShowAlert(AlertModel alertModel)
    {
        this.alertModel = alertModel;

        messageText.text = alertModel.message;

        tickObject.SetActive(alertModel.canEnableTick);

        gameObject.SetActive(true);

        cancelButtonObject.SetActive(alertModel.canEnableCancelButton);

       // okayClick.onClick.AddListener(delegate { alertModel.okayButtonAction?.Invoke(); });
    }

    public void OkbuttonAction()
    {
        gameObject.SetActive(false);

        alertModel.okayButtonAction?.Invoke();
    }

    public void CancelButtonAction()
    {
        gameObject.SetActive(false);

        alertModel.cancelButtonAction?.Invoke();
    }
}

public class AlertModel
{
    public string message;

    public bool canEnableTick = false;

    public bool canEnableCancelButton = false;

    public List<string> buttonTitles;

    public Action okayButtonAction;

    public Action cancelButtonAction;
}

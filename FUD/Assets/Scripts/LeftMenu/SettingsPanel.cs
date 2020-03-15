using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public void SetView()
    {
        gameObject.SetActive(true);
    }
    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    public void CashFree ()
    {
        Dictionary<string, object> planIdInfo = new Dictionary<string, object>
        {
            { "plan_id", 1 }
        };
        Debug.Log ("CashFree button action");
        GameManager.Instance.apiHandler.CashFreeRequest ("911234567890", "GMAIL_ACCOUNT", planIdInfo);
    }


}

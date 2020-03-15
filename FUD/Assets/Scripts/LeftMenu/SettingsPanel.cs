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
        GameManager.Instance.apiHandler.CashFreeRequest ("123456789012", "jka@gmail.com");
    }


}

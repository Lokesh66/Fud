using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeftMenu : MonoBehaviour
{
    public SettingsPanel settingsPanel;
    public MyProfilePanel profilePanel;

    public Image profileIcon;
    public TMP_Text nameText;

    System.Action OnCloseAction;
    public void SetView(System.Action OnClose)
    {
        gameObject.SetActive(true);
        OnCloseAction = OnClose;
    }
    public void BackButtonAction()
    {
        gameObject.SetActive(false);
        OnCloseAction?.Invoke();
        OnCloseAction = null;
    }

    public void SettingsButtonAction()
    {
        settingsPanel.SetView();
    }

    public void ProfileButtonAction()
    {
        profilePanel.SetView();
    }
}

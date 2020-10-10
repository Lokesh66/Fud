using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class UserAuditionController : MonoBehaviour
{
    public enum EAuditionStatusScreen
    {
        None = -1,
        Live = 0,
        Shortlisted = 1,
        Viewed = 2,
    }

    public GameObject userAuditionCell;

    public GameObject buttonsPanel;

    public AuditionResponsesView liveAuditionResponsesView;

    public ShortListedAuditionView shortListedAuditionView;

    public ViewedAuditionsView viewedAuditionsView;

    public GameObject livePanel;

    public GameObject shortListedPanel;

    public TextMeshProUGUI[] buttonList;


    public Color selectedColor;

    public Color disabledColor;


    EAuditionStatusScreen currentType = EAuditionStatusScreen.None;

    List<SearchAudition> activeAuditions;

    UserAuditionCell selectedAuditionCell;

    Action<bool> OnBack;

    List<string> videoURLs = new List<string>();


    int auditionId;

    public void SetView(List<SearchAudition> auditions, int auditionId, Action<bool> OnBack)
    {
        this.activeAuditions = auditions;

        this.OnBack = OnBack;

        this.auditionId = auditionId;

        gameObject.SetActive(true);

        OnTabAction(0);
    }

    public void OnTabAction(int tabIndex)
    {
        UpdateScreen(tabIndex);
    }

    void UpdateScreen(int index)
    {
        EAuditionStatusScreen screenSubType = (EAuditionStatusScreen)index;

        if (currentType != screenSubType)
        {
            if (currentType != EAuditionStatusScreen.None)
            {
                buttonList[(int)currentType].color = disabledColor;
            }

            //noDataObject.SetActive(false);

            currentType = screenSubType;

            buttonList[(int)currentType].color = selectedColor;

            UpdateScreen();
        }
    }

    void UpdateScreen()
    {
        shortListedPanel.SetActive(currentType == EAuditionStatusScreen.Shortlisted);

        livePanel.SetActive(currentType == EAuditionStatusScreen.Live);

        switch (currentType)
        {
            case EAuditionStatusScreen.Live:
                liveAuditionResponsesView.Load(activeAuditions, auditionId, this);
                break;
            case EAuditionStatusScreen.Shortlisted:
                shortListedAuditionView.Load(auditionId, this);
                break;
            case EAuditionStatusScreen.Viewed:
                viewedAuditionsView.Load(auditionId, this);
                break;
        }
    }

    public void OnAuditionSelectAction(UserAuditionCell audition)
    {
        selectedAuditionCell = audition;

        buttonsPanel.SetActive(true);
    }

    void OnVideoComplete()
    {
        Debug.Log("User Audition Controller : OnVideoComplete Called");

        buttonsPanel.SetActive(true);
    }

    void Reload()
    {
        OnBack?.Invoke(true);

        OnBackButtonAction();
    }

    public void OnPlayButtonAction()
    {
        if (currentType == EAuditionStatusScreen.Live)
        {
            liveAuditionResponsesView.OnPlayButtonAction();
        }
        else {
            shortListedAuditionView.OnPlayButtonAction();
        }
    }

    public void AcceptButtonAction()
    {
        buttonsPanel.SetActive(false);

        GameManager.Instance.apiHandler.UpdateAuditionStatus(selectedAuditionCell.auditionData.audition_id, selectedAuditionCell.auditionData.id, 5, (status, response) =>
        {
            if (status)
            {
                Reload();
            }
        });
    }

    public void RejectButtonAction()
    {
        buttonsPanel.SetActive(false);

        GameManager.Instance.apiHandler.UpdateAuditionStatus(selectedAuditionCell.auditionData.audition_id, selectedAuditionCell.auditionData.id, 8, (status, response) =>
        {
            if (status)
            {
                Reload();
            }
        });
    }

    public void ShortListButtonAction()
    {
        buttonsPanel.SetActive(false);

        GameManager.Instance.apiHandler.UpdateAuditionStatus(selectedAuditionCell.auditionData.audition_id, selectedAuditionCell.auditionData.id, 1, (status, response) =>
        {
            if (status)
            {
                Reload();
            }
        });
    }

    public void OnFilterButtonAction()
    {
        switch (currentType)
        {
            case EAuditionStatusScreen.Live:
                liveAuditionResponsesView.OnFilterButtonAction();
                break;
            case EAuditionStatusScreen.Shortlisted:
                shortListedAuditionView.OnFilterButtonAction();
                break;
            case EAuditionStatusScreen.Viewed:
                viewedAuditionsView.OnFilterButtonAction();
                break;
        }
    }

    public void OnCancelButtonAction()
    {
        buttonsPanel.SetActive(false);
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);

        activeAuditions = null;

        OnBack = null;

        currentType = EAuditionStatusScreen.None;
    }
}

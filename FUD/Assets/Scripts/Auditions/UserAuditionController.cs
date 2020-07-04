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
    }

    public GameObject userAuditionCell;

    public GameObject buttonsPanel;

    public AuditionResponsesView liveAuditionResponsesView;

    public ShortListedAuditionView shortListedAuditionView;

    public GameObject livePanel;

    public GameObject shortListedPanel;

    public TextMeshProUGUI[] buttonList;


    public Color selectedColor;

    public Color disabledColor;


    EAuditionStatusScreen currentType = EAuditionStatusScreen.None;

    List<SearchAudition> activeAuditions;

    SearchAudition selectedAudition;

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
                liveAuditionResponsesView.Load(activeAuditions, auditionId);
                break;
            case EAuditionStatusScreen.Shortlisted:
                shortListedAuditionView.Load(auditionId);
                break;
        }
    }

    void SetShortListedVideoThumbnails(int index)
    {
        //shortListedCellsList[index].SetVideoThumbnail(() => {

        //    index++;

        //    if (index >= shortListedAudtions.Count)
        //    {
        //        return;
        //    }

        //    SetShortListedVideoThumbnails(index);
        //});
    }

    public void OnAuditionSelectAction(SearchAudition audition)
    {
        selectedAudition = audition;

        MultimediaModel model = selectedAudition.UserAuditionMultimedia[0];

        EMediaType mediaType = model.GetMediaType(model.media_type);

        if (mediaType == EMediaType.Video)
        {
            VideoStreamer.Instance.StreamVideo(model.content_url, OnVideoComplete);
        }
        if (mediaType == EMediaType.Audio)
        {
            AudioStreamer.Instance.AudioStream(model.content_url, OnVideoComplete);
        }
        else
        {
            buttonsPanel.SetActive(true);
        }
    }

    void OnVideoComplete()
    {
        buttonsPanel.SetActive(true);
    }

    void Reload()
    {
        OnBack?.Invoke(true);

        OnBackButtonAction();
    }

    public void AcceptButtonAction()
    {
        buttonsPanel.SetActive(false);

        GameManager.Instance.apiHandler.AcceptOrRejectAudition(selectedAudition.audition_id, selectedAudition.id, 5, (status, response) =>
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

        GameManager.Instance.apiHandler.AcceptOrRejectAudition(selectedAudition.audition_id, selectedAudition.id, 8, (status, response) =>
        {
            if (status)
            {
                Reload();
            }
        });
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

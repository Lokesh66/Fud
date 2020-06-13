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

    public Transform activeContent;

    public Transform shortListedContent;

    public GameObject buttonsPanel;

    public GameObject livePanel;

    public GameObject shortListedPanel;

    public TextMeshProUGUI[] buttonList;


    public Color selectedColor;

    public Color disabledColor;


    EAuditionStatusScreen currentType = EAuditionStatusScreen.None;

    List<SearchAudition> activeAuditions;

    List<SearchAudition> shortListedAudtions;

    SearchAudition selectedAudition;

    Action<bool> OnBack;

    List<string> videoURLs = new List<string>();

    List<UserAuditionCell> liveCellsList = new List<UserAuditionCell>();

    List<UserAuditionCell> shortListedCellsList = new List<UserAuditionCell>();

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
                LoadLiveAuditions();
                break;
            case EAuditionStatusScreen.Shortlisted:
                LoadShortListedAuditions();
                break;
        }
    }

    void LoadLiveAuditions()
    {
        if (activeContent.childCount > 0)
            return;

        liveCellsList.Clear();

        for (int i = 0; i < activeAuditions.Count; i++)
        {
            GameObject storyObject = Instantiate(userAuditionCell, activeContent);

            UserAuditionCell item = storyObject.GetComponent<UserAuditionCell>();

            item.SetView(activeAuditions[i], OnAuditionSelectAction);

            liveCellsList.Add(item);
        }

        if (activeAuditions.Count > 0)
        {
            SetLiveVideoThumbnails(0);
        }
    }

    void SetLiveVideoThumbnails(int index)
    {
        liveCellsList[index].SetVideoThumbnail(() => {

            index++;

            if (index >= activeAuditions.Count)
            {
                return;
            }

            SetLiveVideoThumbnails(index);
        });
    }

    void LoadShortListedAuditions()
    {
        if (shortListedAudtions == null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("id", auditionId);
            parameters.Add("page", 0);
            parameters.Add("limit", 20);
            parameters.Add("status", "shortlisted");

            GameManager.Instance.apiHandler.SearchAuditions(parameters, (status, response) => {

                if (status)
                {
                    SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(response);

                    if (auditionResponse.data.Count > 0)
                    {
                        shortListedAudtions = auditionResponse.data;

                        UpdateShortListedView();
                    }
                }
            });
        }
    }

    void SetShortListedVideoThumbnails(int index)
    {
        shortListedCellsList[index].SetVideoThumbnail(() => {

            index++;

            if (index >= shortListedAudtions.Count)
            {
                return;
            }

            SetShortListedVideoThumbnails(index);
        });
    }

    void UpdateShortListedView()
    {
        shortListedCellsList.Clear();

        for (int i = 0; i < shortListedAudtions.Count; i++)
        {
            GameObject storyObject = Instantiate(userAuditionCell, shortListedContent);

            UserAuditionCell item = storyObject.GetComponent<UserAuditionCell>();

            item.SetView(shortListedAudtions[i], OnAuditionSelectAction);

            shortListedCellsList.Add(item);
        }

        if (shortListedAudtions.Count > 0)
        {
            SetShortListedVideoThumbnails(0);
        }
    }

    void OnAuditionSelectAction(SearchAudition audition)
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

        activeAuditions = shortListedAudtions = null;

        OnBack = null;

        currentType = EAuditionStatusScreen.None;

        activeContent.DestroyChildrens();

        shortListedContent.DestroyChildrens();
    }
}

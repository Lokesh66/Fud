using System.Collections;
using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class AuditionResponsesView : MonoBehaviour
{
    public GameObject buttonsPanel;

    public GameObject noDataObject;

    public AuditionResponsesTableView tableView;

    public AuditionLiveFilterView filterView;

    public List<SearchAudition> auditionResponses;


    UserAuditionController auditionController;

    UserAuditionCell selectedAuditionCell;

    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_AUDITION_RESPONSES = 50;

    int auditionId = -1;


    public void Load(List<SearchAudition> searchAuditions, int auditionId, UserAuditionController auditionController)
    {
        this.auditionResponses = searchAuditions;

        this.auditionId = auditionId;

        this.auditionController = auditionController;

        pageNo++;

        if (auditionResponses.Count < MAX_AUDITION_RESPONSES)
        {
            isPagingOver = true;

            pageNo = 1;
        }

        if (!isInitialized)
        {
            tableView.gameObject.SetActive(true);

            isInitialized = true;
        }
        else
        {
            tableView.Data.Clear();

            tableView.Data.Add(auditionResponses.Count);

            tableView.Refresh();
        }
    }

    public void OnAuditionSelectAction(UserAuditionCell audition)
    {
        selectedAuditionCell = audition;

        auditionController.OnAuditionSelectAction(audition);
    }

    public void OnPlayButtonAction()
    {
        MultimediaModel model = selectedAuditionCell.auditionData.UserAuditionMultimedia[0];

        EMediaType mediaType = model.GetMediaType(model.media_type);

        if (mediaType == EMediaType.Video)
        {
            UIManager.Instance.topCanvas.PlayVideo(selectedAuditionCell.icon, selectedAuditionCell.mediaPlayer);
            //VideoStreamer.Instance.StreamVideo(model.content_url, OnVideoComplete);
        }
        else if (mediaType == EMediaType.Audio)
        {
            UIManager.Instance.topCanvas.PlayVideo(selectedAuditionCell.icon, selectedAuditionCell.mediaPlayer, EMediaType.Audio);
            //AudioStreamer.Instance.AudioStream(model.content_url, OnVideoComplete);
        }
        else
        {
            UIManager.Instance.ShowBigScreen(model.content_url);
            //buttonsPanel.SetActive(true);
        }
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", auditionId);

        GameManager.Instance.apiHandler.SearchAuditions(pageNo, parameters, (status, response) =>
        {
            if (status)
            {
                SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(response);

                this.auditionResponses = auditionResponse.data;

                pageNo++;

                if (auditionResponses.Count < MAX_AUDITION_RESPONSES)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                tableView.Data.Clear();

                tableView.Data.Add(auditionResponses.Count);

                tableView.Refresh();
            }
        });
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        auditionResponses = data as List<SearchAudition>;

        tableView.Data.Clear();

        if (auditionResponses.Count > 0)
        {
            tableView.Data.Add(auditionResponses.Count);

            tableView.Refresh();
        }

        noDataObject.SetActive(auditionResponses.Count == 0);
    }

    void OnVideoComplete()
    {
        Debug.Log("OnVideo Complete Called : AuditionResponsesView");

        //buttonsPanel.SetActive(true);
    }
}

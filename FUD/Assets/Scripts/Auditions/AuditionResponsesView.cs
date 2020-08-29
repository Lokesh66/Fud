using System.Collections;
using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class AuditionResponsesView : MonoBehaviour
{
    public GameObject buttonsPanel;

    public AuditionResponsesTableView tableView;

    public List<SearchAudition> auditionResponses;

    SearchAudition selectedAudition;

    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_AUDITION_RESPONSES = 50;

    int auditionId = -1;


    public void Load(List<SearchAudition> searchAuditions, int auditionId)
    {
        this.auditionResponses = searchAuditions;

        this.auditionId = auditionId;

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

    public void OnAuditionSelectAction(SearchAudition audition)
    {
        selectedAudition = audition;

        buttonsPanel.SetActive(true);
    }

    public void OnPlayButtonAction()
    {
        MultimediaModel model = selectedAudition.UserAuditionMultimedia[0];

        EMediaType mediaType = model.GetMediaType(model.media_type);

        if (mediaType == EMediaType.Video)
        {
            VideoStreamer.Instance.StreamVideo(model.content_url, OnVideoComplete);
        }
        else if (mediaType == EMediaType.Audio)
        {
            AudioStreamer.Instance.AudioStream(model.content_url, OnVideoComplete);
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

    void OnVideoComplete()
    {
        Debug.Log("OnVideo Complete Called : AuditionResponsesView");

        //buttonsPanel.SetActive(true);
    }
}

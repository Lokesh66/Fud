using System.Collections;
using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class ShortListedAuditionView : MonoBehaviour
{
    public GameObject buttonsPanel;

    public ShortListedAuditionTableView tableView;

    public List<SearchAudition> auditionResponses;

    public GameObject noDataObject;

    SearchAudition selectedAudition;

    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_AUDITION_RESPONSES = 50;

    int auditionId = -1;


    public void Load(int auditionId)
    {
        this.auditionId = auditionId;

        GetShortListedAuditions();
    }

    void GetShortListedAuditions()
    {
        if (auditionResponses == null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("id", auditionId);
           
            parameters.Add("status", "shortlisted");

            GameManager.Instance.apiHandler.SearchAuditions(pageNo, parameters, (status, response) => {

                if (status)
                {
                    SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(response);

                    auditionResponses = auditionResponse.data;

                    noDataObject.SetActive(auditionResponses.Count == 0);

                    if (auditionResponse.data.Count > 0)
                    {
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
                }
            });
        }
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
        buttonsPanel.SetActive(true);
    }
}

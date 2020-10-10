using System.Collections;
using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class ShortListedAuditionView : MonoBehaviour
{
    public GameObject buttonsPanel;

    public ShortListedAuditionTableView tableView;

    public AuditionResponseModifiedFilterView filterView;

    public List<SearchAudition> auditionResponses;

    public GameObject noDataObject;


    UserAuditionController auditionController;

    UserAuditionCell selectedAuditionCell;

    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_AUDITION_RESPONSES = 50;

    int auditionId = -1;


    public void Load(int auditionId, UserAuditionController auditionController)
    {
        this.auditionId = auditionId;

        this.auditionController = auditionController;

        GetShortListedAuditions();
    }

    void GetShortListedAuditions()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", auditionId);

        parameters.Add("source_from", 1);


        GameManager.Instance.apiHandler.SearchAuditions(1, parameters, (status, response) =>
        {
            if (status)
            {
                Debug.Log("Short listed Audition Response = " + response);

                SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(response);

                auditionResponses = auditionResponse.data;

                if (auditionResponses.Count > 0)
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
                else
                {
                    AlertModel alertModel = new AlertModel();

                    alertModel.message = "You have not received any audition responses";

                    UIManager.Instance.ShowAlert(alertModel);
                }
            }
        });
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

        parameters.Add("source_from", 1);

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

                    pageNo = 1;
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
        Debug.Log("OnVideo Complete Called : ShortListedView");

        buttonsPanel.SetActive(true);
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
}

using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class AuditionOfferedView : MonoBehaviour
{
    public NoDataView noDataView;

    public AuditionDetailView detailView;

    public ETabType tabType;

    public UserAuditionController userAuditionController;

    public AuditionsOfferedTableView tableView;


    public AuditionOfferedFilterView filterView;


    public List<Audition> offeredAuditions;

    bool isPagingOver = false;

    bool isInitialized = false;

    int pageNo = 1;

    int MAX_OFFERED_AUDITIONS = 50;


    public void Load()
    {
        ClearData();

        gameObject.SetActive(true);

        GetAuditions();
    }

    #region ButtonActions

    public void GetAuditions()
    {
        noDataView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetOfferedAuditions(pageNo, (status, response) => {

            if (status)
            {
                AuditionsResponse auditionsResponse = JsonUtility.FromJson<AuditionsResponse>(response);
                offeredAuditions = auditionsResponse.data;

                pageNo++;

                if (offeredAuditions.Count < MAX_OFFERED_AUDITIONS)
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
                    Reload();
                }

                if (offeredAuditions.Count == 0)
                {
                    noDataView.SetView(GetNoDataModel());
                }

                noDataView.gameObject.SetActive(offeredAuditions?.Count == 0);

            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });   
    }

    #endregion

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Live Auditions Right Now";

        return noDataModel;
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetOfferedAuditions(pageNo, (status, response) =>
        {
            if (status)
            {
                AuditionsResponse auditionsResponse = JsonUtility.FromJson<AuditionsResponse>(response);
                this.offeredAuditions = auditionsResponse.data;

                pageNo++;

                if (offeredAuditions.Count < MAX_OFFERED_AUDITIONS)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                Reload();
            }
        });
    }

    public void LoadAuditionDetails(Audition auditionModel)
    {
        detailView.Load(auditionModel);
    }

    void ClearData()
    {
        pageNo = 1;

        isPagingOver = false;
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnFilterAction(object data)
    {
        offeredAuditions = data as List<Audition>;

        Reload();

        noDataView.gameObject.SetActive(offeredAuditions.Count == 0);
    }

    void Reload()
    {
        tableView.Data.Clear();

        tableView.Data.Add(offeredAuditions.Count);

        tableView.Refresh();
    }
}


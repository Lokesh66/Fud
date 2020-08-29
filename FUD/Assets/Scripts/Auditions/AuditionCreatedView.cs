using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class AuditionCreatedView : MonoBehaviour
{
    public NoDataView noDataView;

    public AuditionDetailView detailView;

    public ETabType tabType;

    public UserAuditionController userAuditionController;

    public AuditionCreateFilterView filterView;

    public AuditionsCreatedTableView tableView;

    public AuditionCreatedDetailsView detailsView;


    [HideInInspector]
    public List<Audition> createdAuditions;


    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_CREATED_AUDITIONS = 50;

    bool isInitialized = false;


    public void Load()
    {
        ClearData();

        gameObject.SetActive(true);

        GetAuditions();
    }

    public void SetUserAuditions(List<SearchAudition> userAuditions, int auditionId)
    {
        userAuditionController.SetView(userAuditions, auditionId, OnClosed);
    }

    public void OnFilterButtonAction()
    {
        filterView.Load(OnFilterAction);
    }

    void OnClosed(bool canReload)
    {
        if (canReload)
        {
            GetAuditions();
        }
    }

    public void OnCellTapAction(AuditionCreatedCell createdCell)
    {
        detailsView.Load(this, createdCell);
    }

    #region ButtonActions

    public void GetAuditions()
    {
        noDataView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetCreatedAuditions(pageNo, (status, response) => {

            if (status)
            {
                AuditionsResponse auditionsResponse = JsonUtility.FromJson<AuditionsResponse>(response);
                createdAuditions = auditionsResponse.data;

                pageNo++;

                if (createdAuditions.Count < MAX_CREATED_AUDITIONS)
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

                    tableView.Data.Add(createdAuditions.Count);

                    tableView.Refresh();
                }

                if (createdAuditions.Count == 0)
                {
                    noDataView.SetView(GetNoDataModel());
                }

                noDataView.gameObject.SetActive(createdAuditions?.Count == 0);

            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });
    }

    public void CreateAudition()
    {
        Dictionary<string, object> parametres = new Dictionary<string, object>();
        GameManager.Instance.apiHandler.CreateAudition(parametres, (bool status, string response) =>
        {
            if (status)
            {
                Debug.Log("CreateAuction : "+ response);
            }
        });
    }

    public void ModifyAudition()
    {
        Dictionary<string, object> parametres = new Dictionary<string, object>();
        GameManager.Instance.apiHandler.ModifyAudition(parametres, (bool status, string response) =>
        {
            if (status)
            {
                Debug.Log("ModifyAuction : " + response);
            }
        });
    }
    #endregion

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Created Auditions Right Now";

        return noDataModel;
    }

    public void LoadAuditionDetails(Audition auditionModel)
    {
        detailView.Load(auditionModel);
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData();
    }

    void GetNextPageData()
    {
        GameManager.Instance.apiHandler.GetCreatedAuditions(pageNo, (status, response) =>
        {
            if (status)
            {
                AuditionsResponse auditionsResponse = JsonUtility.FromJson<AuditionsResponse>(response);
                this.createdAuditions = auditionsResponse.data;

                pageNo++;

                if (createdAuditions.Count < MAX_CREATED_AUDITIONS)
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

                tableView.Data.Add(createdAuditions.Count);

                tableView.Refresh();
            }
        });
    }

    void ClearData()
    {
        pageNo = 1;

        isPagingOver = false;
    }

    void OnFilterAction(object data)
    {
        createdAuditions = data as List<Audition>;

        tableView.Data.Clear();

        tableView.Data.Add(createdAuditions.Count);

        tableView.Refresh();

        noDataView.gameObject.SetActive(createdAuditions.Count == 0);
    }
}


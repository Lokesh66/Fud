using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class AuditionAlteredView : MonoBehaviour
{
    public NoDataView noDataView;

    public AuditionDetailView detailView;

    public ETabType tabType;

    public UserAuditionController userAuditionController;

    public AuditionsAlteredTableView tableView;

    public AuditionAlteredFilterView filterView;


    [HideInInspector]
    public List<JoinedAudition> joinedAuditions;


    bool isPagingOver = false;

    bool isInitialized = false;

    int pageNo = 1;

    int MAX_ALTERED_AUDITIONS = 50;

    public void Load()
    {
        ClearData();

        gameObject.SetActive(true);

        GetAuditions();
    }

    void OnClosed(bool canReload)
    {
        if (canReload)
        {
            GetAuditions();
        }
    }


    #region ButtonActions

    public void GetAuditions()
    {
        noDataView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetAlteredAuditions(pageNo, (status, response) => {

            JoinedAuditionsResponse joinedAuditionsResponse = JsonUtility.FromJson<JoinedAuditionsResponse>(response);

            joinedAuditions = joinedAuditionsResponse.data;

            if (status && joinedAuditions.Count > 0)
            {
                pageNo++;

                if (joinedAuditions.Count < MAX_ALTERED_AUDITIONS)
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

                    tableView.Data.Add(joinedAuditions.Count);

                    tableView.Refresh();
                }
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

        noDataModel.subTitle = "No Joined Auditions Right Now";

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
        GameManager.Instance.apiHandler.GetAlteredAuditions(pageNo, (status, response) => {

            if (status)
            {
                JoinedAuditionsResponse joinedAuditionsResponse = JsonUtility.FromJson<JoinedAuditionsResponse>(response);

                joinedAuditions = joinedAuditionsResponse.data;

                pageNo++;

                if (joinedAuditions.Count < MAX_ALTERED_AUDITIONS)
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

                tableView.Data.Add(joinedAuditions.Count);

                tableView.Refresh();
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
        joinedAuditions = data as List<JoinedAudition>;

        tableView.Data.Clear();

        tableView.Data.Add(joinedAuditions.Count);

        tableView.Refresh();

        noDataView.gameObject.SetActive(joinedAuditions.Count == 0);
    }
}

public enum EAuditionStatus
{ 
    None,
    Review,
    ShortListed,
    Rejected,
}

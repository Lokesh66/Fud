using System.Collections.Generic;
using UnityEngine;


public enum AuditionType
{
    Live,
    Joined,
    Created
}
public class AuditionController : MonoBehaviour
{
    public RectTransform content;

    public GameObject auditionCell;

    public NoDataView noDataView;

    public AuditionType auditionType;

    public UserAuditionController userAuditionController;


    private void OnEnable()
    {
        GetAuditions();
    }

    public void Load(List<Audition> auditions)
    {
        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (auditions != null && auditions.Count > 0)
        {
            noDataView.gameObject.SetActive(false);

            for (int i = 0; i < auditions.Count; i++)
            {
                GameObject auditionObject = Instantiate(auditionCell, content);

                auditionObject.GetComponent<AuditionCell>().SetView(this, auditions[i]);
            }
        }
        else
        {
            noDataView.gameObject.SetActive(true);
            noDataView.SetView(GetNoDataModel());
        }
    }

    public void Load(List<JoinedAudition> auditions)
    {
        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (auditions != null && auditions.Count > 0)
        {
            noDataView.gameObject.SetActive(false);

            for (int i = 0; i < auditions.Count; i++)
            {
                GameObject auditionObject = Instantiate(auditionCell, content);

                auditionObject.GetComponent<JoinedAuditionCell>().SetView(this, auditions[i]);
            }
        }
        else
        {
            noDataView.gameObject.SetActive(true);
            noDataView.SetView(GetNoDataModel());
        }
    }

    public void SetUserAuditions(List<SearchAudition> userAuditions, int auditionId)
    {
        userAuditionController.SetView(userAuditions, auditionId, OnClosed);
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

        GameManager.Instance.apiHandler.FetchAuditions(auditionType, (status, response) => {

            if (status)
            {
                if (auditionType == AuditionType.Live)
                {
                    AuditionsResponse auditionsResponse = JsonUtility.FromJson<AuditionsResponse>(response);
                    Load(auditionsResponse.data);
                }
                else
                {
                    JoinedAuditionsResponse joinedAuditionsResponse = JsonUtility.FromJson<JoinedAuditionsResponse>(response);
                    Load(joinedAuditionsResponse.data);
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
        switch (auditionType)
        {
            case AuditionType.Live:
                noDataModel.subTitle = "No Live Auditions Right Now";
                break;
            case AuditionType.Joined:
                noDataModel.subTitle = "No Joined Auditions Right Now";
                break;
            case AuditionType.Created:
                noDataModel.subTitle = "No Created Auditions Right Now";
                break;
        }

        return noDataModel;
    }
}

public enum EAuditionStatus
{ 
    None,
    Review,
    ShortListed,
    Rejected,
}

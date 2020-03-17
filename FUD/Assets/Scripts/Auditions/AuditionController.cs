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

    public GameObject noDataObject;

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
            noDataObject.SetActive(false);

            for (int i = 0; i < auditions.Count; i++)
            {
                GameObject auditionObject = Instantiate(auditionCell, content);

                auditionObject.GetComponent<AuditionCell>().SetView(this, auditions[i]);
            }
        }
        else
        {
            noDataObject.SetActive(true);
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
            noDataObject.SetActive(false);

            for (int i = 0; i < auditions.Count; i++)
            {
                GameObject auditionObject = Instantiate(auditionCell, content);

                auditionObject.GetComponent<JoinedAuditionCell>().SetView(this, auditions[i]);
            }
        }
        else
        {
            noDataObject.SetActive(true);
        }
    }

    public void SetUserAuditions(List<UserAudition> userAuditions)
    {
        userAuditionController.SetView(this, userAuditions);
    }


    #region ButtonActions

    public void GetAuditions()
    {
        noDataObject.SetActive(false);

        Debug.Log("GetAuditions");
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
                noDataObject.SetActive(true);
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
}

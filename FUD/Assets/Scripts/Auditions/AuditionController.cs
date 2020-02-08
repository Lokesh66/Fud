using System.Collections.Generic;
using UnityEngine;


public class AuditionController : MonoBehaviour
{
    public RectTransform content;

    public GameObject auditionCell;

    public GameObject noDataObject;

    public bool isJoined;

    private void OnEnable()
    {
        GetAuditions();
    }
    public void Load(List<Audition> auditions)
    {
        if (auditions != null && auditions.Count > 0)
        {
            noDataObject.SetActive(false);

            for (int i = 0; i < auditions.Count; i++)
            {
                GameObject auditionObject = Instantiate(auditionCell, content);

                auditionObject.GetComponent<AuditionCell>().SetView(i, auditions[i]);
            }
        }
        else
        {
            noDataObject.SetActive(true);
        }
    }

    #region ButtonActions

    public void GetAuditions()
    {
        Debug.Log("GetAuditions");
        GameManager.Instance.apiHandler.SearchAuditions(isJoined, (status, auditions) => {
            if (status)
            {
                Debug.Log("GetAuditions : " + auditions.Count);

                Load(auditions);
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
        GameManager.Instance.apiHandler.UpdateCreatedAudition(parametres, (bool status, string response) =>
        {
            if (status)
            {
                Debug.Log("ModifyAuction : " + response);
            }
        });
    }
    #endregion
}

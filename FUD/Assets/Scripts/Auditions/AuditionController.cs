using System.Collections.Generic;
using UnityEngine;


public class AuditionController : MonoBehaviour
{
    public RectTransform content;

    public GameObject auditionCell;


    public void Load()
    {
        /*for (int i = 0; i < 5; i++)
        {
            GameObject auditionObject = Instantiate(auditionCell, content);

            auditionObject.GetComponent<AuditionCell>().SetView(i);
        }*/
    }

    #region ButtonActions

    public void GetAuditions()
    {
        GameManager.Instance.apiHandler.GetAllAuditions((bool status, List<Audition> auditions) => {
            if (status)
            {
                Debug.Log("GetAuditions : " + auditions.Count);
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

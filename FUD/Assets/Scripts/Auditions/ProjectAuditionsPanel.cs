using UnityEngine;
using System.Collections.Generic;

public class ProjectAuditionsPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject auditionCell;
    public NoDataView noData;

    private void OnEnable()
    {
        GetAuditions();
    }
    void GetAuditions()
    {
        noData.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetAllAuditions((status, auditionsList) => {
            Debug.Log("GetAuditions : "+status);
            if (status)
            {
                foreach (Transform child in parentContent)
                {
                    GameObject.Destroy(child.gameObject);
                }

                if (auditionsList!=null && auditionsList.Count > 0)
                {
                    Load(auditionsList);
                }
                else
                {
                    EnableNodata();
                }
            }
            else
            {
                EnableNodata();
            }
        });
    }

    private void Load(List<Audition> auditions)
    {
        for (int i = 0; i < auditions.Count; i++)
        {
            GameObject auditionObject = Instantiate(auditionCell, parentContent);

            auditionObject.GetComponent<ProjectAuditionCell>().SetView(i, auditions[i]);
        }
    }
    public void CreateAudition()
    {
        CreateAuditionView.Instance.SetView(1, (isNewDataUpdated) => {
            if (isNewDataUpdated)
            {
                GetAuditions();
            }
        });
    }

    void EnableNodata()
    {
        noData.gameObject.SetActive(true);
        noData.SetView(GetNoDataModel());
    }
    NoDataModel GetNoDataModel()
    {
        NoDataModel model = new NoDataModel();
        model.buttonName = "Create Audition";
        model.subTitle = "No Audition Right now";
        model.buttonAction = CreateAudition;
        return model;
    }
}

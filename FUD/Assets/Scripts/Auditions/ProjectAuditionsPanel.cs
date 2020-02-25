using UnityEngine;
using System.Collections.Generic;

public class ProjectAuditionsPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject auditionCell;
    public NoDataView noData;

    int projectId;

    public void SetData(int projectId, List<Audition> auditions)
    {
        this.projectId = projectId;
        if(auditions != null && auditions.Count > 0)
        {
            noData.gameObject.SetActive(false);

            for (int i = 0; i < auditions.Count; i++)
            {
                GameObject auditionObject = Instantiate(auditionCell, parentContent);

                auditionObject.GetComponent<ProjectAuditionCell>().SetView(i, auditions[i]);
            }
        }
        else
        {
            EnableNodata();
        }

    }
    public void CreateAudition()
    {
        CreateAuditionView.Instance.SetView(projectId, (isNewDataUpdated) => {
            if (isNewDataUpdated)
            {
                ProjectsDetailedView.Instance.Reload();
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

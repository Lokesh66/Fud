using System.Collections.Generic;
using UnityEngine;

public class ProjectCastsPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject castCell;
    public NoDataView noData;

    int projectId;

    public void SetData(int projectId, List<ProjectCast> casts)
    {
        this.projectId = projectId;
        if (casts != null && casts.Count > 0)
        {
            noData.gameObject.SetActive(false);

            for (int i = 0; i < casts.Count; i++)
            {
                GameObject auditionObject = Instantiate(castCell, parentContent);

                auditionObject.GetComponent<ProjectCastCell>().SetView(i, casts[i]);
            }
        }
        else
        {
            EnableNodata();
        }
    }
    public void CreateCast()
    {
        CreateCastView.Instance.SetView(projectId, (isNewDataUpdated) =>
        {
            if (isNewDataUpdated)
            {
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
        model.buttonName = "Create Cast";
        model.subTitle = "No Cast Right now";
        model.buttonAction = CreateCast;
        return model;
    }
}

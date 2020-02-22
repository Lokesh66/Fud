using UnityEngine;
using System.Collections.Generic;

public class ProjectAuditionsPanel : MonoBehaviour
{
    public Transform parentContent;
    public GameObject auditionCell;
    public NoDataView noData;    

    public void SetData(List<Audition> auditions)
    {
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
        CreateAuditionView.Instance.SetView(1, (isNewDataUpdated) => {
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
        model.buttonName = "Create Audition";
        model.subTitle = "No Audition Right now";
        model.buttonAction = CreateAudition;
        return model;
    }
}

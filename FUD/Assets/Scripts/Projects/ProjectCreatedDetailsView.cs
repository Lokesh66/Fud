using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectCreatedDetailsView : MonoBehaviour
{

    AuditionCreatedCell auditionCell;

    Project projectModel;


    public void Load(Project projectModel)
    {
        this.projectModel = projectModel;

        gameObject.SetActive(true);
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetProjectDetails(projectModel.id, (status, projectDetail) =>
        {
            if (status)
            {
                ProjectsDetailedView.Instance.SetView(projectDetail, () =>
                {
                });
            }
        });
    }

    public void OnEditButtonAction()
    {
        gameObject.SetActive(false);

        //CreateAuditionView.Instance.EditView(projectModel, Refresh);
    }

    void Refresh(bool isDataUpdated)
    {
        if (isDataUpdated)
        {
            //controller.GetAuditions();
        }
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }
}

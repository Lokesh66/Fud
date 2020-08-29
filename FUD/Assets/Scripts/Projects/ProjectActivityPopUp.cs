using System;
using TMPro;
using UnityEngine;

public class ProjectActivityPopUp : MonoBehaviour
{
    //public Image storyImage;

    public GameObject rejectObject;


    public ProjectCastDetailsView detailsView;


    ProjectCast activityModel;

    ProjectCastsPanel castPanel;


    int currentUserId;



    public void Load(ProjectCast activityModel, ProjectCastsPanel castsPanel)
    {
        gameObject.SetActive(true);

        this.activityModel = activityModel;

        this.castPanel = castsPanel;

        currentUserId = DataManager.Instance.userInfo.id;
    }

    public void OnViewButtonAction()
    {
        GameManager.Instance.apiHandler.GetProjectCasts(1, "cast&crew", activityModel.project_id, (status, response) => {

            if (status)
            {
                gameObject.SetActive(false);

                ProjectCast projectCast = JsonUtility.FromJson<ProjectCastDetailsResponse>(response).data;

                detailsView.Load(projectCast);
            }
        });
    }

    public void OnStatusUpdate(int statusIndex)
    {
        if (statusIndex != -1)
        {
            GameManager.Instance.apiHandler.UpdateProjectCastStauts(activityModel.project_id, activityModel.id, statusIndex, (status) =>
            {
                if (status)
                {
                    activityModel.cast_status = statusIndex;
                    castPanel.OnStatusApplied(activityModel);
                    gameObject.SetActive(false);
                }
            });
        }
        else {
            gameObject.SetActive(false);
        }
    }

    public void OnGoToAuditionAction()
    {
        gameObject.SetActive(false);

        ProjectsDetailedView.Instance.LoadAuditions();
    }
}

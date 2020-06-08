using System;
using TMPro;
using UnityEngine;

public class ProjectActivityPopUp : MonoBehaviour
{
    //public Image storyImage;

    public TextMeshProUGUI descriptionText;

    public GameObject acceptObject;

    public GameObject rejectObject;


    ProjectCast activityModel;

    ProjectCastsPanel castPanel;


    int currentUserId;



    public void Load(ProjectCast activityModel, ProjectCastsPanel castsPanel)
    {
        gameObject.SetActive(true);

        this.activityModel = activityModel;

        this.castPanel = castsPanel;

        //descriptionText.text = activityModel.comment;

        currentUserId = DataManager.Instance.userInfo.id;

        acceptObject.SetActive(currentUserId != activityModel.user_id);

        rejectObject.SetActive(activityModel.cast_status != 8);
    }

    public void OnStatusUpdate(int statusIndex)
    {
        if (statusIndex != -1)
        {
            GameManager.Instance.apiHandler.UpdateProjectCastStauts(activityModel.project_id, activityModel.id, statusIndex, (status) =>
            {
                if (status)
                {
                    castPanel.OnStatusApplied(activityModel);
                    gameObject.SetActive(false);
                }
            });
        }
        else {
            gameObject.SetActive(false);
        }
    } 
}

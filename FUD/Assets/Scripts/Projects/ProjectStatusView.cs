using TMPro;
using UnityEngine;

public class ProjectStatusView : MonoBehaviour
{

    public ProjectDetailView detailView;


    ProjectOfferedModel selectedProject;

    ProjectHandler projectHandler;


    public void Load(ProjectOfferedModel selectedProject, ProjectHandler projectHandler)
    {
        this.selectedProject = selectedProject;

        this.projectHandler = projectHandler;

        gameObject.SetActive(true);
    }

    public void OnButtonAction(int buttonIndex)
    {
        gameObject.SetActive(false);

        int status = -1;

        switch (buttonIndex)
        {
            case 0://Accept
                status = 3;
                break;
            case 1://Reject
                status = 8;
                break;
        }

        UpdateProjectStatus(status);
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);    
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        detailView.Load(selectedProject);
    }

    void UpdateProjectStatus(int status)
    {
        GameManager.Instance.apiHandler.UpdateProjectStauts(selectedProject.project_id, selectedProject.project_cast_id, status, (apiStatus, response) => {

            OnAPIResponse(apiStatus, response);
        });
    }

    void OnAPIResponse(bool status, string response)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

        alertModel.message = status ? "Project status updated Successfully" : baseResponse.message;

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        gameObject.SetActive(false);

        projectHandler.RemoveProject(selectedProject);
    }
}

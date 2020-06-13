using System.Collections.Generic;
using UnityEngine;

public class ProjectHandler : MonoBehaviour
{
    public RectTransform content;

    public ETabType tabType;

    public GameObject projectCell;

    public NoDataView noDataView;


    List<Project> projectModels;


    public void Load()
    {
        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        switch (tabType)
        {
            case ETabType.Offers:
                GetOfferedProjects();
                break;

            case ETabType.Altered:
                GetAlteredProjects();
                break;

            case ETabType.Created:
                GetAllProjects();
                break;
        }
    }

    void GetOfferedProjects()
    {
        GameManager.Instance.apiHandler.GetOfferedProjects((status, projectsResponse) => {

            if (status && projectsResponse != null)
            {
                Load(projectsResponse.data);
            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });
    }

    void GetAlteredProjects()
    {
        GameManager.Instance.apiHandler.GetAlteredProjects((status, projectsResponse) => {

            if (status)
            {
                Load(projectsResponse.data);
            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });
    }

    void GetAllProjects()
    {
        noDataView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetProjects((status, projectsResponse) => {

            if (status)
            {
                Load(projectsResponse.data);
            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });
    }

    void Load(List<Project> models)
    {
        content.DestroyChildrens();

        projectModels = models;

        if(models != null && models.Count > 0)
        {
            noDataView.gameObject.SetActive(false);

            for (int i = 0; i < models.Count; i++)
            {
                GameObject projectObject = Instantiate(projectCell, content);

                projectObject.GetComponent<ProjectCell>().SetView(models[i], OnProjectClickAction);
            }
        }
        else
        {
            noDataView.gameObject.SetActive(true);
            noDataView.SetView(GetNoDataModel());
        }
        
    }

    void OnProjectClickAction(Project project)
    {
        if (tabType == ETabType.Created)
        {
            GameManager.Instance.apiHandler.GetProjectDetails(project.id, (status, projectDetail) =>
            {
                if (status)
                {
                    ProjectsDetailedView.Instance.SetView(projectDetail, () =>
                    {
                    });
                }
            });
        }
        else if(tabType == ETabType.Offers)
        {

        }
    }

    public void OnAddButtonAction()
    {
        if (DataManager.Instance.CanLoadScreen(EFeatureType.ProjectCreation))
        {
            GameManager.Instance.apiHandler.GetProjectStories((status, response) =>
            {
                if (status)
                {
                    Debug.Log("OnAddButtonAction : " + response);

                    ProjectStoriesResponse stories = JsonUtility.FromJson<ProjectStoriesResponse>(response);

                    if (stories.data != null && stories.data.Count > 0)
                    {
                        ProjectCreationView.Instance.SetView(stories.data, (isProjectAdded) =>
                        {
                            if (isProjectAdded)
                            {
                                GetAllProjects();
                            }
                        });
                    }
                    else {
                        AlertModel alertModel = new AlertModel();

                        alertModel.message = "You don't have enough stories to create a project.";

                        UIManager.Instance.ShowAlert(alertModel);
                    }
                }
            });
        }
        else {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.ProjectCreation);
        }
    }

    public void RemoveProject(Project selectedProject)
    {
        int projectIndex = projectModels.IndexOf(selectedProject);

        Destroy(content.GetChild(projectIndex).gameObject);

        projectModels.Remove(selectedProject);
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        switch (tabType) 
        {
            case ETabType.Offers:
                noDataModel.subTitle = "No offered Projects Right Now";
                noDataModel.buttonName = "Awaiting";
                break;

            case ETabType.Altered:
                noDataModel.subTitle = "No Altered Projects Right Now";
                noDataModel.buttonName = "Awaiting";
                break;

            case ETabType.Created:
                noDataModel.subTitle = "No Active Projects Right Now";
                noDataModel.buttonName = "Create Project";
                noDataModel.buttonAction = OnAddButtonAction;
                break;
        }

        return noDataModel;
    }
}

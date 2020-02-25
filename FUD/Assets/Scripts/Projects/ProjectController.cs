using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    public RectTransform content;

    public GameObject projectCell;

    public NoDataView noDataView;

    List<Project> projectModels;

    private void OnEnable()
    {
        GetAllProjects();
    }
    public void GetAllProjects()
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
        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }

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
        GameManager.Instance.apiHandler.GetProjectDetails(project.id, (status, projectDetail) => {
            if (status)
            {
                ProjectsDetailedView.Instance.SetView(projectDetail, () => {
                });
            }
        });
    }

    public void OnAddButtonAction()
    {
        GameManager.Instance.apiHandler.GetProjectStories((status, response) => {
            if (status)
            {
                Debug.Log("OnAddButtonAction : "+response);

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
            }
        });
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Active Projects Right Now";

        noDataModel.buttonName = "Create Project";

        noDataModel.buttonAction = OnAddButtonAction;

        return noDataModel;
    }
}

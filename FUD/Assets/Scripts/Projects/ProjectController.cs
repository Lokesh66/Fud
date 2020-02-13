using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    public RectTransform content;

    public GameObject projectCell;

    public GameObject noDataObject;

    List<ProjectDataModel> projectModels;


    private void OnEnable()
    {
        GetAllProjects();
    }
    public void GetAllProjects()
    {
        noDataObject.SetActive(false);

        GameManager.Instance.apiHandler.GetProjects((status, projectsResponse) => {

            if (status)
            {
                Load(projectsResponse.data);
            }
            else
            {
                noDataObject.SetActive(true);
            }
        });
    }

    void Load(List<ProjectDataModel> models)
    {
        foreach (Transform child in content)
        {
            GameObject.Destroy(child.gameObject);
        }

        projectModels = models;
        if(models != null && models.Count > 0)
        {
            noDataObject.SetActive(false);
            for (int i = 0; i < models.Count; i++)
            {
                GameObject projectObject = Instantiate(projectCell, content);

                projectObject.GetComponent<ProjectCell>().SetView(models[i], OnProjectClickAction);
            }
        }
        else
        {
            noDataObject.SetActive(true);

        }
        
    }

    void OnProjectClickAction(ProjectDataModel project)
    {
        ProjectsDetailedView.Instance.SetView(project, () => { });
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

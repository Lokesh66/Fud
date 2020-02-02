using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    public RectTransform content;

    public GameObject projectCell;

    List<ProjectDataModel> projectModels;

    public void Load()
    {
        gameObject.SetActive(true);

        GameManager.Instance.apiHandler.GetProjects((status, projectsResponse) => {

            if (status)
            {
                LoadView(projectsResponse.data);
            }
        });
    }

    void LoadView(List<ProjectDataModel> models)
    {
        projectModels = models;

        for (int i = 0; i < models.Count; i++)
        {
            GameObject projectObject = Instantiate(projectCell, content);

            projectObject.GetComponent<ProjectCell>().SetView();
        }
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

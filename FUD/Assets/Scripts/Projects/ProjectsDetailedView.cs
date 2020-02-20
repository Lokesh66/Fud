using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectsDetailedView : MonoBehaviour
{
    #region Singleton

    private static ProjectsDetailedView instance = null;

    private ProjectsDetailedView()
    {

    }

    public static ProjectsDetailedView Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ProjectsDetailedView>();
            }
            return instance;
        }
    }
    #endregion

    public GameObject parentPanel;

    System.Action OnBackButtonClick;

    private Project project;
    public void SetView(Project projectData, System.Action backAction)
    {
        project = projectData;
        parentPanel.SetActive(true);
        OnBackButtonClick = backAction;        
    }

    private void Load()
    {
    }
    void Reload()
    {
        GameManager.Instance.apiHandler.GetProjectDetails(project.id, (status, project) =>
        {
            if (status)
            {
                this.project = project;
                Load();
            }
        });
    }

    public void BackButtonAction()
    {
        parentPanel.SetActive(false);
        OnBackButtonClick?.Invoke();
        OnBackButtonClick = null;
    }
}

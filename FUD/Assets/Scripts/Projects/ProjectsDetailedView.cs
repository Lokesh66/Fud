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
    public GameObject createButton;

    public ProjectStoriesPanel storiesPanel;
    public ProjectCastsPanel castsPanel;
    public ProjectAuditionsPanel auditionsPanel;

    System.Action OnBackButtonClick;

    private Project project;
    public void SetView(Project projectData, System.Action backAction)
    {
        project = projectData;
        parentPanel.SetActive(true);
        OnBackButtonClick = backAction;
        storiesPanel.SetData(projectData.StoryVersions[0]);
        castsPanel.SetData(projectData.id, projectData.Project_cast);
        auditionsPanel.SetData(projectData.Audition);
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

    public void AddNewButtonAction()
    {

    }
}

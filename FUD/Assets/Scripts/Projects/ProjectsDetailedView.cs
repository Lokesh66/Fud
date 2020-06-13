using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public HorizontalLayoutGroup tabsLayoutGroup;

    public ProjectStoriesPanel storiesPanel;
    public ProjectCastsPanel castsPanel;
    public ProjectAuditionsPanel auditionsPanel;
    public ProjectScenesPanel scenesPanel;

    public ProjectAuditionDetailView auditionDetails;

    public UserAuditionController userAuditionController;

    public List<Toggle> tabToggles;


    System.Action OnBackButtonClick;

    private Project project;
    public void SetView(Project projectData, System.Action backAction)
    {
        project = projectData;
        parentPanel.SetActive(true);
        OnBackButtonClick = backAction;
        StartCoroutine(UpdateTabPanelView());
        Load();
    }

    private void Load()
    {
        if (project.StoryVersions.Count > 0)
        {
            storiesPanel.SetData(project.StoryVersions[0]);
        }

        castsPanel.SetData(project.id, project.Project_cast);
        auditionsPanel.SetData(project.id, project.Audition);
        scenesPanel.SetData(project, project.StoryScenes);
    }

    public void Reload()
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

    IEnumerator UpdateTabPanelView()
    {
        yield return new WaitForEndOfFrame();

        tabsLayoutGroup.enabled = false;

        yield return new WaitForEndOfFrame();

        tabsLayoutGroup.enabled = true;
    }

    public void LoadAuditions()
    {
        tabToggles[3].isOn = true;
    }
}

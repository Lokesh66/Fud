using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    public TextMeshProUGUI titleText;

    public GameObject parentPanel;

    public HorizontalLayoutGroup tabsLayoutGroup;

    public ProjectStoriesPanel storiesPanel;
    public ProjectCastsPanel castsPanel;
    public ProjectAuditionsPanel auditionsPanel;
    public ProjectScenesPanel scenesPanel;
    public ProjectTeamView teamView;

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

        titleText.text = projectData.title;
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
        teamView.Load(project.MyProjectTeam);
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
        ClearData();
        parentPanel.SetActive(false);
        OnBackButtonClick?.Invoke();
        OnBackButtonClick = null;
        teamView.Unload();
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

    void ClearData()
    {
        tabToggles[0].isOn = true;
    }

    public Project GetCurrentProjectModel()
    {
        return project;
    }
}

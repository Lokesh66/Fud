using UnityEngine;

public class HomeView : BaseView
{
    public GameObject leftMenuPanel;

    public GameObject projectCreationPanel;

    public GameObject projectsPanel;

    public ProjectController projectController;


    #region Ovveride Methods
    protected override void EnableView()
    {
        base.EnableView();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    protected override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();
    }

    #endregion

    public void OnMenuButtonAction()
    {
        leftMenuPanel.SetActive(true);
    }

    public void OnProjectCreateAction()
    {
        projectCreationPanel.SetActive(true);

        leftMenuPanel.SetActive(false);
    }

    public void OnProjectsAction()
    {
        projectController.Load();

        leftMenuPanel.SetActive(false);
    }

    public void OnCloseLeftMenu()
    {
        leftMenuPanel.SetActive(false);
    }
}

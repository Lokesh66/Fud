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
    public void SetView(ProjectDataModel projectData, System.Action backAction)
    {
        parentPanel.SetActive(true);
        OnBackButtonClick = backAction;
        
    }

    public void BackButtonAction()
    {
        parentPanel.SetActive(false);
        OnBackButtonClick?.Invoke();
        OnBackButtonClick = null;
    }
}

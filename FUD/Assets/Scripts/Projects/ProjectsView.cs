using UnityEngine;
using TMPro;


public class ProjectsView : BaseView
{
    public ProjectHandler offeredProjects;

    public ProjectHandler alteredProjects;

    public ProjectHandler createdProjects;

    public ProjectBrowserView browserView;

    public GameObject addObject;

    public TextMeshProUGUI[] buttonsList;


    public Color selectedColor;

    public Color disabledColor;


    private GameObject currentObject;

    private ETabType currentTab = ETabType.Offers;


    protected override void EnableView()
    {
        base.EnableView();

        UpdateCurrentView();
    }

    protected override void OnAddSubView(GameObject addedObject)
    {
        base.OnAddSubView(addedObject);
    }

    public override void OnRemoveLastSubView()
    {
        base.OnRemoveLastSubView();
    }

    public override void OnExitScreen()
    {
        base.OnExitScreen();

        ResetScreen();
    }

    public void OnTabAction(int tabIndex)
    {
        if (currentTab != (ETabType)tabIndex)
        {
            buttonsList[(int)currentTab].color = disabledColor;

            buttonsList[tabIndex].color = selectedColor;

            currentTab = (ETabType)tabIndex;

            addObject.SetActive(currentTab == ETabType.Created);

            currentObject?.SetActive(false);

            UpdateCurrentView();
        }
    }

    void UpdateCurrentView()
    {
        switch (currentTab)
        {
            case ETabType.Offers:
                currentObject = offeredProjects.gameObject;
                offeredProjects.Load();
                break;

            case ETabType.Altered:
                currentObject = alteredProjects.gameObject;
                alteredProjects.Load();
                break;

            case ETabType.Created:
                currentObject = createdProjects.gameObject;
                createdProjects.Load();
                break;

            case ETabType.Browser:
                currentObject = browserView.gameObject;
                browserView.Load();
                break;
        }
    }

    void ResetScreen()
    {
        buttonsList[(int)currentTab].color = disabledColor;

        buttonsList[0].color = selectedColor;

        currentObject?.SetActive(false);

        currentTab = ETabType.Offers;
    }
}

using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;


public class ProjectHandler : MonoBehaviour
{
    public RectTransform content;

    public ETabType tabType;

    public GameObject projectCell;

    public GameObject projectOfferedCell;

    public NoDataView noDataView;

    public ProjectStatusView offeredDetailsView;


    public ProjectOfferedTableView offeredTableView;

    public ProjectCreatedTableView createdTableView;


    public ProjectAlteredFilterView alteredFilterView;

    public ProjectOfferedFilterView offeredFilterView;


    public ProjectCreatedDetailsView detailsView;


    [HideInInspector]
    public List<CreatedProjectModel> createProjects;

    [HideInInspector]
    public List<Project> alteredProjects;

    [HideInInspector]
    public List<ProjectOfferedModel> offeredModels;


    bool isInitialized = false;

    bool isPagingOver = false;

    int pageNo = 1;

    int MAX_PROJECTS = 50;


    public void Load()
    {
        ClearData();

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        switch (tabType)
        {
            case ETabType.Offers:
                GetOfferedProjects();
                break;

            case ETabType.Altered:
                GetAlteredProjects();
                break;

            case ETabType.Created:
                GetAllProjects();
                break;
        }
    }

    void GetOfferedProjects()
    {
        offeredTableView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetOfferedProjects(pageNo, (status, projectsResponse) => {

            if (status && projectsResponse != null)
            {
                offeredModels = projectsResponse.data;

                pageNo++;

                if (offeredModels.Count < MAX_PROJECTS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                offeredTableView.gameObject.SetActive(true);

                if (offeredModels.Count == 0)
                {
                    noDataView.SetView(GetNoDataModel());
                }

                noDataView.gameObject.SetActive(offeredModels?.Count == 0);
            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });
    }

    void GetAlteredProjects()
    {
        offeredTableView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetAlteredProjects(pageNo, (status, projectsResponse) => {

            if (status && projectsResponse != null)
            {
                offeredModels = projectsResponse.data;

                pageNo++;

                if (offeredModels.Count < MAX_PROJECTS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                offeredTableView.gameObject.SetActive(true);

                if (offeredModels.Count == 0)
                {
                    noDataView.SetView(GetNoDataModel());
                }

                noDataView.gameObject.SetActive(offeredModels?.Count == 0);
            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });
    }

    void GetAllProjects()
    {
        noDataView.gameObject.SetActive(false);

        GameManager.Instance.apiHandler.GetProjects(pageNo, (status, projectsResponse) => {

            if (status && projectsResponse != null)
            {
                createProjects = projectsResponse.data;

                pageNo++;

                if (createProjects.Count < MAX_PROJECTS)
                {
                    isPagingOver = true;

                    pageNo = 1;
                }

                if (!isInitialized)
                {
                    createdTableView.gameObject.SetActive(true);

                    isInitialized = true;
                }
                else
                {
                    createdTableView.Data.Clear();

                    createdTableView.Data.Add(createProjects.Count);

                    createdTableView.Refresh();
                }

                if (createProjects.Count == 0)
                {
                    noDataView.SetView(GetNoDataModel());
                }

                noDataView.gameObject.SetActive(createProjects?.Count == 0);
            }
            else
            {
                noDataView.gameObject.SetActive(true);
                noDataView.SetView(GetNoDataModel());
            }
        });
    }

    public void OnProjectClickAction(CreatedProjectModel project)
    {
        if (tabType == ETabType.Created)
        {
            detailsView.Load(project);
        }
    }

    public void OnOfferedProjectAction(ProjectOfferedModel offeredModel)
    {
        offeredDetailsView.Load(offeredModel, this);
    }

    public void OnAddButtonAction()
    {
        if (DataManager.Instance.CanLoadScreen(EFeatureType.ProjectCreation))
        {
            GameManager.Instance.apiHandler.GetProjectStories((status, response) =>
            {
                if (status)
                {
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
                    else {
                        AlertModel alertModel = new AlertModel();

                        alertModel.message = "You don't have enough stories to create a project.";

                        UIManager.Instance.ShowAlert(alertModel);
                    }
                }
            });
        }
        else {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.ProjectCreation);
        }
    }

    public void RemoveProject(ProjectOfferedModel selectedProject)
    {
        offeredModels.Remove(selectedProject);

        offeredTableView.Data.Clear();

        offeredTableView.Data.Add(offeredModels.Count);

        offeredTableView.Refresh();
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        switch (tabType)
        {
            case ETabType.Offers:
                noDataModel.subTitle = "No offered Projects Right Now";
                noDataModel.buttonName = "Awaiting";
                break;

            case ETabType.Altered:
                noDataModel.subTitle = "No Altered Projects Right Now";
                noDataModel.buttonName = "Awaiting";
                break;

            case ETabType.Created:
                noDataModel.subTitle = "No Active Projects Right Now";
                noDataModel.buttonName = "Create Project";
                noDataModel.buttonAction = OnAddButtonAction;
                break;
        }

        return noDataModel;
    }

    public void OnAPICall()
    {
        if (isPagingOver)
            return;

        GetNextPageData(tabType);
    }

    void GetNextPageData(ETabType tabType)
    {
        switch (tabType)
        {
            case ETabType.Altered:
                GetAlteredNextData();
                break;
            case ETabType.Created:
                GetCreatedNextData();
                break;

            case ETabType.Offers:
                GetOfferedNextData();
                break;
        }
    }

    void GetOfferedNextData()
    {
        GameManager.Instance.apiHandler.GetOfferedProjects(pageNo, (status, projectsResponse) =>
        {
            if (status)
            {
                this.offeredModels = projectsResponse.data;

                pageNo++;

                if (offeredModels.Count < MAX_PROJECTS)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                offeredTableView.Data.Clear();

                offeredTableView.Data.Add(offeredModels.Count);

                offeredTableView.Refresh();
            }
        });
    }

    void GetAlteredNextData()
    {
        GameManager.Instance.apiHandler.GetAlteredProjects(pageNo, (status, projectsResponse) =>
        {
            if (status)
            {
                this.offeredModels = projectsResponse.data;

                pageNo++;

                if (offeredModels.Count < MAX_PROJECTS)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                offeredTableView.Data.Clear();

                offeredTableView.Data.Add(offeredModels.Count);

                offeredTableView.Refresh();
            }
        });
    } 

    void GetCreatedNextData()
    {
        GameManager.Instance.apiHandler.GetProjects(pageNo, (status, projectsResponse) =>
        {
            if (status)
            {
                this.createProjects = projectsResponse.data;

                pageNo++;

                if (offeredModels.Count < MAX_PROJECTS)
                {
                    isPagingOver = true;

                    pageNo = 0;
                }
                else
                {
                    isPagingOver = false;

                    pageNo++;
                }

                createdTableView.Data.Clear();

                createdTableView.Data.Add(createProjects.Count);

                createdTableView.Refresh();
            }
        });
    }

    void ClearData()
    {
        isPagingOver = false;

        pageNo = 1;
    }

    public void OnFilterButtonAction()
    {
        if (tabType == ETabType.Offers)
        {
            offeredFilterView.Load(OnOfferedFilterApplied);
        }
        else {
            alteredFilterView.Load(OnAlteredFilterApplied);
        }
    }

    void OnOfferedFilterApplied(object data)
    {
        offeredModels = data as List<ProjectOfferedModel>;

        offeredTableView.Data.Clear();

        offeredTableView.Data.Add(offeredModels.Count);

        offeredTableView.Refresh();

        noDataView.gameObject.SetActive(offeredModels.Count == 0);
    }

    void OnAlteredFilterApplied(object data)
    {
        alteredProjects = data as List<Project>;

        createdTableView.Data.Clear();

        createdTableView.Data.Add(alteredProjects.Count);

        createdTableView.Refresh();

        noDataView.gameObject.SetActive(alteredProjects.Count == 0);
    }
}

using System.Collections.Generic;
using UnityEngine;


public class ProjectTeamView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;

    public NoDataView noDataView;

    public GameObject addNewButton;

    public ProjectTeamDetails teamDetails;


    public GameObject createTeamObject;


    List<ProjectTeamModel> teamModels;

    GameObject createObject;


    private void OnEnable()
    {
        addNewButton.SetActive(true);
    }

    private void OnDisable()
    {
        addNewButton.SetActive(false);
    }

    public void Load(List<ProjectTeamModel> teamModels)
    {
        this.teamModels = teamModels; 

        content.DestroyChildrens();

        if (teamModels?.Count > 0)
        {
            SetView();
        }
        else
        {
            noDataView.SetView(GetNoDataModel());
        }

        noDataView.gameObject.SetActive(teamModels?.Count == 0);
    }

    void SetView()
    {
        GameObject teamObject = null;

        for (int i = 0; i < teamModels.Count; i++)
        {
            teamObject = Instantiate(cellCache, content);

            teamObject.GetComponent<ProjectTeamCell>().Load(teamModels[i], OnCellButtonAction);
        }
    }

    public void OnCellButtonAction(ProjectTeamModel teamModel)
    {
        teamDetails.Load(teamModel, this);
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Teams Right Now";

        noDataModel.buttonName = "Add Team";

        noDataModel.buttonAction = CreateTeam;

        return noDataModel;
    }

    public void Refresh(ProjectTeamModel teamModel)
    {
        gameObject.SetActive(true);

        teamModels.Add(teamModel);

        GameObject characterObject = Instantiate(cellCache, content);

        characterObject.GetComponent<ProjectTeamCell>().Load(teamModel, OnCellButtonAction);

        noDataView.gameObject.SetActive(teamModels?.Count == 0);
    }

    public void RemoveTeam(ProjectTeamModel teamModel)
    {
        gameObject.SetActive(true);

        int modelIndex = teamModels.IndexOf(teamModel);

        Destroy(content.GetChild(modelIndex).gameObject);

        teamModels.Remove(teamModel);

        if (teamModels.Count <= 0)
        {
            noDataView.SetView(GetNoDataModel());
        }

        noDataView.gameObject.SetActive(teamModels?.Count == 0);
    }

    public void UpdateTeam(ProjectTeamModel oldModel, ProjectTeamModel updatedModel)
    {
        gameObject.SetActive(true);

        int modelIndex = teamModels.IndexOf(oldModel);

        ProjectTeamCell teamCell = content.GetChild(modelIndex).GetComponent<ProjectTeamCell>();

        teamCell.Load(updatedModel, teamCell.GetButtonAction());
    }

    public void CreateTeam()
    {
        if (FindObjectOfType<ProjectTeamCreationView>() != null)
        {
            return;
        }

        createObject = Instantiate(createTeamObject, ProjectsDetailedView.Instance.transform);

        createObject.GetComponent<ProjectTeamCreationView>().SetView(Refresh);

        noDataView.gameObject.SetActive(false);
    }

    public void Unload()
    {
        Destroy(createObject);
    }
}

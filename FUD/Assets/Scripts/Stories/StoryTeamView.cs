using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryTeamView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;

    public NoDataView noDataView;


    List<StoryTeamModel> teamModels;

    StoryDetailsController detailsController;


    public void Load(List<StoryTeamModel> teamModels, StoryDetailsController storyDetails)
    {
        this.teamModels = teamModels;

        gameObject.SetActive(true);

        detailsController = storyDetails;

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

            teamObject.GetComponent<StoryTeamCell>().Load(teamModels[i]);
        }
    }

    NoDataModel GetNoDataModel()
    {
        NoDataModel noDataModel = new NoDataModel();

        noDataModel.subTitle = "No Teams Right Now";

        noDataModel.buttonName = "Add Team";

        noDataModel.buttonAction = detailsController.OnAddButtonAction;

        return noDataModel;
    }

    public void Refresh(StoryTeamModel teamModel)
    {
        gameObject.SetActive(true);

        teamModels.Add(teamModel);

        GameObject characterObject = Instantiate(cellCache, content);

        characterObject.GetComponent<StoryTeamCell>().Load(teamModel);

        noDataView.gameObject.SetActive(teamModels?.Count == 0);
    }
}

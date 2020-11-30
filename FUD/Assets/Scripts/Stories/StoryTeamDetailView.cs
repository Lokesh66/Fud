using frame8.ScrollRectItemsAdapter.GridExample;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StoryTeamDetailView : MonoBehaviour
{
    public TextMeshProUGUI teamNameText;

    public TextMeshProUGUI descriptionText;


    public TeamMembersTableView tableView;


    [HideInInspector]
    public List<TeamMembersItem> dataList;

    StoryTeamModel teamModel;

    bool isInitialised = false;


    public void Load(StoryTeamModel teamModel)
    {
        gameObject.SetActive(true);

        this.teamModel = teamModel;

        dataList = teamModel.TeamMembers;

        SetView();
    }

    public void SetView()
    {
        teamNameText.text = teamModel.title;

        descriptionText.text = teamModel.description;

        List<TeamMembersItem> membersItem = teamModel.TeamMembers.FindAll(item => item.users.id == 0);

        foreach (var item in membersItem)
        {
            teamModel.TeamMembers.Remove(item);
        }

        if (!isInitialised)
        {
            tableView.gameObject.SetActive(true);

            isInitialised = true;
        }
        else
        {
            tableView.Data.Clear();

            tableView.Data.Add(dataList.Count);

            tableView.Refresh();
        }
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

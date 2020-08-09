using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using System;
using TMPro;


public class StoryTeamDetailView : MonoBehaviour
{
    public TextMeshProUGUI teamNameText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI membersText;


    StoryTeamModel teamModel;


    public void Load(StoryTeamModel teamModel)
    {
        gameObject.SetActive(true);

        this.teamModel = teamModel;

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

        StartCoroutine(UpdateTeamMembers());
    }

    IEnumerator UpdateTeamMembers()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < teamModel.TeamMembers.Count; i++)
        {
            UserSearchModel searchModel = new UserSearchModel();

            searchModel.id = teamModel.TeamMembers[i].users.id;

            searchModel.name = teamModel.TeamMembers[i].users.name;

            membersText.text += teamModel.TeamMembers[i].users.name + ",";
        }
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

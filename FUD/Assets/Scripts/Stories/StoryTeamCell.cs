using UnityEngine;
using System;
using TMPro;

public class StoryTeamCell : MonoBehaviour
{
    public TextMeshProUGUI membersText;

    public TextMeshProUGUI titleText;


    StoryTeamModel teamModel;

    Action<StoryTeamModel> OnButtonAction;

    public void Load(StoryTeamModel teamModel, Action<StoryTeamModel> OnButtonAction)
    {
        this.teamModel = teamModel;

        this.OnButtonAction = OnButtonAction;

        SetView();
    }

    void SetView()
    {
        titleText.text = teamModel.title;

        for (int i = 0; i < teamModel.TeamMembersUsers.Count; i++)
        {
            string appendString = i != teamModel.TeamMembersUsers.Count - 1 ? "," : string.Empty;
            membersText.text = teamModel.TeamMembersUsers[i].name + ",";
        }
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(teamModel);
    }
}

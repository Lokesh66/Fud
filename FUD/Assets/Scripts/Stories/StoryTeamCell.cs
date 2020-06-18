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

    public Action<StoryTeamModel> GetButtonAction()
    {
        return OnButtonAction;
    }

    void SetView()
    {
        titleText.text = teamModel.title;

        membersText.text = string.Empty;

        for (int i = 0; i < teamModel.TeamMembers.Count; i++)
        {
            string appendString = i != teamModel.TeamMembers.Count - 1 ? "," : string.Empty;

            membersText.text += teamModel.TeamMembers[i].users.name + appendString;
        }
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(teamModel);
    }
}

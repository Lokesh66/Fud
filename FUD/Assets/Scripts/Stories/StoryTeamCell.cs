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

        membersText.text = teamModel.members;
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(teamModel);
    }
}

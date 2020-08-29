using UnityEngine;
using System;
using TMPro;

public class ProjectTeamCell : MonoBehaviour
{
    public TextMeshProUGUI membersText;

    public TextMeshProUGUI titleText;


    ProjectTeamModel teamModel;

    Action<ProjectTeamModel> OnButtonAction;



    public void Load(ProjectTeamModel teamModel, Action<ProjectTeamModel> OnButtonAction)
    {
        this.teamModel = teamModel;

        this.OnButtonAction = OnButtonAction;

        SetView();
    }

    public Action<ProjectTeamModel> GetButtonAction()
    {
        return OnButtonAction;
    }

    void SetView()
    {
        titleText.text = teamModel.title;

        membersText.text = string.Empty;

        //for (int i = 0; i < teamModel.TeamMembers.Count; i++)
        //{
        //    string appendString = i != teamModel.TeamMembers.Count - 1 ? "," : string.Empty;

        //    membersText.text += teamModel.TeamMembers[i].users.name + appendString;
        //}
    }

    public void OnTapAction()
    {
        OnButtonAction?.Invoke(teamModel);
    }
}

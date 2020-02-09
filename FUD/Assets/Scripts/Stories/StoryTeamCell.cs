using UnityEngine;
using TMPro;


public class StoryTeamCell : MonoBehaviour
{
    public TextMeshProUGUI membersText;

    public TextMeshProUGUI titleText;


    StoryTeamModel teamModel;

    public void Load(StoryTeamModel teamModel)
    {
        this.teamModel = teamModel;

        SetView();
    }

    void SetView()
    {
        titleText.text = teamModel.title;

        membersText.text = teamModel.members;
    }
}

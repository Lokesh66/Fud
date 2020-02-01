using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryTeamView : MonoBehaviour
{
    public RectTransform content;

    public GameObject cellCache;


    List<StoryTeamModel> teamModels;


    public void Load(List<StoryTeamModel> teamModels)
    {
        this.teamModels = teamModels;

        SetView();
    }

    void SetView()
    {
        GameObject teamObject = null;

        for (int i = 0; i < teamModels.Count; i++)
        {
            teamObject = Instantiate(cellCache, content);
        }
    }
}

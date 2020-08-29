using System;
using TMPro;
using UnityEngine;

public class StoryHistoryActivityPopUp : MonoBehaviour
{
    //public Image storyImage;

    public StoryActivitiesView activitiesView;

    public VersionMultimediaView multimediaView;


    StoryActivityModel activityModel;


    Action<int> OnClose;



    public void Load(StoryActivityModel activityModel, Action<int> OnClose)
    {
        gameObject.SetActive(true);
    }

    public void OnViewButtonAction()
    {
        gameObject.SetActive(false);

        StoryVersion storyVersion = new StoryVersion();

        storyVersion.id = activityModel.StoryVersions.id;

        multimediaView.Load(storyVersion);
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }
}

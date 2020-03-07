using System;
using TMPro;
using UnityEngine;

public class StoryActivityPopUp : MonoBehaviour
{
    //public Image storyImage;

    public TextMeshProUGUI descriptionText;

    public GameObject acceptObject;

    public StoryActivitiesView activitiesView;


    StoryActivityModel activityModel;


    Action<int> OnClose;

    int currentUserId;

    public void Load(StoryActivityModel activityModel, Action<int> OnClose)
    {
        gameObject.SetActive(true);

        this.activityModel = activityModel;

        this.OnClose = OnClose;

        //descriptionText.text = activityModel.comment;

        currentUserId = DataManager.Instance.userInfo.id;

        acceptObject.SetActive(currentUserId != activityModel.user_id);
    }

    public void OnStatusUpdate(int statusIndex)
    {
        if (statusIndex != -1)
        {
            GameManager.Instance.apiHandler.UpdateStoryPostStatus(activityModel.id, statusIndex, (status, message) =>
            {

                if (status)
                {
                    activitiesView.OnStatusApplied(activityModel);
                    OnClose?.Invoke(statusIndex);
                    gameObject.SetActive(false);
                }
            });
        }
        else {
            OnClose?.Invoke(statusIndex);
            gameObject.SetActive(false);
        }
    } 
}

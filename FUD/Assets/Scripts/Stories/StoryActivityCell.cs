using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StoryActivityCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    //public Image storyImage;

    public TextMeshProUGUI commentText;


    StoryActivityModel activityModel;


    public void Load(StoryActivityModel activityModel)
    {
        this.activityModel = activityModel;

        titleText.text = activityModel.title;
    }

    public void OnAcceptButtonAction()
    {
        GameManager.Instance.apiHandler.UpdateStoryPostStatus(activityModel.id, 3, (status, message) => {

            if (status)
            {
                Destroy(gameObject);
            }
        });
    }

    public void OnRejectButtonAction()
    {
        GameManager.Instance.apiHandler.UpdateStoryPostStatus(activityModel.id, 8, (status, message) => {

            if (status)
            {
                Destroy(gameObject);
            }
        });
    }
}

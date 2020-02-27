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

    public GameObject acceptObject;


    StoryActivityModel activityModel;

    int currentUserId;


    public void Load(StoryActivityModel activityModel)
    {
        this.activityModel = activityModel;

        titleText.text = activityModel.title;

        currentUserId = DataManager.Instance.userInfo.id;
        Debug.Log("currentUserId = " + currentUserId);

        Debug.Log("user_id = " + activityModel.user_id);

        acceptObject.SetActive(44 != activityModel.user_id);
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

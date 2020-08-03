using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StoryActivityCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    //public Image storyImage;

    public TextMeshProUGUI descriptionText;    


    StoryActivityPopUp activityPopUp;

    StoryActivityModel activityModel;

    int currentUserId;


    public void Load(StoryActivityModel activityModel, StoryActivityPopUp activityPopUp)
    {
        this.activityModel = activityModel;

        this.activityPopUp = activityPopUp;

        titleText.text = activityModel.title;

        descriptionText.text = activityModel.StoryVersions.description;
    }

    public void OnTapAction()
    {
        activityPopUp.Load(activityModel, OnPopUpClose);
    }

    void OnPopUpClose(int updatedStatus)
    {
        switch (updatedStatus)
        {
            case 3:
            case 5:
            case 8:
                Destroy(gameObject);
                break;
        }
    }
}

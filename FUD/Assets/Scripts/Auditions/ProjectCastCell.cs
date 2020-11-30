using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class ProjectCastCell : MonoBehaviour
{
    public Image icon;
    public Image statusImage;
    public TMP_Text titleText;
    public TMP_Text genderText;
    public TMP_Text descriptionText;

    public TMP_Text statusText;

        
    Action<ProjectCast> OnTapAction;

    ProjectCast castData;


    public void SetView(ProjectCast cast, Action<ProjectCast> OnTapAction)
    {
        castData = cast;
        this.OnTapAction = OnTapAction;

        if (castData != null)
        {
            titleText.text = castData.StoryCharacters.title;
            descriptionText.text = castData.StoryCharacters.description;
            genderText.text = castData.StoryCharacters.gender;

            UpdateStatusTag();
        }
    }

    public void OnClickAction()
    {
        OnTapAction?.Invoke(castData);
    }

    void UpdateStatusTag()
    {
        if (castData.cast_status == 1)
        {
            castData.cast_status = 0;
        }

        EStatusType statusType = (EStatusType)castData.cast_status;

        statusImage.sprite = Resources.Load<Sprite>("Images/StatusTags/" + statusType);

        statusText.text = GetStatusText(castData.cast_status);
    }

    string GetStatusText(int castStatus)
    {
        string statusMsg = string.Empty;

        switch (castStatus)
        {
            case 0:
                statusMsg = "Go for Audition";
                break;

            case 1:
                statusMsg = "Send Offer";
                break;

            case 3:
                statusMsg = "Offer Sent";
                break;

            case 5:
                statusMsg = "Selected";
                break;

            case 8:
                statusMsg = "Declined";
                break;
        }

        return statusMsg;
    }
}

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

        statusText.text = statusType.ToString();
    }
}

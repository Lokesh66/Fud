using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    int index = 0;

    public void SetView(int index, ProjectCast cast, Action<ProjectCast> OnTapAction)
    {
        castData = cast;
        this.OnTapAction = OnTapAction;
        this.index = index;

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
        EStatusType statusType = (EStatusType)castData.cast_status;

        statusImage.sprite = Resources.Load<Sprite>("Images/StatusTags/" + statusType);

        statusText.text = statusType.ToString();
    }
}

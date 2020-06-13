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

    public Sprite reviewSprite;
    public Sprite shortListSprite;
    public Sprite rejectSprite;
        
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
        switch (castData.cast_status)
        {
            case 3:
            case 5:
                statusImage.sprite = shortListSprite;
                break;

            case 0:
            case 8:
                statusImage.sprite = rejectSprite;
                break;

            default:
                statusImage.sprite = reviewSprite;
                break;
        }
    }
}

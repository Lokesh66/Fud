using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


public class ProjectCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI directorName;
    public TextMeshProUGUI descriptionText;

    private CreatedProjectModel projectModel;

    Action<CreatedProjectModel> OnClickAction;


    public void SetView(CreatedProjectModel model, Action<CreatedProjectModel> OnProjectSelected)
    {
        projectModel = model;

        OnClickAction = OnProjectSelected;

        List<StoryCharacterModel> charactersList = model.StoryCharacters;

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        titleText.text = projectModel.title;

        descriptionText.text = projectModel.description;

        if (charactersList.Count > 0)
        {
            directorName.text = charactersList[0].Craftroles.name + " : " + charactersList[0].UserInfo.name;
        }
    }

    public void OnButtonAction()
    {
        //Need to call API for list of auditions for this projectby sending Project Id

        OnClickAction?.Invoke(projectModel);
    }
}

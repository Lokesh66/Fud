using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class ProjectCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI releaseDateText;
    public TextMeshProUGUI descriptionText;

    private Project projectModel;

    Action<Project> OnClickAction;


    public void SetView(Project model, Action<Project> OnProjectSelected)
    {
        projectModel = model;

        OnClickAction = OnProjectSelected;

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        titleText.text = projectModel.title;

        descriptionText.text = projectModel.description;

        releaseDateText.text = "Release Date : " + DatePicker.Instance.GetDateString(dateTime.AddSeconds(projectModel.release_date));
    }

    public void OnButtonAction()
    {
        //Need to call API for list of auditions for this projectby sending Project Id

        OnClickAction?.Invoke(projectModel);
    }
}

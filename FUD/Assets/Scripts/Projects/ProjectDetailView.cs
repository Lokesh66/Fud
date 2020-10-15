using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ProjectDetailView : MonoBehaviour
{
    public UploadedFilesHandler filesHandler;


    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI costText;

    public TextMeshProUGUI startDateText;

    public TextMeshProUGUI releaseDateText;


    ProjectOfferedModel offeredModel;

    Project projectData;


    public void Load(ProjectOfferedModel offeredModel)
    {
        this.offeredModel = offeredModel;

        projectData = offeredModel.Projects;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        DateTime startDate = dateTime.AddSeconds(projectData.start_date);

        DateTime releaseDate = dateTime.AddSeconds(projectData.release_date);

        titleText.text = projectData.title;

        descriptionText.text = offeredModel.Project_cast.StoryCharacters.description;

        costText.text = projectData.cost_estimation.ToString();

        startDateText.text = DatePicker.Instance.GetDateString(startDate);

        releaseDateText.text = DatePicker.Instance.GetDateString(releaseDate);
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

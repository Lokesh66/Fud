using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ProjectOfferedCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI genreText;
    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI budgetText;


    private ProjectOfferedModel projectModel;

    System.Action<ProjectOfferedModel> OnClickAction;

    public void SetView(ProjectOfferedModel model, System.Action<ProjectOfferedModel> OnProjectSelected)
    {
        projectModel = model;

        OnClickAction = OnProjectSelected;

        List<Genre> genres = DataManager.Instance.genres;

        Genre selectedGenre = genres.Find(item => item.id == projectModel.role);

        titleText.text = projectModel.Projects.title;

        genreText.text = selectedGenre.name;

        descriptionText.text = projectModel.Projects.description;

        budgetText.text = "Budget : " + projectModel.Projects.cost_estimation.ToString();
    }

    public void OnButtonAction()
    {
        //Need to call API for list of auditions for this projectby sending Project Id

        OnClickAction?.Invoke(projectModel);
    }
}

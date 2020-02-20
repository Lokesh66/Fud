using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ProjectCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI genreText;
    public TextMeshProUGUI durationText;
    public TextMeshProUGUI descriptionText;

    public Image projectIcon;

    private Project projectModel;

    System.Action<Project> OnClickAction;
    public void SetView(Project model, System.Action<Project> OnProjectSelected)
    {
        projectModel = model;
        OnClickAction = OnProjectSelected;

        titleText.text = projectModel.title;
    }

    public void OnButtonAction()
    {
        //Need to call API for list of auditions for this projectby sending Project Id

        OnClickAction?.Invoke(projectModel);
    }
}

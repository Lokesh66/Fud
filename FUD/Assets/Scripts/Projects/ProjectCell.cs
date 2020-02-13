using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ProjectCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public Image projectIcon;

    private ProjectDataModel projectModel;

    System.Action<ProjectDataModel> OnClickAction;
    public void SetView(ProjectDataModel model, System.Action<ProjectDataModel> OnProjectSelected)
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

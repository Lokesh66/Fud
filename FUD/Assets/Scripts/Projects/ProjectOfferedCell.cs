using frame8.ScrollRectItemsAdapter.MultiplePrefabsExample;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ProjectOfferedCell : MonoBehaviour
{
    public RemoteImageBehaviour profileImage;

    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI workingDaysText;
    public TextMeshProUGUI descriptionText;


    private ProjectOfferedModel projectModel;

    System.Action<ProjectOfferedModel> OnClickAction;

    public void SetView(ProjectOfferedModel model, System.Action<ProjectOfferedModel> OnProjectSelected)
    {
        projectModel = model;

        OnClickAction = OnProjectSelected;

        titleText.text = projectModel.Project_cast.StoryCharacters.title;

        descriptionText.text = projectModel.Project_cast.StoryCharacters.description;

        profileImage.Load(model.Projects?.Users?.profile_image);

        if (projectModel.Project_cast.StoryCharacters.estimated_working_days != null)
        {
            workingDaysText.text = ((int)projectModel.Project_cast.StoryCharacters.estimated_working_days).ToString();
        }
    }

    public void OnButtonAction()
    {
        //Need to call API for list of auditions for this projectby sending Project Id

        OnClickAction?.Invoke(projectModel);
    }
}

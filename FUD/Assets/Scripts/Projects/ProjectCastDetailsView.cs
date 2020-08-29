using frame8.ScrollRectItemsAdapter.MultiplePrefabsExample;
using UnityEngine;
using TMPro;


public class ProjectCastDetailsView : MonoBehaviour
{
    public RemoteImageBehaviour remoteImageBehaviour;

    public TextMeshProUGUI roleText;

    public TextMeshProUGUI roleCategoryText;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI userNameText;


    public void Load(ProjectCast projectCast)
    {
        gameObject.SetActive(true);

        remoteImageBehaviour.Load(projectCast.StoryCharacters.UserInfo.profile_image);

        roleText.text = projectCast.StoryCharacters.Craftroles.name;

        roleCategoryText.text = projectCast.StoryCharacters.RoleCategories.name;

        titleText.text = projectCast.StoryCharacters.title;

        descriptionText.text = projectCast.StoryCharacters.description;

        userNameText.text = projectCast.StoryCharacters.UserInfo.name;
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

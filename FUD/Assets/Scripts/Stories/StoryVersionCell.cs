using UnityEngine;
using TMPro;

public class StoryVersionCell : MonoBehaviour
{
    public RectTransform content;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    public GameObject shareCell;

    StoryVersion versionModel;


    public GameObject sharePanel;

    StoryVersionsView versionsView;

    public void SetView(StoryVersion versionModel, StoryVersionsView versionsView)
    {
        this.versionModel = versionModel;

        this.versionsView = versionsView;

        titleText.text = "title";

        description.text = versionModel.description;
    }

    public void OnButtonAction()
    {
        GameManager.Instance.apiHandler.GetStoryVersionDetails(versionModel.story_id, (status, response) => {


        });
    }


    public void OnShareButtonAction()
    {
        versionsView.OnCellButtonAction(versionModel);
    }
}

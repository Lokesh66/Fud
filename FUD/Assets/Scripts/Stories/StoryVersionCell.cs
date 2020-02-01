using UnityEngine;
using TMPro;

public class StoryVersionCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    StoryVersion versionModel;


    public void SetView(StoryVersion versionModel)
    {
        this.versionModel = versionModel;

        titleText.text = "title";

        description.text = versionModel.description;
    }

    public void OnButtonAction()
    {
        GameManager.Instance.apiHandler.GetStoryVersionDetails(versionModel.story_id, (status, response) => { 
            

        });
    }
}

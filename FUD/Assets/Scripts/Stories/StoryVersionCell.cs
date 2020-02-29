using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using DG.Tweening;

public class StoryVersionCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    StoryVersion versionModel;


    StoryVersionsView versionsView;

    Vector2 startPoint;

    public void SetView(StoryVersion versionModel, StoryVersionsView versionsView)
    {
        this.versionModel = versionModel;

        this.versionsView = versionsView;

        description.text = versionModel.description;
    }

    public void OnButtonAction()
    {
        versionsView.OnCellButtonAction(versionModel);
    }


    public void OnShareButtonAction()
    {
        versionsView.OnCellButtonAction(versionModel);
    }

    public void OnDeleteButtonAction()
    { 
        
    }

    public void OnUpdateButtonAction()
    {
        
    }
}

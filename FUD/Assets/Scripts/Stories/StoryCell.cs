using UnityEngine;
using System;
using TMPro;

public class StoryCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    StoryModel storyModel;

    Action<object> OnTapActon;

    public void SetView(StoryModel storyModel, Action<object> tapAction = null)
    {
        this.storyModel = storyModel;

        this.OnTapActon = tapAction;

        titleText.text = storyModel.title;

        description.text = storyModel.description;
    }

    public void OnButtonAction()
    {
        OnTapActon?.Invoke(storyModel.id);
    }
}

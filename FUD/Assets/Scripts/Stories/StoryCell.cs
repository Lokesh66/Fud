using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

public class StoryCell : MonoBehaviour
{
    public RectTransform editStoryTrans;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    public GameObject updateStoryCache;

    public SwipeButtonsHelper swipeHelper;

    StoryModel storyModel;

    Action<object> OnTapActon;

    ScrollRect scrollRect;

    Vector2 startPoint;

    public void SetView(StoryModel storyModel, Action<object> tapAction = null)
    {
        this.storyModel = storyModel;

        this.OnTapActon = tapAction;

        swipeHelper.cellButtonAction = OnButtonAction;

        titleText.text = storyModel.title;

        description.text = storyModel.description;
    }

    public void OnButtonAction()
    {
        OnTapActon?.Invoke(storyModel.id);
    }

    public void OnUpdateButtonAction()
    {
        Transform parent = StoryDetailsController.Instance.transform;

        GameObject createObject = Instantiate(updateStoryCache, parent);

        createObject.GetComponent<StoryUpdateView>().Load();
    }
}

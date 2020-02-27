using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;


public class StoryCell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform editStoryTrans;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI description;

    public GameObject updateStoryCache;

    StoryModel storyModel;

    Action<object> OnTapActon;

    Vector2 startPoint;

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

    public void OnUpdateButtonAction()
    {
        Transform parent = StoryDetailsController.Instance.transform;

        GameObject createObject = Instantiate(updateStoryCache, parent);

        createObject.GetComponent<StoryUpdateView>().Load();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("startPoint.x = " + startPoint.x);

        Debug.Log("eventData.x = " + eventData.position.x);

        if (startPoint.x - eventData.position.x > 0)
        {
            //float targetValue = editStoryTrans.anchoredPosition.x - editStoryTrans.anchoredPosition.x;

            editStoryTrans.DOAnchorPosX(0.0f, 0.4f);
        }
        else {
            //float targetValue = editStoryTrans.anchoredPosition.x + editStoryTrans.anchoredPosition.x;

            editStoryTrans.DOAnchorPosX(editStoryTrans.sizeDelta.x, 0.4f);
        }
    }
}

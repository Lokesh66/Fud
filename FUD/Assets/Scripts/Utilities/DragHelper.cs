using UnityEngine.EventSystems;
using UnityEngine;
using System;


public class DragHelper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public RectTransform rectTransform;

    Vector2 currentPosition;

    Action OnDragging;


    public void Init(Action OnDragging)
    {
        this.OnDragging = OnDragging;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.position.y == currentPosition.y)
        {
            rectTransform.position = currentPosition;

            OnDragging?.Invoke();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}

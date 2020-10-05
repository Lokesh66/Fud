using UnityEngine.EventSystems;
using UnityEngine;
using System;


public class DragHelper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    bool isDragged = false;

  
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    public void ResetIsDragged()
    {
        isDragged = false;
    }

    public bool IsDragged()
    {
        return isDragged;
    }
}

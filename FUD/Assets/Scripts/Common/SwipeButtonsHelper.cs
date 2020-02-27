using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SwipeButtonsHelper : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform buttonsPanel;

    public bool isLeftSwipe;

    Vector2 startPoint;

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPoint = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLeftSwipe)
        {
            //Left Swipe
            if (startPoint.x - eventData.position.x > 0)
            {
                buttonsPanel.DOAnchorPosX(0.0f, 0.4f);
            }
            else
            {
                buttonsPanel.DOAnchorPosX(buttonsPanel.sizeDelta.x, 0.4f);
            }
        }
        else
        {
            //Right Swipe
            Debug.Log("startPoint.x - eventData.position.x = " + (startPoint.x - eventData.position.x));

            if (startPoint.x - eventData.position.x < -40)
            {
                buttonsPanel.DOAnchorPosX(-500, 0.4f);
            }
            else if(startPoint.x - eventData.position.x > 40)
            {
                buttonsPanel.DOAnchorPosX(0, 0.4f);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SwipeButtonsHelper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform buttonsPanel;

    public System.Action cellButtonAction;

    public bool isLeftSwipe;

    Vector2 startPoint;


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown Called");
        // Do action
        startPoint = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (startPoint.Equals(eventData.position))
        {
            cellButtonAction?.Invoke();
        }
        else
        {
            Debug.Log("OnPointerUp Called");
            if (isLeftSwipe)
            {
                //Left Swipe
                if (startPoint.x - eventData.position.x > 0)
                {
                    buttonsPanel.DOAnchorPosX(-buttonsPanel.sizeDelta.x, 0.4f);
                }
                else
                {
                    buttonsPanel.DOAnchorPosX(0, 0.4f);
                }
            }
            else
            {
                //Right Swipe
                Debug.Log("startPoint.x - eventData.position.x = " + (startPoint.x - eventData.position.x));

                if (startPoint.x - eventData.position.x < -4)
                {
                    buttonsPanel.DOAnchorPosX(500, 0.4f);
                }
                else if (startPoint.x - eventData.position.x > 4)
                {
                    buttonsPanel.DOAnchorPosX(0, 0.4f);
                }
            }
        }
    }
}

using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

public class SwipeButtonsHelper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform buttonsPanel;

    public System.Action cellButtonAction;

    public bool isLeftSwipe;

    Vector2 startPoint;


    public void OnPointerDown(PointerEventData eventData)
    {
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

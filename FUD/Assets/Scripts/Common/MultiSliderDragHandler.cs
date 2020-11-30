using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class MultiSliderDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform minFillTrans;

    public RectTransform minSliderTrans;

    public Slider ageSlider;

    public TextMeshProUGUI ageValueText;

    [HideInInspector]
    public float minAgeValue;


    float maxMinAgePos = 900.0f;

    float minAgePos = 100.0f;

    float minCurrentPosX = 100.0f;

    float maxCurrentPosX;



    public void OnBeginDrag(PointerEventData eventData)
    {
        maxCurrentPosX = ageSlider.value > 0 ? ageSlider.handleRect.anchorMin.x * (maxMinAgePos - minAgePos) : 100.0f;

        maxCurrentPosX += 100.0f;
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.position.x >= minAgePos && data.position.x <= maxCurrentPosX)
        {
            UpdateSliderView(data.position.x);
        }
        else if (data.position.x >= maxCurrentPosX)
        {
            UpdateSliderView(maxCurrentPosX);
        }
        else if (data.position.x <= minAgePos)
        {
            UpdateSliderView(minAgePos);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    void UpdateSliderView(float currentPosX)
    {
        minCurrentPosX = currentPosX;

        minAgeValue = (minCurrentPosX - 100) / (maxMinAgePos - minAgePos) * 100;

        minFillTrans.anchoredPosition = new Vector2(minCurrentPosX - 100, minFillTrans.anchoredPosition.y);

        minSliderTrans.sizeDelta = new Vector2(minFillTrans.anchoredPosition.x, minSliderTrans.sizeDelta.y);

        ageValueText.text = (int)minAgeValue + "-" + (int)ageSlider.value + " Yrs";
    }

    public void ClearData()
    {
        minCurrentPosX = 100;

        minAgeValue = 0;

        minFillTrans.anchoredPosition = new Vector2(minCurrentPosX, minFillTrans.anchoredPosition.y);
    }
}

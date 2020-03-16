using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public RectTransform backGroundTrans;

    public TextMeshProUGUI dialogueText;


    Vector2 topLeftAnchor = new Vector2(0, 1);

    Vector2 topRightAnchor = new Vector2(1, 1);

    float padding = 40.0f;


    public void SetView(string message, bool isLeftAlign)
    {
        dialogueText.text = message;

        StartCoroutine(UpdateBackGround(isLeftAlign)); 
    }

    IEnumerator UpdateBackGround(bool isLeftAlign)
    {
        yield return new WaitForEndOfFrame();

        Vector2 bgSize = new Vector2(backGroundTrans.rect.width, dialogueText.rectTransform.rect.height + padding);

        backGroundTrans.sizeDelta = bgSize;

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, bgSize.y);

        Vector2 anchorPoint = isLeftAlign ? topLeftAnchor : topRightAnchor;

        backGroundTrans.anchorMin = anchorPoint;

        backGroundTrans.anchorMax = anchorPoint;

        backGroundTrans.pivot = anchorPoint;
    }
}

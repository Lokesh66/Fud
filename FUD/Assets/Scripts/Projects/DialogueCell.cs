using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class DialogueCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public RectTransform backGroundTrans;

    public TextMeshProUGUI dialogueText;

    public Button cellButton;

    [HideInInspector]
    public UserSearchModel userSearchModel;

    [HideInInspector]
    public Dictionary<string, object> dialogueModel = new Dictionary<string, object>();

    [HideInInspector]
    public bool isLeftAlign;

    [HideInInspector]
    public int dialogueId;


    Vector2 topLeftAnchor = new Vector2(0, 1);

    Vector2 topRightAnchor = new Vector2(1, 1);

    float padding = 40.0f;

    Action<DialogueCell> OnCellButtonAction;

    public void SetView(string message, bool isLeftAlign, UserSearchModel userSearchModel, Action<DialogueCell> OnCellButtonAction, int dialogueId = -1)
    {
        dialogueText.text = message;

        this.isLeftAlign = isLeftAlign;

        this.OnCellButtonAction = OnCellButtonAction;

        this.userSearchModel = userSearchModel;

        this.dialogueId = dialogueId; 

        cellButton.enabled = OnCellButtonAction != null;

        if (dialogueId != -1)
        {
            dialogueModel["id"] = dialogueId;
        }

        dialogueModel["character_id"] = userSearchModel.id;

        dialogueModel["dailogue"] = message;

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

    public void OnButtonAction()
    {
        OnCellButtonAction?.Invoke(this);
    }
}

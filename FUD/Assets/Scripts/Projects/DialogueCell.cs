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

    public RectTransform deleteButtonTrans;

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

    float xLeftdeleteButtonPos = 650f;

    float xRightdeleteButtonPos = 300f;

    Action<DialogueCell> OnCellButtonAction;

    Action<DialogueCell> OnDeleteAction;


    public void SetView(string message, bool isLeftAlign, UserSearchModel userSearchModel, Action<DialogueCell> OnCellButtonAction, Action<DialogueCell> OnDeleteButtonAction, int dialogueId = -1)
    {
        dialogueText.text = message;

        this.isLeftAlign = isLeftAlign;

        this.OnCellButtonAction = OnCellButtonAction;

        this.OnDeleteAction = OnDeleteButtonAction;

        this.userSearchModel = userSearchModel;

        this.dialogueId = dialogueId; 

        cellButton.enabled = OnCellButtonAction != null;

        if (OnCellButtonAction != null)
        {
            if (dialogueId != -1)
            {
                dialogueModel["id"] = dialogueId;
            }

            dialogueModel["character_id"] = userSearchModel.id;

            dialogueModel["dailogue"] = message;

            SetDeleteButtonPosition();
        }

        deleteButtonTrans.gameObject.SetActive(OnCellButtonAction != null);

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

    void SetDeleteButtonPosition()
    {
        float xTargetValue = isLeftAlign ? xLeftdeleteButtonPos : xRightdeleteButtonPos;

        deleteButtonTrans.anchoredPosition = new Vector2(xTargetValue, 0);
    }

    public void OnButtonAction()
    {
        OnCellButtonAction?.Invoke(this);
    }

    public void OnDeleteButtonAction()
    {
        OnDeleteAction?.Invoke(this);
    }
}

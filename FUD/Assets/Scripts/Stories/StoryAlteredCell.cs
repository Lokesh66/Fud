using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;


public class StoryAlteredCell : MonoBehaviour
{
    public RectTransform rectTransform;

    public TextMeshProUGUI titleText;

    public Image statusTag;

    public TextMeshProUGUI statusText;

    [Space]

    public Sprite reviewTag;

    public Sprite deleteTag;

    public Sprite shortListTag;

    [Space]


    StoryAlteredModel alteredModel;

    public Action<StoryAlteredModel> OnTapActon;

    bool isOwnStory = false;


    public void SetAlteredView(StoryAlteredModel alteredModel, Action<StoryAlteredModel> tapAction)
    {
        this.OnTapActon = tapAction;

        this.alteredModel = alteredModel;

        titleText.text = alteredModel.title;

        isOwnStory = DataManager.Instance.userInfo.id == alteredModel.user_id;

        UpdateStatusTag();
    }

    public void OnButtonAction()
    {
        OnTapActon?.Invoke(alteredModel);
    }

    void UpdateStatusTag()
    {
        int statusValue = isOwnStory ? alteredModel.sender_status : alteredModel.reciever_status;

        EStatusType statusType = (EStatusType)statusValue;

        statusTag.sprite = Resources.Load<Sprite>("Images/StatusTags/" + statusType);

        statusText.text = statusType.ToString();
    }
}

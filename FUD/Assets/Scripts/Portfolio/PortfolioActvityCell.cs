using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class PortfolioActvityCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public Image statusTag;

    public TextMeshProUGUI statusText;

    //public Image storyImage;


    PortfolioActivityPopUp activityPopUp;

    PortfolioActivityModel activityModel;

    ETabType tabType;


    bool isOwnAlbum = false;

    int currentUserId;


    public void Load(PortfolioActivityModel model, PortfolioActivityPopUp activityPopUp, ETabType tabType)
    {
        this.activityModel = model;

        this.activityPopUp = activityPopUp;

        this.tabType = tabType;

        titleText.text = model.Portfolio.title;

        descriptionText.text = model.Portfolio.description;

        statusTag.gameObject.SetActive(tabType == ETabType.Altered);

        if (tabType == ETabType.Altered)
        {
            UpdateStatusTag();
        }
    }

    void OnPopUpClose(int updatedStatus)
    {
        switch (updatedStatus)
        {
            case 1:
            case 8:
                Destroy(gameObject);
                break;
        }
    }

    public void OnTapAction()
    { 
        activityPopUp?.Load(activityModel, OnPopUpClose, tabType);
    }

    void UpdateStatusTag()
    {
        int statusValue = isOwnAlbum ? activityModel.sender_status : activityModel.reciever_status;

        EStatusType statusType = (EStatusType)statusValue;

        statusTag.sprite = Resources.Load<Sprite>("Images/StatusTags/" + statusType);

        statusText.text = statusType.ToString();
    }
}

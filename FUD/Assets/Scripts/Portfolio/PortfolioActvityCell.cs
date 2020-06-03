using UnityEngine;
using TMPro;

public class PortfolioActvityCell : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    //public Image storyImage;

    public TextMeshProUGUI commentText;


    PortfolioActivityPopUp activityPopUp;

    PortfolioActivityModel activityModel;

    ETabType tabType;


    int currentUserId;


    public void Load(PortfolioActivityModel model, PortfolioActivityPopUp activityPopUp, ETabType tabType)
    {
        this.activityModel = model;

        this.activityPopUp = activityPopUp;

        this.tabType = tabType;

        titleText.text = model.Portfolio.title;

        commentText.text = model.comments;

        descriptionText.text = model.Portfolio.description;
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
        if (tabType == ETabType.Offers)
        {
            activityPopUp?.Load(activityModel, OnPopUpClose, tabType);
        }
    }
}

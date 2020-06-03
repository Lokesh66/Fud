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

    int currentUserId;


    public void Load(PortfolioActivityModel model, PortfolioActivityPopUp activityPopUp)
    {
        this.activityModel = model;

        this.activityPopUp = activityPopUp;

        titleText.text = model.Portfolio.title;

        commentText.text = model.comments;

        descriptionText.text = model.Portfolio.description;
    }

    void OnPopUpClose(string updatedStatus)
    {
        switch (updatedStatus)
        {
            case "liked":
            case "rejected":
                Destroy(gameObject);
                break;
        }
    }

    public void OnTapAction()
    {
        activityPopUp?.Load(activityModel, OnPopUpClose);
    }
}

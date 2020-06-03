using System;
using TMPro;
using UnityEngine;

public class PortfolioActivityPopUp : MonoBehaviour
{
    //public Image storyImage;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI commentText;

    public TextMeshProUGUI descriptionText;

    public GameObject acceptObject;


    PortfolioActivityModel activityModel;

    int userStatus = -1;

    Action<int> OnClose;

    int currentUserId;

    ETabType tabType;


    public void Load(PortfolioActivityModel activityModel, Action<int> OnClose, ETabType tabType)
    {
        gameObject.SetActive(true);

        this.activityModel = activityModel;

        this.OnClose = OnClose;

        this.tabType = tabType;

        //titleText.text = activityModel.Portfolio.title;

        //commentText.text = activityModel.comments;

        //descriptionText.text = activityModel.Portfolio.description;

        currentUserId = DataManager.Instance.userInfo.id;

        acceptObject.SetActive(currentUserId != activityModel.user_id);
    }

    public void OnStatusButtonAction(int userStatus)
    {
        if (userStatus != -1)
        {
            GameManager.Instance.apiHandler.UpdatePortfolioPostStatus(activityModel.id, userStatus, (status, response) =>
            {
                this.userStatus = userStatus;

                CreateAlert(status);
            });
        }
        else
        {
            OnClose?.Invoke(userStatus);
            gameObject.SetActive(false);
        }
    }

    void CreateAlert(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Status updation success" : "Something went wrong, please try again";

        alertModel.okayButtonAction = OnCloseButton;


        if (status)
        {
            gameObject.SetActive(false);

            alertModel.canEnableTick = true;
        }

        UIManager.Instance.ShowAlert(alertModel);
    }

    void OnCloseButton()
    {
        OnClose?.Invoke(userStatus);
    }
}

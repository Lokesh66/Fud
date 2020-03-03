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

    string statusMessage = string.Empty;

    Action<string> OnClose;

    int currentUserId;

    public void Load(PortfolioActivityModel activityModel, Action<string> OnClose)
    {
        gameObject.SetActive(true);

        this.activityModel = activityModel;

        this.OnClose = OnClose;

        titleText.text = activityModel.Portfolio.title;

        commentText.text = activityModel.comments;

        descriptionText.text = activityModel.Portfolio.description;

        currentUserId = DataManager.Instance.userInfo.id;

        acceptObject.SetActive(currentUserId != activityModel.user_id);
    }

    public void OnStatusButtonAction(string statusMessage)
    {
        if (!string.IsNullOrEmpty(statusMessage))
        {
            GameManager.Instance.apiHandler.UpdatePortfolioPostStatus(activityModel.id, statusMessage, (status, response) =>
            {
                this.statusMessage = statusMessage;

                CreateAlert(status);
            });
        }
        else
        {
            OnClose?.Invoke(statusMessage);
            gameObject.SetActive(false);
        }
    }

    void CreateAlert(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Status updation success" : "Something went wrong, please try again";

        alertModel.okayButtonAction = OnCloseButton;

        CanvasManager.Instance.alertView.ShowAlert(alertModel);

        if (status)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCloseButton()
    {
        OnClose?.Invoke(statusMessage);
    }
}

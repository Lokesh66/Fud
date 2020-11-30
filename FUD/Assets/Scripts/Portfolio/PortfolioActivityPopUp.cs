using UnityEngine;
using System;


public class PortfolioActivityPopUp : MonoBehaviour
{
    public GameObject acceptObject;

    public GameObject likeObject;

    public GameObject shortListObject;

    public GameObject rejectObject;


    public PortfolioOfferedDetailView detailsView;


    PortfolioActivityModel activityModel;

    int userStatus = -1;

    Action<int> OnClose;

    bool isOwnStory = false;

    int currentUserId;


    public void Load(PortfolioActivityModel activityModel, Action<int> OnClose, ETabType tabType)
    {
        gameObject.SetActive(true);

        this.activityModel = activityModel;

        this.OnClose = OnClose;

        if (tabType == ETabType.Offers)
        {
            SetOfferedView();
        }
        else {
            SetAlteredView();
        }
    }

    void SetOfferedView()
    {
        shortListObject.SetActive(true);

        acceptObject.SetActive(true);

        rejectObject.SetActive(true);
    }

    void SetAlteredView()
    {
        currentUserId = DataManager.Instance.userInfo.id;

        isOwnStory = currentUserId == activityModel.user_id;

        int statusValue = isOwnStory ? activityModel.sender_status : activityModel.reciever_status;

        EStatusType statusType = (EStatusType)statusValue;

        shortListObject.SetActive(false);

        acceptObject.SetActive(statusType != EStatusType.Accepted && statusType != EStatusType.Rejected);

        rejectObject.SetActive(statusType != EStatusType.Rejected);
    }

    public void OnViewButtonAction()
    {
        detailsView.Load(activityModel);

        gameObject.SetActive(false);
    }

    public void OnStatusButtonAction(int userStatus)
    {
        if (userStatus != -1)
        {
            GameManager.Instance.apiHandler.UpdatePortfolioPostStatus(activityModel.id, userStatus, (status, response) =>
            {
                this.userStatus = userStatus;

                CreateAlert(status, response);
            });
        }
        else
        {
            OnClose?.Invoke(userStatus);
            gameObject.SetActive(false);
        }
    }

    void CreateAlert(bool status, string response)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse baseResponse = JsonUtility.FromJson<BaseResponse>(response);

        alertModel.message = status ? "Status updation success" : baseResponse.message;

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

using System;
using TMPro;
using UnityEngine;

public class PortfolioActivityPopUp : MonoBehaviour
{
    //public Image storyImage;


    public GameObject acceptObject;

    public GameObject likeObject;

    public GameObject shortListObject;


    public PortfolioOfferedDetailView detailsView;


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

        currentUserId = DataManager.Instance.userInfo.id;

        likeObject.SetActive(tabType == ETabType.Offers);

        acceptObject.SetActive(tabType == ETabType.Offers);

        shortListObject.SetActive(tabType == ETabType.Offers);

        acceptObject.SetActive(currentUserId != activityModel.user_id);

        shortListObject.SetActive(currentUserId != activityModel.user_id);
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

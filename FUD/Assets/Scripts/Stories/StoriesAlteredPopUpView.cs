using System.Collections.Generic;
using UnityEngine;


public class StoriesAlteredPopUpView : MonoBehaviour
{
    public GameObject viewObject;

    public GameObject acceptObject;

    public GameObject shortListObject;

    public GameObject rejectObject;

    public GameObject shareObject;


    public StoryShareView shareStoryVersionView;

    public VersionMultimediaView multimediaView;

    public StoriesHistoryView historyView;


    StoryAlteredModel alteredModel;

    bool isOwnStory = false;


    public void Load(StoryAlteredModel alteredModel)
    {
        this.alteredModel = alteredModel;

        gameObject.SetActive(true);

        isOwnStory = DataManager.Instance.userInfo.id == alteredModel.user_id;

        SetView();
    }

    void SetView()
    {
        int statusValue = isOwnStory ? alteredModel.sender_status : alteredModel.reciever_status;

        EStatusType statusType = (EStatusType)statusValue;

        shareObject.SetActive(statusType == EStatusType.Accepted && !isOwnStory);

        rejectObject.SetActive(!isOwnStory);

        acceptObject.SetActive(!isOwnStory && statusType != EStatusType.Accepted);

        shortListObject.SetActive(!isOwnStory && statusType != EStatusType.ShortListed);
    }


    public void OnViewButtonAction()
    {
        multimediaView.Load(alteredModel.StoryVersions);

        gameObject.SetActive(false);
    }

    public void OnShareButtonAction()
    {
        gameObject.SetActive(false);

        shareStoryVersionView.Load(alteredModel.StoryVersions, null);
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }

    public void OnHistoryButtonAction()
    {
        GameManager.Instance.apiHandler.GetStoryHistories(1, alteredModel.story_id, (status, response) => {

            if (status)
            {
                gameObject.SetActive(false);

                List<StoryHistoryModel> modelsList = JsonUtility.FromJson<StoryHistoryResponse>(response).data[0].StoryTrack;

                historyView.Load(modelsList);
            }
        });
    }

    public void OnSelectButtonAction(int statusIndex)
    {
        GameManager.Instance.apiHandler.UpdateStoryPostStatus(alteredModel.id, statusIndex, (status, response) =>
        {
            OnAPIResponse(status, response);
        });
    }

    void OnAPIResponse(bool status, string apiResponse)
    {
        AlertModel alertModel = new AlertModel();

        BaseResponse responseModel = JsonUtility.FromJson<BaseResponse>(apiResponse);

        alertModel.message = status ? "Status updation success" : responseModel.message;

        alertModel.okayButtonAction = OnCancelButtonAction;

        UIManager.Instance.ShowAlert(alertModel);
    }
}

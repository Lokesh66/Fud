using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoriesAlteredPopUpView : MonoBehaviour
{
    public GameObject viewObject;

    public GameObject acceptObject;

    public GameObject shortListObject;

    public GameObject rejectObject;


    public VersionMultimediaView multimediaView;


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
        shortListObject.SetActive(!isOwnStory);

        rejectObject.SetActive(!isOwnStory);

        acceptObject.SetActive(!isOwnStory);
    }


    public void OnViewButtonAction()
    {
        multimediaView.Load(alteredModel.StoryVersions);

        gameObject.SetActive(false);
    }

    public void OnCancelButtonAction()
    {
        gameObject.SetActive(false);
    }

    public void OnSelectButtonAction(int statusIndex)
    {
        GameManager.Instance.apiHandler.UpdateStoryPostStatus(alteredModel.id, statusIndex, (status, message) =>
        {
            if (status)
            {
                
            }
            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Status updation success" : "Something went wrong, please try again.";

        alertModel.okayButtonAction = OnCancelButtonAction;

        UIManager.Instance.ShowAlert(alertModel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VersionDetailsView : MonoBehaviour
{

    //public Image userImage;

    public TextMeshProUGUI description;

    public UpdateStoryVersionView updateVersionView;


    StoryVersion storyVersion;

    StoryVersionsView versionsView;


    public void Load(StoryVersion storyVersion, StoryVersionsView versionsView)
    {
        this.storyVersion = storyVersion;

        this.versionsView = versionsView;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        //description.text = storyVersion.description;
    }

    public void OnButtonAction(int buttonIndex)
    {
        gameObject.SetActive(false);

        switch (buttonIndex)
        {
            case 0:
                OnEditButtonAction();
                break;
            case 1:
                OnMediaButtonAction();
                break;
            case 2:
                OnShareButtonAction();
                break;
            case 3:
                OnDeleteButtonAction();
                break;
            case 4:
                OnCancelButtonAction();
                break;
        }
    }

    void OnCancelButtonAction()
    {
        Reset();

        gameObject.SetActive(false);
    }

    void OnEditButtonAction()
    {
        updateVersionView.Load(storyVersion, this);
    }

    void OnDeleteButtonAction()
    {
        int storyId = StoryDetailsController.Instance.GetStoryId();

        GameManager.Instance.apiHandler.RemoveStoryVersion(storyVersion.id, storyId, 8, (status) => {

            OnAPIResponse(status);
        });
    }

    void OnAPIResponse(bool status)
    {
        AlertModel alertModel = new AlertModel();

        alertModel.message = status ? "Story Version Removed Successfully" : "Something went wrong, please try again.";

        if (status)
        {
            alertModel.okayButtonAction = OnSuccessResponse;

            alertModel.canEnableTick = true;
        }

        CanvasManager.Instance.alertView.ShowAlert(alertModel);
    }

    void OnSuccessResponse()
    {
        Reset();

        versionsView.OnRemoveVersion(storyVersion);
    }

    void OnShareButtonAction()
    {
        if (DataManager.Instance.CanLoadScreen(EFeatureType.ShareStoryVersion))
        {
            versionsView.OnShareButtonAction(storyVersion);
        }
        else
        {
            UIManager.Instance.CreateUnAvaiableAlert(EFeatureType.ShareStoryVersion);
        }
    }

    void OnMediaButtonAction()
    {
        versionsView.OnMediaButtonAction(storyVersion);
    }

    void Reset()
    {
        //description.text = string.Empty;

        //userImage.sprite = null;
    }

    public void OnEditCallBack(StoryVersion storyVersion)
    {
        versionsView.UpdateStoryVersion(storyVersion);
    }
}

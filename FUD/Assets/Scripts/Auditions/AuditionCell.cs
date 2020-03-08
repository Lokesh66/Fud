using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AuditionCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text ageText;
/*    public TMP_Text otherText;
*/
    Audition auditionData;
    AuditionController auditionController;
    bool isJoined;
    bool isUpdated;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    public void SetView(AuditionController auditionController, Audition audition)
    {
        auditionData = audition;
        this.auditionController = auditionController;
        isJoined = auditionController.isJoined; ;
        isUpdated = false;
        if (auditionData != null)
        {
            titleText.text = auditionData.topic;
            ageText.text = auditionData.age_to.ToString();
        }
    }

    public void OnClickAction()
    {
        Debug.Log("OnClickAction ");
        AuditionJoinView.Instance.Load(auditionData, isJoined, (index) => {
            switch (index)
            {
                case 3:
                    GalleryButtonsPanel.Instance.Load(MediaButtonAction);
                    break;
                case 4:
                    WithDrawAudition();
                    break;
            }
        });
    }

    void Refresh()
    {
        auditionController?.GetAuditions();
    }

    void MediaButtonAction(int index)
    {
        EMediaType selectedType = (EMediaType)index;

        uploadedDict.Clear();

        switch (selectedType)
        {
            case EMediaType.Image: 
                GalleryManager.Instance.PickImages(OnImagesUploaded);
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery(OnVideosUploaded);
                break;
            case EMediaType.Audio:
                GalleryManager.Instance.GetAudiosFromGallery(OnAudiosUploaded);
                break;
        }
    }

    void OnImagesUploaded(bool status, List<string> imageUrls)
    {
        if (status)
        {
            for (int i = 0; i < imageUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", imageUrls[i]);

                kvp.Add("media_type", "image");

                uploadedDict.Add(kvp);
            }
            if (isJoined)
            {
                UpdateAudition();
            }
            else
            {
                JoinAudition();
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        }
    }

    void OnAudiosUploaded(bool status, List<string> audioUrls)
    {
        if (status)
        {
            for (int i = 0; i < audioUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", audioUrls[i]);

                kvp.Add("media_type", "audio");

                uploadedDict.Add(kvp);
            }
            if (isJoined)
            {
                UpdateAudition();
            }
            else
            {
                JoinAudition();
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        }
    }

    void OnVideosUploaded(bool status, List<string> videoUrls)
    {
        if (status)
        {
            for (int i = 0; i < videoUrls.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoUrls[i]);

                kvp.Add("media_type", "video");

                uploadedDict.Add(kvp);
            }
            if (isJoined)
            {
                UpdateAudition();
            }
            else
            {
                JoinAudition();
            }
        }
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            CanvasManager.Instance.alertView.ShowAlert(alertModel);
        }
    }

    void JoinAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("audition_id", auditionData.id);
        parameters.Add("port_album_media", uploadedDict);
        GameManager.Instance.apiHandler.JoinAudition(parameters, (status, response) => {
            if (status)
            {
                isUpdated = true;
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Joined Successfully";
                alertModel.okayButtonAction = Refresh;
                alertModel.canEnableTick = true;
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Joining Audition Failed";
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
        });
    }

    void UpdateAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("audition_id", auditionData.id);
        parameters.Add("id", auditionData.project_id);
        parameters.Add("user_id", DataManager.Instance.userInfo.id);
        parameters.Add("port_album_media", uploadedDict);

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) => {
            if (status)
            {
                isUpdated = true;
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Updated Successfully";
                alertModel.okayButtonAction = Refresh;
                alertModel.canEnableTick = true;
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Updating Audition Failed";
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }        
        });
    }

    void WithDrawAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("audition_id", auditionData.id);
        parameters.Add("id", auditionData.project_id);
        parameters.Add("user_id", DataManager.Instance.userInfo.id);
        parameters.Add("status", "inactive");

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) => {
            if (status)
            {
                isUpdated = true;
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Deleted Successfully";
                alertModel.okayButtonAction = Refresh;
                alertModel.canEnableTick = true;
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Deleting Audition Failed";
                CanvasManager.Instance.alertView.ShowAlert(alertModel);
            }
        });
    }

}

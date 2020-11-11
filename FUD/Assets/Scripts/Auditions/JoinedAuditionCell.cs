using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class JoinedAuditionCell : MonoBehaviour
{
    public Image icon;

    public TMP_Text titleText;

    public TMP_Text descriptionText;
    
    public TMP_Text statusText;

    public Sprite defaultSprite;

    public Image tagImage;

    JoinedAudition auditionData;

    AuditionAlteredView auditionController;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    private string mediaSource = "audition";


    public void SetView(AuditionAlteredView auditionController, JoinedAudition audition)
    {
        auditionData = audition;

        this.auditionController = auditionController;

        if (auditionData != null)
        {
            titleText.text = auditionData.Audition.title;

            descriptionText.text = auditionData.Audition.description;

            UpdateStatusTag();
        }
    }

    void UpdateStatusTag()
    {
        tagImage.gameObject.SetActive(true);

        bool isOwnAudition = DataManager.Instance.userInfo.id == auditionData.user_id;

        int statusValue = isOwnAudition ? auditionData.sender_status : auditionData.reciever_status;

        EStatusType statusType = (EStatusType)statusValue;

        tagImage.sprite = Resources.Load<Sprite>("Images/StatusTags/" + statusType);

        statusText.text = statusType.ToString();
    }

    public void OnClickAction()
    {
        AuditionJoinView.Instance.Load(true, (index) =>
        {
            switch (index)
            {
                case 8:
                case 3:
                case 5:
                    UpdateAuditionStatus(index);
                    break;

                case 9://View
                    auditionController.LoadAuditionDetails(auditionData.Audition);
                    break;

                case 10:
                    GalleryButtonsPanel.Instance.Load(MediaButtonAction);
                    break;
            }
        }, auditionData.GetAuditonStatus());
    }

    void Refresh()
    {
        auditionController?.GetAuditions();
    }

    void MediaButtonAction(int index)
    {
        EMediaType selectedType = (EMediaType)index;
        uploadedDict = new List<Dictionary<string, object>>();

        switch (selectedType)
        {
            case EMediaType.Image:
                GalleryManager.Instance.PickImages(mediaSource, OnImagesUploaded);
                break;
            case EMediaType.Video:
                GalleryManager.Instance.GetVideosFromGallery(mediaSource, OnVideosUploaded);
                break;
            case EMediaType.Audio:
                GalleryManager.Instance.GetAudiosFromGallery(mediaSource, OnAudiosUploaded);
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
            UpdateAudition();
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
            UpdateAudition();
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
            UpdateAudition();
        }
    }

    void UpdateAudition()
    {
        AlertMessage.Instance.SetText("AuditionCell/UpdateAudition");
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("id", auditionData.id);
        parameters.Add("port_album_media", uploadedDict);

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) =>
        {
            if (status)
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Updated Successfully";
                alertModel.okayButtonAction = Refresh;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Updating Audition Failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    void UpdateAuditionStatus(int statusIndex)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("id", auditionData.id);

        parameters.Add("audition_id", auditionData.audition_id);

        parameters.Add("status", statusIndex);

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) =>
        {
            if (status)
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Status updation success";
                alertModel.okayButtonAction = Refresh;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Status updation failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }
}


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;


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

    ETabType auditionType;

    int seconds;
    int minutes;
    int hours;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    private string mediaSource = "audition";


    public void SetView(AuditionAlteredView auditionController, JoinedAudition audition)
    {
        auditionData = audition;
        this.auditionController = auditionController;
        auditionType = auditionController.tabType;

        if (auditionData != null)
        {
            titleText.text = auditionData.Audition.title;

            descriptionText.text = auditionData.Audition.description;

            UpdateStatusTag();

            //StartCountDown();
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
        AlertMessage.Instance.SetText("OnClickAction : " + auditionData.id, false);
        Debug.Log("OnClickAction ");

        AuditionJoinView.Instance.Load(auditionData.Audition, true, (index) =>
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

                case 6:
                    RecordVideo();
                    break;
            }
        }, auditionData.GetAuditonStatus());
    }

    void RecordVideo()
    {
        NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
        {
            string fileName = Path.GetFileName(path);

            byte[] fileBytes = File.ReadAllBytes(path);

            titleText.text = fileBytes.Length.ToString();

            NativeGallery.SaveVideoToGallery(fileBytes, "Videos", fileName);

            GalleryManager.Instance.UploadVideoFile(path, mediaSource, OnVideoUploaded);
        });
    }

    void OnVideoUploaded(bool status, List<string> videoURL)
    {
        if (status)
        {
            for (int i = 0; i < videoURL.Count; i++)
            {
                Dictionary<string, object> kvp = new Dictionary<string, object>();

                kvp.Add("content_id", 1);

                kvp.Add("content_url", videoURL[i]);

                kvp.Add("media_type", "video");

                uploadedDict.Add(kvp);
            }
        }
        JoinAudition();
    }

    void Refresh()
    {
        auditionController?.GetAuditions();
    }

    void MediaButtonAction(int index)
    {
        EMediaType selectedType = (EMediaType)index;
        AlertMessage.Instance.SetText(index + "  " + selectedType);
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
        AlertMessage.Instance.SetText("OnImagesUploaded/" + status);
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

    void JoinAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("audition_id", auditionData.id);
        parameters.Add("port_album_media", uploadedDict);
        GameManager.Instance.apiHandler.JoinAudition(parameters, (status, response) =>
        {
            if (status)
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Joined Successfully";
                alertModel.okayButtonAction = Refresh;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    void UpdateAudition()
    {
        AlertMessage.Instance.SetText("AuditionCell/UpdateAudition");
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("id", auditionData.id);
        parameters.Add("port_album_media", uploadedDict);

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) =>
        {

            AlertMessage.Instance.SetText(status + "==" + response);

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

    void WithDrawAudition()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();

        parameters.Add("audition_id", auditionData.audition_id);
        parameters.Add("id", auditionData.id);
        parameters.Add("status", 8);

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) =>
        {
            if (status)
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Audition Deleted Successfully";
                alertModel.okayButtonAction = Refresh;
                alertModel.canEnableTick = true;
                UIManager.Instance.ShowAlert(alertModel);
            }
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Deleting Audition Failed";
                UIManager.Instance.ShowAlert(alertModel);
            }
        });
    }

    void StartCountDown()
    {
        //CancelInvoke("CountDown");

        long totalSeconds = auditionData.Audition.end_date;

        if (totalSeconds > 0)
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(totalSeconds);

            if (timeSpan.Days > 2)
            {
                //endDate.text = "Entries close in " + timeSpan.Days + "Days";
            }
            else
            {
                hours = (int)(totalSeconds / 3600);

                int remainingSeconds = (int)(totalSeconds % 3600);

                minutes = remainingSeconds / 60;

                seconds = remainingSeconds % 60;

                InvokeRepeating("CountDown", 0.0f, 1.0f);
            }
        }
    }

    void CountDown()
    {
        if (--seconds >= 0)
        {

        }
        else
        {
            if (minutes > 0)
            {
                minutes--;
                seconds = 59;
            }
            else
            {
                if (hours > 0)
                {
                    hours--;
                    minutes = 59;
                    seconds = 59;
                }
                else
                {
                    CancelInvoke("CountDown");
                }
            }
        }

        //endDate.text = "Entries close in " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
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


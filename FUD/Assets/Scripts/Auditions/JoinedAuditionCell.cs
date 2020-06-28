using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class JoinedAuditionCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text endDate;
    public TMP_Text entriesCountText;
    public TMP_Text statusText;

    public Sprite defaultSprite;

    public Image tagImage;


    public Sprite reviewSprite;

    public Sprite shortlistedSprite;

    public Sprite rejectedSprite;

    JoinedAudition auditionData;
    AuditionAlteredView auditionController;

    ETabType auditionType;

    int seconds;
    int minutes;
    int hours;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    public void SetView(AuditionAlteredView auditionController, JoinedAudition audition)
    {
        auditionData = audition;
        this.auditionController = auditionController;
        auditionType = auditionController.tabType;

        if (auditionData != null)
        {
            titleText.text = auditionData.Audition.title;

            SetStatus(auditionData.status);

            StartCountDown();
        }
    }

    void SetStatus(int status)
    {
        tagImage.gameObject.SetActive(true);

        tagImage.sprite = GetStatusSprite(auditionData.GetAuditonStatus());
        //statusText.text = status;
    }

    Sprite GetStatusSprite(EAuditionStatus auditionStatus)
    {
        Sprite sprite = null;

        switch (auditionStatus)
        {
            case EAuditionStatus.Review:
                sprite = reviewSprite;
                break;

            case EAuditionStatus.ShortListed:
                sprite = shortlistedSprite;
                break;

            case EAuditionStatus.Rejected:
                sprite = rejectedSprite;
                break;
        }

        return sprite;
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

            GalleryManager.Instance.UploadVideoFile(path, OnVideoUploaded);
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
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            UIManager.Instance.ShowAlert(alertModel);
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
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            UIManager.Instance.ShowAlert(alertModel);
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
        else
        {
            AlertModel alertModel = new AlertModel();

            alertModel.message = status.ToString();

            UIManager.Instance.ShowAlert(alertModel);
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
            else
            {
                AlertModel alertModel = new AlertModel();
                alertModel.message = "Joining Audition Failed";
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
        CancelInvoke("CountDown");

        long totalSeconds = auditionData.Audition.end_date;

        if (totalSeconds > 0)
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromMilliseconds(totalSeconds);

            if (timeSpan.Days > 2)
            {
                endDate.text = "Entries close in " + timeSpan.Days + "Days";
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

        endDate.text = "Entries close in " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
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


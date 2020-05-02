using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class JoinedAuditionCell : MonoBehaviour
{
    public Image icon;
    public TMP_Text titleText;
    public TMP_Text endDate;
    public TMP_Text statusText;
    public GameObject TagObject;

    JoinedAudition auditionData;
    AuditionController auditionController;
    AuditionType auditionType;

    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    public void SetView(AuditionController auditionController, JoinedAudition audition)
    {
        auditionData = audition;
        this.auditionController = auditionController;
        auditionType = auditionController.auditionType;
        if (auditionData != null)
        {
            titleText.text = auditionData.Audition.topic;
            endDate.text = "End date : " + DatePicker.Instance.GetDateString(auditionData.Audition.end_date);
            GameManager.Instance.downLoadManager.DownloadImage(auditionData.Audition.image_url, (sprite) => {
                icon.sprite = sprite;
            });
            SetStatus(auditionData.status);
        }
    }

    void SetStatus(string status)
    {
        if (string.IsNullOrEmpty(status))
        {
            TagObject.SetActive(false);
            return;
        }
        TagObject.SetActive(true);
        statusText.text = status;
    }
    public void OnClickAction()
    {
        AlertMessage.Instance.SetText("OnClickAction : "+auditionData.id, false);
        Debug.Log("OnClickAction ");
        if (auditionType == AuditionType.Joined)
        {
            AuditionJoinView.Instance.Load(auditionData.Audition, true, (index) =>
            {
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
        }else if (auditionType == AuditionType.Created)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("id", auditionData.id);
            parameters.Add("page", 0);
            parameters.Add("limit", 20);

            GameManager.Instance.apiHandler.SearchAuditions(parameters, (status, response) => {
                if (status)
                {
                    Debug.Log(response);

                    SearchAuditionResponse auditionResponse = JsonUtility.FromJson<SearchAuditionResponse>(response);
                    
                    if(auditionResponse.data.UserAudition!=null && auditionResponse.data.UserAudition.Count > 0)
                    {
                        auditionController.SetUserAuditions(auditionResponse.data.UserAudition);
                    }
                }
            });
        }
    }

    void Refresh()
    {
        auditionController?.GetAuditions();
    }

    void MediaButtonAction(int index)
    {
        EMediaType selectedType = (EMediaType)index;
        AlertMessage.Instance.SetText(index+"  "+selectedType);
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
        AlertMessage.Instance.SetText("OnImagesUploaded/"+status);
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
        GameManager.Instance.apiHandler.JoinAudition(parameters, (status, response) => {
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

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) => {

            AlertMessage.Instance.SetText(status+"=="+response);

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
        parameters.Add("id", auditionData.id);
        parameters.Add("status", "inactive");

        GameManager.Instance.apiHandler.UpdateJoinedAudition(parameters, (status, response) => {
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

}

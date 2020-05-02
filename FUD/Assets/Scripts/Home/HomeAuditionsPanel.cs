using System.Collections.Generic;
using UnityEngine;

public class HomeAuditionsPanel : MonoBehaviour
{
    public GameObject homeCell;
    public Transform parentContent;
    List<Audition> auditionsList = new List<Audition>();
    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    Audition auditionData;

    private void OnEnable()
    {
        GetAuditions();
    }

    void GetAuditions()
    {
        auditionsList = new List<Audition>();
        GameManager.Instance.apiHandler.FetchAuditions(AuditionType.Live, (status, response) => {

            if (status)
            {
                AuditionsResponse auditionsResponse = JsonUtility.FromJson<AuditionsResponse>(response);

                this.auditionsList = auditionsResponse.data;
            }

            SetView();
        });
    }

    void SetView()
    {
        foreach (Transform child in parentContent)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (auditionsList == null)
        {
            auditionsList = new List<Audition>();
        }

        for(int i = 0; i < auditionsList.Count; i++)
        {
            GameObject storyObject = Instantiate(homeCell, parentContent);

            HomeCell homeItem = storyObject.GetComponent<HomeCell>();

            homeItem.SetView(HomeCellType.AUDITION, i, auditionsList[i].title, OnAuditionSelectAction);
        }
    }

    void OnAuditionSelectAction(int index)
    {
        //Open audition of storieslist[index]

        Debug.Log("OnAuditionSelectAction : " + index);

        auditionData = auditionsList[index];

        AuditionJoinView.Instance.Load(auditionData, false, (_index) =>
        {
            switch (_index)
            {
                case 3:
                    GalleryButtonsPanel.Instance.Load(MediaButtonAction);
                    break;
            }
        });
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
            JoinAudition();
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
            JoinAudition();
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
            JoinAudition();
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
                alertModel.okayButtonAction = GetAuditions;
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
}

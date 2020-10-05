﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HomeAuditionsPanel : MonoBehaviour
{
    public GameObject homeCell;
    public Transform parentContent;

    public GameObject noDataObject;

    public AuditionDetailView detailView;

    List<Audition> auditionsList = new List<Audition>();
    List<Dictionary<string, object>> uploadedDict = new List<Dictionary<string, object>>();

    Audition auditionData;

    private string mediaSource = "audition";

    private void OnEnable()
    {
        GetAuditions();
    }

    void GetAuditions()
    {
        auditionsList = new List<Audition>();
        GameManager.Instance.apiHandler.GetHomeAuditions((status, response) => {

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
        parentContent.DestroyChildrens();

        noDataObject.SetActive(auditionsList.Count == 0);

        if (auditionsList == null)
        {
            auditionsList = new List<Audition>();
        }

        for(int i = 0; i < auditionsList.Count; i++)
        {
            GameObject storyObject = Instantiate(homeCell, parentContent);

            HomeCell homeItem = storyObject.GetComponent<HomeCell>();

            homeItem.SetView(HomeCellType.AUDITION, i, auditionsList[i].title, auditionsList[i].image_url, OnAuditionSelectAction);
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
                case 6:
                    RecordVideo();
                    break;

                case 9://View
                    detailView.Load(auditionData);
                    break;

                case 10:
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

    void RecordVideo()
    {
        NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
        {
            string fileName = Path.GetFileName(path);

            byte[] fileBytes = File.ReadAllBytes(path);

            NativeGallery.SaveVideoToGallery(fileBytes, "Videos", fileName);

            GalleryManager.Instance.UploadVideoFile(path, mediaSource, OnVideosUploaded);
        });
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
        });
    }
}

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WorkExperienceDetailView : MonoBehaviour
{
    public TextMeshProUGUI roleText;

    public TextMeshProUGUI locationText;

    public TextMeshProUGUI descriptionText;

    public TMP_Text startDateText;

    public TMP_Text endDateText;


    public UploadedFilesHandler filesHandler;


    public GameObject mediaCell;

    public RectTransform mediaContent;


    WorkExperianceModel experianceModel;


    public void Load(WorkExperianceModel experianceModel)
    {
        this.experianceModel = experianceModel;

        gameObject.SetActive(true);

        SetView();
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }

    void SetView()
    {
        List<Genre> genres = DataManager.Instance.genres;

        Genre genre = genres.Find(item => item.id == experianceModel.role_id);

        roleText.text = genre.name;

        locationText.text = experianceModel.MasterData.name;

        descriptionText.text = experianceModel.description;

        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        DateTime selectedDate = dateTime.AddSeconds(experianceModel.start_date);

        startDateText.text = DatePicker.Instance.GetDateString(selectedDate);

        selectedDate = dateTime.AddSeconds(experianceModel.end_date);

        endDateText.text = DatePicker.Instance.GetDateString(selectedDate);

        UpdateMediaView();
    }

    void UpdateMediaView()
    {
        string[] _imageURls = new string[experianceModel.WorkExpMedia.Count];

        int totalCount = experianceModel.WorkExpMedia.Count;

        EMediaType mediaType;

        for (int i = 0; i < totalCount; i++)
        {
            mediaType = DataManager.Instance.GetMediaType(experianceModel.WorkExpMedia[i].media_type);

            if (mediaType == EMediaType.Image)
            {
                _imageURls[i] = experianceModel.WorkExpMedia[i].content_url;
            }

            filesHandler.Load(_imageURls, true);
        }
    }
}

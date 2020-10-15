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
        List<Craft> genres = DataManager.Instance.crafts;

        Craft genre = genres.Find(item => item.id == experianceModel.role_id);

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
        for (int i = 0; i < 4; i++)
        {
            filesHandler.Load(GetMediaModels(i), (EMediaType)i);
        }
    }

    List<MultimediaModel> GetMediaModels(int index)
    {
        EMediaType mediaType = (EMediaType)index;

        List<MultimediaModel> multimediaModels = experianceModel.WorkExpMedia.FindAll(item => DataManager.Instance.GetMediaType(item.media_type) == mediaType);

        return multimediaModels;
    }
}

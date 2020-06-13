using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class AuditionDetailView : MonoBehaviour
{
    public RectTransform mediaContent;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI descriptionText;

    public TextMeshProUGUI topicText;

    public TextMeshProUGUI budgetText;

    public TextMeshProUGUI ageText;

    public TextMeshProUGUI endDateText;

    public GameObject mediaCell;


    Audition auditionData;


    public void Load(Audition auditionData)
    {
        this.auditionData = auditionData;

        gameObject.SetActive(true);

        SetView();
    }

    void SetView()
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        DateTime endDate = dateTime.AddSeconds(auditionData.end_date);

        titleText.text = auditionData.title;

        descriptionText.text = auditionData.description;

        topicText.text = auditionData.topic;

        budgetText.text = auditionData.rate_of_pay.ToString();

        ageText.text = auditionData.age_from + " To " + auditionData.age_to;

        endDateText.text = DatePicker.Instance.GetDateString(endDate);
    }

    public void OnBackButtonAction()
    {
        gameObject.SetActive(false);
    }
}

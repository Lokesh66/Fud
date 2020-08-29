using UnityEngine;
using System;
using TMPro;


public class StoryHistoryCell : MonoBehaviour
{
    public TextMeshProUGUI descriptionText;


    public void Load(StoryHistoryModel historyModel)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);

        DateTime updatedDateTime = dateTime.AddSeconds(historyModel.updated_date_time);

        string dateFormat = DatePicker.Instance.GetDateString(updatedDateTime);

        descriptionText.text = historyModel.fromUser.name + " " + historyModel.activity + " story at " + dateFormat; 
    }
}

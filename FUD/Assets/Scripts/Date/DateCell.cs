using System;
using UnityEngine;
using TMPro;

public class DateCell : MonoBehaviour
{
    public TMP_Text textField;

    private DateTime selectedDate;

    Action<DateTime, Transform> onSelectDate;

    public void SetData(DateTime date, DateTime currentDate, Action<DateTime, Transform> action)
    {
        onSelectDate = action;

        selectedDate = date;

        textField.color = selectedDate.Month == currentDate.Month ? Color.white : Color.grey;

        textField.text = (selectedDate.Day).ToString();
    }

    public void OnClick()
    {
        onSelectDate?.Invoke(selectedDate, this.transform);
    }
}

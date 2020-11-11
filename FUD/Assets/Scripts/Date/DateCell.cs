using UnityEngine;
using System;
using TMPro;


public class DateCell : MonoBehaviour
{
    public TMP_Text textField;

    private DateTime selectedDate;

    private bool isValid;

    Action<DateTime, Transform> onSelectDate;


    public void SetData(DateTime date, DateTime currentDate, bool isValidDate, Action<DateTime, Transform> action)
    {
        onSelectDate = action;

        selectedDate = date;
        isValid = isValidDate;
        textField.color = isValid ? selectedDate.Month == currentDate.Month ? Color.white : Color.grey : new Color(0.75f, 0f, 0f, 1f);

        textField.text = (selectedDate.Day).ToString();
    }

    public void OnClick()
    {
        if (isValid)
        {
            onSelectDate?.Invoke(selectedDate, transform);
        }
    }
}

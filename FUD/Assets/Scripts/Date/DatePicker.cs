using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DatePicker : MonoBehaviour
{
    #region Singleton

    private static DatePicker instance = null;

    private DatePicker()
    {

    }

    public static DatePicker Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DatePicker>();
            }
            return instance;
        }
    }
    #endregion

    #region variables
    public GameObject parentPanel;

    public TMP_Text dateText;
    public GameObject frame;
    
    public List<DateCell> dateCells;

    #endregion

    private DateTime selectedDate;

    System.Action<string> OnSelectDate;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void GetDate(DateTime startDate, System.Action<string> action)
    {
        OnSelectDate = action;
        parentPanel.SetActive(true);
    }

    public void OnSelectAction(DateTime date, Transform cell)
    {        
        selectedDate = date;

        if(selectedDate.Month != _dateTime.Month)
        {
            _dateTime = _dateTime.AddMonths(selectedDate.Month > _dateTime.Month ? 1 : -1);
            CreateCalendar();
        }
        else
        {
            frame.gameObject.SetActive(true);
            frame.transform.position = cell.position;
        }
    }

    public void RightButtonAction()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }

    public void LeftButtonAction()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void OkButtonAction()
    {
        string dateString = selectedDate.Year.ToString() + "-" + selectedDate.Month.ToString("d2") + "-" + selectedDate.Day.ToString("d2");
        
        ClosePanel(dateString);
    }

    public void CancelButtonAction()
    {
        ClosePanel();
    }

    void ClosePanel(string result = "")
    {
        parentPanel.SetActive(false);
        OnSelectDate?.Invoke(result);
    }

    private DateTime _dateTime;

    void OnEnable()
    {
        _dateTime = DateTime.Now;

        selectedDate = _dateTime;

        CreateCalendar();
    }

    void CreateCalendar()
    {
        frame.gameObject.SetActive(false);

        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < dateCells.Count; i++)
        {
            DateTime thatDay = firstDay.AddDays(i - index >= 0 ? date : i-index);

            dateCells[i].SetData(thatDay, firstDay, OnSelectAction);
            
            if (i- index >= 0) { date++; }

            if (selectedDate.Year == thatDay.Year && selectedDate.Month == thatDay.Month && selectedDate.Day == thatDay.Day)
            {
                OnSelectAction(thatDay, dateCells[i].transform);
            }
        }

        dateText.text = _dateTime.ToString("MMMM")+" "+_dateTime.Year.ToString();
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }
}

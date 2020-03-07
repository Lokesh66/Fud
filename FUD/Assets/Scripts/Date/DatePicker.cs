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
    public GameObject leftButton;
    public GameObject rightButton;
    public TMP_Text dateText;
    public GameObject frame;
    
    public List<DateCell> dateCells;

    #endregion

    private DateTime currentDate;
    private DateTime selectedDate;
    private DateTime startDate;
    public DateTime endDate;

    System.Action<DateTime, string> OnSelectDate;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void GetDate(DateTime _selectedDate, DateTime _startDate, DateTime _endDate, System.Action<DateTime, string> action)
    {
        OnSelectDate = action;
        startDate = _startDate;
        endDate = _endDate;
        parentPanel.SetActive(true);
        currentDate = _selectedDate;
        selectedDate = currentDate;
        _dateTime = currentDate;
        CreateCalendar();
    }

    public void OnSelectAction(DateTime date, Transform cell)
    {
        if (!IsValidDate(date))
            return;
        selectedDate = date;
        if(selectedDate.Month != _dateTime.Month)
        {
            if (selectedDate.Year != _dateTime.Year)
            {
                if (selectedDate.Month == 12 && _dateTime.Month == 1)
                {
                    LeftButtonAction();
                }
                else if (selectedDate.Month == 1 && _dateTime.Month == 12)
                {
                    RightButtonAction();
                }
            }
            else
            {
                if (selectedDate.Month > _dateTime.Month)
                    RightButtonAction();
                else
                    LeftButtonAction();
            }
        }
        else
        {
            UpdateFramePosition(cell);
        }
    }

    bool IsValidDate(DateTime selectedDate)
    {
        if (selectedDate.Year > startDate.Year ||
            (selectedDate.Year == startDate.Year && selectedDate.Month > startDate.Month) ||
            (selectedDate.Month == startDate.Month && selectedDate.Date >= startDate.Date))
        {
            if(selectedDate.Year < endDate.Year ||
                (selectedDate.Year == endDate.Year && selectedDate.Month < endDate.Month) ||
                (selectedDate.Month == endDate.Month && selectedDate.Day <= endDate.Day))
            {
                return true;
            }
        }
        return false; ;
    }
    void UpdateFramePosition(Transform cell = null)
    {
        if (cell != null)
        {
            frame.gameObject.SetActive(true);
            frame.transform.position = cell.position;
        }
        else
        {
            frame.gameObject.SetActive(false);
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

    public string GetDateString(DateTime dateTime)
    {
        if(dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
        {
            return string.Empty;
        }
        string dateString = dateTime.Year.ToString("d4") + "-" + dateTime.Month.ToString("d2") + "-" + dateTime.Day.ToString("d2");
        return dateString;
    }

    public void OkButtonAction()
    {        
        ClosePanel(GetDateString(selectedDate));
    }

    public void CancelButtonAction()
    {
        ClosePanel(GetDateString(currentDate));
    }

    void ClosePanel(string result = "")
    {
        parentPanel.SetActive(false);
        OnSelectDate?.Invoke(selectedDate, result);
    }

    private DateTime _dateTime;

    
    void CreateCalendar()
    {
        UpdateFramePosition();
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < dateCells.Count; i++)
        {
            DateTime thatDay = firstDay.AddDays(i - index >= 0 ? date : i-index);

            dateCells[i].SetData(thatDay, firstDay, IsValidDate(thatDay), OnSelectAction);
            
            if (i- index >= 0) { date++; }

            if (selectedDate.Year == thatDay.Year && selectedDate.Month == thatDay.Month && selectedDate.Day == thatDay.Day)
            {
                UpdateFramePosition(dateCells[i].transform);
            }
        }
        
        leftButton.SetActive(IsValidDate(firstDay.AddDays(-1)));
        rightButton.SetActive(IsValidDate(firstDay.AddMonths(1)));

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

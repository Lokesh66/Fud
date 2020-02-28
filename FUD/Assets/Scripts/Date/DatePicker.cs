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
    public TMP_Text dateText;
    public GameObject frame;
    
    public List<DateCell> dateCells;

    #endregion

    private DateTime selectedDate;
    private DateTime startDate;

    System.Action<string> OnSelectDate;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void GetDate(DateTime _startDate, System.Action<string> action)
    {
        OnSelectDate = action;
        startDate = _startDate;
        parentPanel.SetActive(true);
        _dateTime = DateTime.Now;
        selectedDate = _dateTime;
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
        if (selectedDate.Year > startDate.Year)
        {
            return true;
        }
        else if (selectedDate.Year == startDate.Year && selectedDate.Month > startDate.Month)
        {
            return true;
        }
        else if (selectedDate.Month == startDate.Month && selectedDate.Date >= startDate.Date)
        {
            return true;
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

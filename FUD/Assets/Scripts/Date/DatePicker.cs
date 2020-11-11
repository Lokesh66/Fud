using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


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

    public enum EYearsType
    {
        None,
        DOBYears,
        ProjectYears,
    }

    #region variables
    public GameObject parentPanel;
    public GameObject leftButton;
    public GameObject rightButton;
    public TMP_Text monthText;
    public TMP_Text yearText;
    public GameObject frame;

    public TMP_Dropdown monthDropDown;

    public TMP_Dropdown yearDropDown;

    public List<DateCell> dateCells;

    #endregion

    private DateTime currentDate;
    private DateTime selectedDate;
    private DateTime startDate;
    private EYearsType yearType;

    List<int> years = null;

    public DateTime endDate;

    List<int> dobYears = new List<int> { 1940, 1941, 1942, 1943, 1944, 1945, 1946, 1947, 1948, 1949, 1950, 1951, 1952, 1953, 1954, 1955,

        1956, 1957, 1958, 1959, 1960, 1961, 1962, 1963, 1964, 1965, 1966, 1967, 1968, 1969, 1970, 1971, 1972, 1973, 1974, 1975, 1976, 1977,

        1978, 1979, 1980, 1981, 1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999,

        2000, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020

    };

    List<int> projectYears = new List<int> {

        2020, 2021, 2022, 2023, 2024, 2025, 2026, 2027, 2028, 2029, 2030, 2040, 2041, 2042, 2043, 2044, 2045, 2045, 2046, 2047, 2048, 2049, 2050
    };

    Action<DateTime, string> OnSelectDate;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void GetDate(DateTime _selectedDate, DateTime _startDate, DateTime _endDate, Action<DateTime, string> action)
    {
        OnSelectDate = action;
        startDate = _startDate;
        endDate = _endDate;
        parentPanel.SetActive(true);
        currentDate = _selectedDate;
        selectedDate = currentDate;
        _dateTime = currentDate;
        CreateCalendar();

        if (endDate.CompareTo(DateTime.Now) == -1)
        {
            yearType = EYearsType.DOBYears;
        }
        else if (endDate.Equals(DateTime.MaxValue))
        {
            yearType = EYearsType.ProjectYears;
        }

        monthDropDown.value = selectedDate.Month - 1;

        PopulateYearsDropDown();
    }

    public void OnSelectAction(DateTime date, Transform cell)
    {
        if (!IsValidDate(date))
            return;
        selectedDate = date;
        if (selectedDate.Month != _dateTime.Month)
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
            if (selectedDate.Year < endDate.Year ||
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
        if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
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
            DateTime thatDay = firstDay.AddDays(i - index >= 0 ? date : i - index);

            dateCells[i].SetData(thatDay, firstDay, IsValidDate(thatDay), OnSelectAction);

            if (i - index >= 0) { date++; }

            if (selectedDate.Year == thatDay.Year && selectedDate.Month == thatDay.Month && selectedDate.Day == thatDay.Day)
            {
                UpdateFramePosition(dateCells[i].transform);
            }
        }

        leftButton.SetActive(IsValidDate(firstDay.AddDays(-1)));
        rightButton.SetActive(IsValidDate(firstDay.AddMonths(1)));

        monthText.text = _dateTime.ToString("MMMM");

        yearText.text = _dateTime.Year.ToString();
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

    public void OnMonthValueChanged()
    {
        int monthDifference = Mathf.Abs(monthDropDown.value + 1 - _dateTime.Month);

        int dropDownValue = monthDropDown.value + 1;

        if (dropDownValue != _dateTime.Month)
        {
            _dateTime = dropDownValue < _dateTime.Month ? _dateTime.AddMonths(-monthDifference) : _dateTime.AddMonths(monthDifference);

            CreateCalendar();
        }
    }

    public void OnYearDropDownValueChanged()
    {
        int selectedYear = int.Parse(yearDropDown.captionText.text);

        int yearDifference = Mathf.Abs(selectedYear - _dateTime.Year);

        if (selectedYear != _dateTime.Year)
        {
            _dateTime = selectedYear < _dateTime.Year ? _dateTime.AddYears(-yearDifference) : _dateTime.AddYears(yearDifference);

            CreateCalendar();
        }
    }

    void PopulateYearsDropDown()
    {
        List<string> options = new List<string>();

        switch (yearType)
        {
            case EYearsType.DOBYears:
                years = dobYears;
                break;

            case EYearsType.ProjectYears:
                years = projectYears;
                break;
        }

        foreach (var option in years)
        {
            options.Add(option.ToString());
        }

        yearDropDown.ClearOptions();
        yearDropDown.AddOptions(options);
    }
}




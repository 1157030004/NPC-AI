using System;
using UnityEngine;

namespace Shadee.WorldTimes
{
    public enum TimeFormat
    {
        Hour_24,
        Hour_12,
    }
    public enum DateFormat
    {
        MM_DD_YYYY,
        DD_MM_YYYY,
        YYYY_MM_DD,
        YYYY_DD_MM,
    }
    public class WorldTimeManager : MonoBehaviour
    {
        [SerializeField] private TimeFormat timeFormat = TimeFormat.Hour_24;
        [SerializeField] private DateFormat dateFormat = DateFormat.DD_MM_YYYY;
        [SerializeField] private float secondPerMinute = 0.5f;

        private string _time;
        private string _date;
        private bool isAm = false;
        
        private int hour;
        private int minute;
        private int day;
        private int month;
        private int year;

        private int maxHour = 24;
        private int maxMinute = 60;
        private int maxDay = 30;
        private int maxMonth = 12;

        private float timer;

        public static WorldTimeManager Instance { get; private set; }

        public static Action OnMinuteChanged;
        public static Action OnHourChanged;
        public static Action OnDayChanged;
        public static Action OnMonthChanged;
        public static Action OnYearChanged;

        public int Hour { get => hour; }
        public int Minute { get => minute; }
        public int Day { get => day; }
        public int Month { get => month; }
        public int Year { get => year; }

        public string Time { get => _time; }
        public string Date { get => _date; }

        private void Awake() 
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            hour = 6;
            minute = 0;
            day = 1;
            month = 1;
            year = 1;

            if(hour < 12)
            {
                isAm = true;
            }
            SetTimeDateString();    
        }

        private void Update() 
        {
            if(timer >= secondPerMinute)
            {
                minute++;
                OnMinuteChanged?.Invoke();
                if(minute >= maxMinute)
                {
                    minute = 0;
                    hour++;
                    OnHourChanged?.Invoke();
                    if(hour >= maxHour)
                    {
                        hour = 0;
                        day++;
                        OnDayChanged?.Invoke();
                        if(day >= maxDay)
                        {
                            day = 1;
                            month++;
                            OnMonthChanged?.Invoke();
                            if(month >= maxMonth)
                            {
                                month = 1;
                                year++;
                                OnYearChanged?.Invoke();
                            }
                        }
                    }
                }
                SetTimeDateString();
                timer = 0;
            }
            else
            {
                timer += UnityEngine.Time.deltaTime;
            }    
        }

        private void SetTimeDateString()
        {
            switch (timeFormat)
            {
                case TimeFormat.Hour_12:
                {
                    int h;
                    if(hour >= 13)
                    {
                        h = hour - 12;
                    }
                    else if(hour == 0)
                    {
                        h = 12;
                    }
                    else
                    {
                        h = hour;
                    }

                    _time = h + ":";
                    if(minute <= 9)
                    {
                        _time += "0" + minute;
                    }
                    else
                    {
                        _time += minute;
                    }

                    if(isAm && hour < 12)
                    {
                        _time += " AM";
                    }
                    else
                    {
                        _time += " PM";
                    }
                    break;
                }
                case TimeFormat.Hour_24:
                {
                    if(hour <= 9)
                    {
                        _time = "0" + hour + ":";
                    }
                    else
                    {
                        _time = hour + ":";
                    }

                    if(minute <= 9)
                    {
                        _time += "0" + minute;
                    }
                    else
                    {
                        _time += minute;
                    }
                    break;
                }
            }

            switch (dateFormat)
            {
                case DateFormat.DD_MM_YYYY:
                    _date = day + "/" + month + "/" + year;
                    break;
                case DateFormat.MM_DD_YYYY:
                    _date = month + "/" + day + "/" + year;
                    break;
                case DateFormat.YYYY_MM_DD:
                    _date = year + "/" + month + "/" + day;
                    break;
                case DateFormat.YYYY_DD_MM:
                    _date = year + "/" + day + "/" + month;
                    break;
            }

        }
    }
}

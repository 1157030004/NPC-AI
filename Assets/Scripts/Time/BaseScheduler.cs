using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shadee
{
    public class BaseScheduler : MonoBehaviour
    {
        // initiate list of event eventhandler
        public List<EventHandler<ScheduleEventArgs>> ScheduledEvents = new List<EventHandler<ScheduleEventArgs>>();

        protected virtual void Start()
        {

        }

        protected virtual void OnEnable() 
        {
            TimeManager.OnMinuteChanged += CheckSchedule;
            TimeManager.OnHourChanged += CheckSchedule;
            TimeManager.OnDayChanged += CheckSchedule;
            TimeManager.OnMonthChanged += CheckSchedule;
            TimeManager.OnYearChanged += CheckSchedule;
        }

        protected virtual void OnDisable() 
        {
            TimeManager.OnMinuteChanged -= CheckSchedule;
            TimeManager.OnHourChanged -= CheckSchedule;
            TimeManager.OnDayChanged -= CheckSchedule;
            TimeManager.OnMonthChanged -= CheckSchedule;
        }
        protected virtual void CheckSchedule()
        {

        }

        [Serializable]
        public class Schedule
        {
            public int Hour;
            public int Minute;
            public int Day;
            public int Month;
            public int Year;
            public BaseInteraction Interaction;
            public SmartObject Target;
        }
    }
}

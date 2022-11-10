using System;
using System.Collections;
using System.Collections.Generic;
using Shadee.WorldTimes;
using UnityEngine;
using UnityEngine.Events;

namespace Shadee.NPC_Villagers
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
            WorldTimeManager.OnMinuteChanged += CheckSchedule;
            WorldTimeManager.OnHourChanged += CheckSchedule;
            WorldTimeManager.OnDayChanged += CheckSchedule;
            WorldTimeManager.OnMonthChanged += CheckSchedule;
            WorldTimeManager.OnYearChanged += CheckSchedule;
        }

        protected virtual void OnDisable() 
        {
            WorldTimeManager.OnMinuteChanged -= CheckSchedule;
            WorldTimeManager.OnHourChanged -= CheckSchedule;
            WorldTimeManager.OnDayChanged -= CheckSchedule;
            WorldTimeManager.OnMonthChanged -= CheckSchedule;
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

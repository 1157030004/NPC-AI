using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shadee.WorldTimes;
using UnityEngine;
using UnityEngine.Events;

namespace Shadee.NPC_Villagers
{
    public class DateMonthScheduler : BaseScheduler
    {
        [SerializeField] private List<Schedule> _schedules;
        public event EventHandler<ScheduleEventArgs> Scheduled;

        protected override void Start()
        {
            base.Start();
            ScheduledEvents.Add(Scheduled);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }


        protected override void CheckSchedule()
        {
            var schedule = _schedules.FirstOrDefault(s =>
            s.Hour == WorldTimeManager.Instance.Hour && 
            s.Minute == WorldTimeManager.Instance.Minute &&
            s.Day == WorldTimeManager.Instance.Day &&
            s.Month == WorldTimeManager.Instance.Month
            );

            if(schedule != null)
            {
                Scheduled?.Invoke(this, new ScheduleEventArgs{
                Hour = schedule.Hour,
                Minute = schedule.Minute,
                Day = schedule.Day,
                Month = schedule.Month,
                Interaction = schedule.Interaction,
                Target = schedule.Target
                });
            }
        }
    }
}


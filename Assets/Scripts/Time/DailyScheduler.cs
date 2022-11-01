using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shadee
{
    public class DailyScheduler : BaseScheduler
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
            s.Hour == TimeManager.Instance.Hour && 
            s.Minute == TimeManager.Instance.Minute);

            if(schedule != null)
            {
                Scheduled?.Invoke(this, new ScheduleEventArgs{
                Hour = schedule.Hour,
                Minute = schedule.Minute,
                Interaction = schedule.Interaction,
                Target = schedule.Target
                });
            }
        }
    }
}

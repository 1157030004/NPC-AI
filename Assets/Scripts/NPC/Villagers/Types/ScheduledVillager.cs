using Shadee.NPC_AI;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    [RequireComponent(typeof(BaseNavigation))]
    public class ScheduledVillager : CommonVillager
    {
        protected DailyScheduler DailyScheduler;
        protected DateScheduler DateScheduler;
        protected DateMonthScheduler DateMonthScheduler;
        protected ScheduleEventArgs ScheduledInteraction;

        protected bool ShouldStartTimedInteraction = false;

        protected override void Awake()
        {
            base.Awake();
            DailyScheduler = GetComponent<DailyScheduler>();
            DateScheduler = GetComponent<DateScheduler>();
            DateMonthScheduler = GetComponent<DateMonthScheduler>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            DailyScheduler.Scheduled += OnScheduled;
            DateScheduler.Scheduled += OnScheduled;
            DateMonthScheduler.Scheduled += OnScheduled;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            DailyScheduler.Scheduled -= OnScheduled;
            DateScheduler.Scheduled -= OnScheduled;
            DateMonthScheduler.Scheduled -= OnScheduled;
        }
        private void OnScheduled(object sender, ScheduleEventArgs e)
        {
            ShouldStartTimedInteraction = true;
            ScheduledInteraction = e;
            StartScheduledInteraction();
        }

        private void StartScheduledInteraction()
        {
            if(ScheduledInteraction == null)
                return;

            if(ScheduledInteraction.Interaction.CanPerform())
            {
                CurrenInteraction = ScheduledInteraction.Interaction;
                CurrenInteraction.LockInteraction(this);
                StartedPerforming = false;

                if(!Navigation.SetDestination(ScheduledInteraction.Target.InteractionPoint))
                {
                    Debug.LogError($"Could not move to {ScheduledInteraction.Target.name}");
                    CurrenInteraction = null;
                    return;
                }
                else
                {
                    Debug.Log($"Going to {CurrenInteraction.DisplayName} at {ScheduledInteraction.Target.DisplayName}");
                }
  
            }
        }
    }
}

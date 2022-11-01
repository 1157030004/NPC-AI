using UnityEngine;

namespace Shadee
{
    [RequireComponent(typeof(BaseNavigation))]
    public class ScheduledAI : CommonAIBase
    {
        protected override void Start()
        {
            base.Start();
        }

        protected override void OnScheduled(object sender, ScheduleEventArgs e)
        {
            base.OnScheduled(sender, e);
            StartScheduledInteraction();
        }

        private void StartScheduledInteraction()
        {
            if(ScheduledInteraction == null)
            {
                return;
            }


            if(ScheduledInteraction.Interaction.CanPerform())
            {
                CurrenInteraction = ScheduledInteraction.Interaction;
                CurrenInteraction.LockInteraction();
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

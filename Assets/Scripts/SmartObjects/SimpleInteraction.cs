using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shadee
{
    public class SimpleInteraction : BaseInteraction
    {
        protected class PerformerInfo
        {
            public CommonAIBase PerformingAI;
            public float ElapseTime;
            public UnityAction<BaseInteraction> OnCompleted;
        }
        [SerializeField] protected int MaxSimultaneousUsers = 1;
        protected int NumCurrentUsers = 0;
        protected List<PerformerInfo> CurrentPerformers = new List<PerformerInfo>(); 
        public override bool CanPerform()
        {
            return NumCurrentUsers < MaxSimultaneousUsers;
        }

        public override void LockInteraction()
        {
            NumCurrentUsers++;

            if(NumCurrentUsers > MaxSimultaneousUsers)
            {
                Debug.LogError($"Too many users have locked this interaction {_DisplayName}");
            }
        }

        public override void Perform(CommonAIBase performer, UnityAction<BaseInteraction> onCompleted = null)
        {
            if(NumCurrentUsers <= 0)
            {
                Debug.LogError($"Trying to perform {DisplayName} on {gameObject.name} but no users are currently using it");
                return;
            }

            
            if(InteractionType == EInteractionType.Instantaneous)
            {
                if(_StatChanges.Length > 0)
                {
                    ApplyStatChanges(performer, 1f);
                }
                onCompleted.Invoke(this);
  
            }
            else if(InteractionType == EInteractionType.OverTime)
            {
                CurrentPerformers.Add(new PerformerInfo()
                {
                    PerformingAI = performer,
                    ElapseTime = 0.0f,
                    OnCompleted = onCompleted
                });
            }
        }

        public override void UnlockInteraction()
        {
            if(NumCurrentUsers <= 0)
            {
                Debug.LogError($"Trying to unlock interaction {DisplayName} on {gameObject.name} but it is already unlocked");
                return;
            }
            NumCurrentUsers--;
        }

        protected virtual void Update()
        {
            for(int i = CurrentPerformers.Count - 1; i >= 0; i--)
            {
                PerformerInfo performer = CurrentPerformers[i];
                
                float previousElapsedTime = performer.ElapseTime;
                performer.ElapseTime = Mathf.Min(performer.ElapseTime + Time.deltaTime, _Duration);


                if(_StatChanges.Length > 0)
                {
                    ApplyStatChanges(performer.PerformingAI, (performer.ElapseTime - previousElapsedTime) / _Duration);
                }

                if(performer.ElapseTime >= Duration)
                {
                    performer.OnCompleted.Invoke(this);
                    CurrentPerformers.RemoveAt(i);
                }
            }
        }
    }
}

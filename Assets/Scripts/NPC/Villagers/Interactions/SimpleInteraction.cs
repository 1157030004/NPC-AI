using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shadee.NPC_Villagers
{
    public class SimpleInteraction : BaseInteraction
    {
        protected class PerformerInfo
        {
            // public CommonAIBase PerformingAI;
            public float ElapseTime;
            public UnityAction<BaseInteraction> OnCompleted;
        }
        [SerializeField] protected int MaxSimultaneousUsers = 1;
        protected Dictionary<CommonVillager, PerformerInfo> CurrentPerformers = new Dictionary<CommonVillager, PerformerInfo>(); 
        protected List<CommonVillager> PerformersToCleanUp = new List<CommonVillager>();
        public int NumCurrentUsers => CurrentPerformers.Count;

        public override bool CanPerform()
        {
            return NumCurrentUsers < MaxSimultaneousUsers;
        }

        public override bool LockInteraction(CommonVillager performer)
        {

            if(NumCurrentUsers >= MaxSimultaneousUsers)
            {
                Debug.LogError($"{performer.name} tried to lock {_DisplayName} but it is already locked");
                return false;
            }

            if(CurrentPerformers.ContainsKey(performer))
            {
                Debug.LogError($"Trying to lock an interaction twice {_DisplayName}");
                return false;
            }

            CurrentPerformers[performer] = null;
            return true;
        }

        public override bool Perform(CommonVillager performer, UnityAction<BaseInteraction> onCompleted = null)
        {
            if(!CurrentPerformers.ContainsKey(performer))
            {
                Debug.LogError($"{performer.name} is trying to perform {_DisplayName} but it is not locked");
                return false;
            }

            
            if(InteractionType == EInteractionType.Instantaneous)
            {
                if(_StatChanges.Length > 0)
                    ApplyStatChanges(performer, 1f);
                
                OnInteractionCompleted(performer, onCompleted);
  
            }
            else if(InteractionType == EInteractionType.OverTime)
            {
                CurrentPerformers[performer] = new PerformerInfo()
                {
                    ElapseTime = 0f,
                    OnCompleted = onCompleted
                };
            }

            return true;
        }

        protected void OnInteractionCompleted(CommonVillager performer, UnityAction<BaseInteraction> onCompleted)
        {
            onCompleted.Invoke(this);

            if(!PerformersToCleanUp.Contains(performer))
            {
                PerformersToCleanUp.Add(performer);
                Debug.LogWarning($"{performer.name} did not unlock {_DisplayName} after performing it");
            }
        }

        public override bool UnlockInteraction(CommonVillager performer)
        {
            if(CurrentPerformers.ContainsKey(performer))
            {
                PerformersToCleanUp.Add(performer);
                return true;
            }

            Debug.LogError($"{performer.name} is trying to unlock an interaction: {_DisplayName} they have not locked");
            return false;
        }

        protected virtual void Update()
        {
            foreach(var keyValuePair in CurrentPerformers)
            {
                CommonVillager performer = keyValuePair.Key;
                PerformerInfo performerInfo = keyValuePair.Value;

                if(performerInfo == null)
                    continue;
                
                float previousElapsedTime = performerInfo.ElapseTime;
                performerInfo.ElapseTime = Mathf.Min(performerInfo.ElapseTime + Time.deltaTime, _Duration);


                if(_StatChanges.Length > 0)
                    ApplyStatChanges(performer, (performerInfo.ElapseTime - previousElapsedTime) / _Duration);

                if(performerInfo.ElapseTime >= Duration)
                {
                    OnInteractionCompleted(performer, performerInfo.OnCompleted);
                }
            }

            // clean up any performers that have finished
            foreach (var performer in PerformersToCleanUp)
                CurrentPerformers.Remove(performer);

            PerformersToCleanUp.Clear();
        }
    }
}

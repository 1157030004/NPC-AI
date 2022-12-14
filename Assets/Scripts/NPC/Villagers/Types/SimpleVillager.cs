using System.Collections;
using System.Collections.Generic;
using Shadee.NPC_AI;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    [RequireComponent(typeof(BaseNavigation))]
    public class SimpleVillager : CommonVillager
    {
        [SerializeField] protected float PickInteractionInterval = 2f;
        protected float TimeUntilNextInteractionPicked = -1f;

        protected override void Update() 
        {
            base.Update();
            
            if(CurrenInteraction == null)
            {
                TimeUntilNextInteractionPicked -= Time.deltaTime;

                if(TimeUntilNextInteractionPicked <= 0f)
                {
                    TimeUntilNextInteractionPicked = PickInteractionInterval;
                    PickRandomInteraction();
                }
            }
        }

        private void PickRandomInteraction()
        {
            int objectIndex = Random.Range(0, SmartObjectManager.Instance.RegisteredObjects.Count); 
            var selectedObject = SmartObjectManager.Instance.RegisteredObjects[objectIndex];

            int interactionIndex = Random.Range(0, selectedObject.Interactions.Count);
            var selectedInteraction = selectedObject.Interactions[interactionIndex];

            if(selectedInteraction.CanPerform())
            {
                CurrenInteraction = selectedInteraction;
                CurrenInteraction.LockInteraction(this);
                StartedPerforming = false;

                if(!Navigation.SetDestination(selectedObject.InteractionPoint))
                {
                    Debug.LogError($"Could not move to {selectedObject.name}");
                    CurrenInteraction = null;
                    return;
                }
                else
                {
                    Debug.Log($"Going to {CurrenInteraction.DisplayName} at {selectedObject.DisplayName}");
                }
            }
        }
    }
}

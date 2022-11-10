using System.Collections.Generic;
using System.Linq;
using Shadee.Blackboards;
using Shadee.NPC_AI;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    [RequireComponent(typeof(BaseNavigation))]
    public class TraitfulVillager : CommonVillager
    {
        [SerializeField] protected float PickInteractionInterval = 2f;
        [SerializeField] protected float DefaultInteractionScore = 0f;
        [SerializeField] protected int InteractionPickSize = 5;
        [SerializeField] protected bool AvoidInUseObjects = true;
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
                    PickBestInteraction();
                }
            }
        }

        private float ScoreInteraction(BaseInteraction interaction)
        {
            if(interaction.StatChanges.Length == 0)
            {
                return DefaultInteractionScore;
            }

            float score = 0;
            foreach (var change in interaction.StatChanges)
            {
                score += ScoreChange(change.LinkedStat, change.Value);
            }

            return score;
        }

        private float ScoreChange(Stat linkedStat, float amount)
        {
            float currentValue =  GetStatValue(linkedStat);

            return (1f - currentValue) * ApplyTraitsTo(linkedStat, Trait.ETargetType.Score, amount);
        }

        private class ScoredInteraction
        {
            public SmartObject TargetObject;
            public BaseInteraction Interaction;
            public float Score;
        }

        private void PickBestInteraction()
        {
            /*
                1. Find all objects that are in range
                2. Find all interactions that are in range
                3. Score them
                4. Pick the best one 
            */
            List<GameObject> objectsInUse = null;
            HouseHoldBackboard.TryGetGeneric(EBlackboardKey.Household_ObjectsInUse, out objectsInUse, null);
            List<ScoredInteraction> unsortedInteractions = new List<ScoredInteraction>();
            foreach (var smartObject in SmartObjectManager.Instance.RegisteredObjects)
            {
                foreach (var interaction in smartObject.Interactions)
                {
                    if(!interaction.CanPerform())
                        continue;

                    if( AvoidInUseObjects &&  objectsInUse != null && objectsInUse.Contains(interaction.gameObject))
                        continue;
                    
                    float score = ScoreInteraction(interaction);

                    unsortedInteractions.Add(new ScoredInteraction()
                    {
                        TargetObject = smartObject,
                        Interaction = interaction,
                        Score = score
                    });
                }
            }

            if(unsortedInteractions.Count == 0)
            {
                Debug.Log("No interactions available");
                return;
            }

            var sortedInteractions = unsortedInteractions.OrderByDescending(scoredInteractions => scoredInteractions.Score).ToList();
            foreach (var item in sortedInteractions)
            {
                Debug.Log($"Interaction {item.Interaction.DisplayName} at {item.TargetObject.DisplayName} has score {item.Score}");
            }
            int maxIndex = Mathf.Min(InteractionPickSize, sortedInteractions.Count);

            int selectedIndex = Random.Range(0, maxIndex);
            var selectedObject = sortedInteractions[selectedIndex].TargetObject;
            var selectedInteraction = sortedInteractions[selectedIndex].Interaction;

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

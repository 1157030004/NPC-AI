using System;
using System.Collections.Generic;
using Shadee.Blackboards;
using Shadee.NPC_AI;
using Shadee.UI;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    [System.Serializable]
    public class StatConfiguration
    {
        [field: SerializeField] public Stat LinkedStat { get; private set; }
        [field: SerializeField] public bool OverrideDefaults { get; private set; } = false;
        [field: SerializeField, Range(0f, 1f)] public float Override_InitialValue { get; protected set; } = 0.5f;
        [field: SerializeField, Range(0f, 1f)] public float Override_DecayRate { get; protected set; } = 0.005f;
    }

    [RequireComponent(typeof(BaseNavigation))]
    public class CommonVillager : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private int HouseHoldID = 1;
        [field: SerializeField] StatConfiguration[] Stats;
        [SerializeField] protected FeedbackUIPanel LinkedUI;

        [Header("Traits")]
        [SerializeField] protected List<Trait> Traits;

        protected BaseNavigation Navigation;
        protected BaseInteraction CurrenInteraction 
        {
            get 
            {
                BaseInteraction interaction = null;
                IndividualBackboard.TryGetGeneric(EBlackboardKey.Character_FocusObject, out interaction, null);
                return interaction;
            }
            set 
            { 
                BaseInteraction previousInteraction = null;
                IndividualBackboard.TryGetGeneric(EBlackboardKey.Character_FocusObject, out previousInteraction, null);

                IndividualBackboard.SetGeneric(EBlackboardKey.Character_FocusObject, value);

                List<GameObject> objectsInUse = null;
                HouseHoldBackboard.TryGetGeneric(EBlackboardKey.Household_ObjectsInUse, out objectsInUse, null);

                // are we starting to use something/
                if(value != null)
                {
                    // need to create list?
                    if(objectsInUse == null)
                        objectsInUse = new List<GameObject>();

                    // not already in list? add and update blackboard
                    if(!objectsInUse.Contains(value.gameObject))
                    {
                        objectsInUse.Add(value.gameObject);
                        HouseHoldBackboard.SetGeneric(EBlackboardKey.Household_ObjectsInUse, objectsInUse);
                    }
                } //we've stopped using something 
                else if(objectsInUse != null)
                {
                    // attempt to remove and update blackboard
                    if(objectsInUse.Remove(previousInteraction.gameObject))
                        HouseHoldBackboard.SetGeneric(EBlackboardKey.Household_ObjectsInUse, objectsInUse);
                }
            }
        }
        protected bool StartedPerforming = false;
        protected Dictionary<Stat, float> DecayRates = new Dictionary<Stat, float>();
        protected Dictionary<Stat, StatPanel> StatUIPanel = new Dictionary<Stat, StatPanel>();

        public Blackboard IndividualBackboard {get; protected set; }
        public Blackboard HouseHoldBackboard {get; protected set; }
        protected virtual void Awake() 
        {
            Navigation = GetComponent<BaseNavigation>();
        }

        protected virtual void Start()
        {
            IndividualBackboard = BlackboardManager.Instance.GetIndivualBlackboard(this);
            HouseHoldBackboard = BlackboardManager.Instance.GetSharedBlackboard(HouseHoldID);

            foreach (var statConfig in Stats)
            {
                var linkedStat = statConfig.LinkedStat;
                float initialValue = statConfig.OverrideDefaults ? statConfig.Override_InitialValue : linkedStat.InitialValue;
                float decayRate = statConfig.OverrideDefaults ? statConfig.Override_DecayRate : linkedStat.DecayRate;

                DecayRates[linkedStat] = decayRate;
                IndividualBackboard.SetStat(linkedStat, initialValue);

                if(linkedStat.IsVisible)
                    StatUIPanel[linkedStat] = LinkedUI.AddStat(linkedStat, initialValue);
            }
        }

        protected virtual void OnEnable() 
        {
        }

        protected virtual void OnDisable() 
        {
        }


        protected float ApplyTraitsTo(Stat targetStat, Trait.ETargetType targetType,  float currentValue)
        {
            foreach (var trait in Traits)
            {
                currentValue = trait.Apply(targetStat, targetType, currentValue);
            }

            return currentValue;
        }
        
        protected virtual void Update() 
        {
            if(CurrenInteraction != null)
            {
                if(Navigation.IsAtDestination && !StartedPerforming)
                {
                    StartedPerforming = true;
                    CurrenInteraction.Perform(this, OnInteractionFinished);
                }
            }

            foreach (var statConfig in Stats)
            {
                UpdateIndividualStat(statConfig.LinkedStat, -DecayRates[statConfig.LinkedStat] * Time.deltaTime, Trait.ETargetType.DecayRate);
            }
        }

        protected virtual void OnInteractionFinished(BaseInteraction interaction)
        {
            interaction.UnlockInteraction(this);
            CurrenInteraction = null;
            Debug.Log($"Finished {interaction.DisplayName}");
        }

        public void UpdateIndividualStat(Stat linkedStat, float amount, Trait.ETargetType targetType)
        {
            var adjustedAmount = ApplyTraitsTo(linkedStat, targetType, amount);
            float newValue = Mathf.Clamp01(GetStatValue(linkedStat) + adjustedAmount);

            IndividualBackboard.SetStat(linkedStat, newValue);

            if(linkedStat.IsVisible)
                StatUIPanel[linkedStat].OnStatChanged(newValue);
        }

        public float GetStatValue(Stat linkedStat)
        {
            
            return IndividualBackboard.GetStat(linkedStat);
        }
    }
}

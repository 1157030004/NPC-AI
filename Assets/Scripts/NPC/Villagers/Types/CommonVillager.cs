using System;
using System.Collections;
using System.Collections.Generic;
using Shadee.Blackboards;
using Shadee.NPC_AI;
using UnityEngine;
using UnityEngine.UI;

namespace Shadee.NPC_Villagers
{
    public enum EStat
    {
        Energy,
        Fun,
    }

    [RequireComponent(typeof(BaseNavigation))]

    public class CommonVillager : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private int HouseHoldID = 1;

        [Header("Fun")]
        [SerializeField] float InitialEnergyLevel = 0.5f;
        [SerializeField] float BaseFunDecayRate = 0.05f;
        [SerializeField] Slider FunDisplay;
        
        [Header("Energy")]
        [SerializeField] float InitialFunLevel = 0.5f;
        [SerializeField] float BaseEnergyDecayRate = 0.05f;
        [SerializeField] Slider EnergyDisplay;

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

        public float CurrentFun 
        {
            get { return IndividualBackboard.GetFloat(EBlackboardKey.Character_Stat_Fun); }
            set { IndividualBackboard.Set(EBlackboardKey.Character_Stat_Fun, value); }
        }
        public float CurrentEnergy 
        {
            get { return IndividualBackboard.GetFloat(EBlackboardKey.Character_Stat_Energy); }
            set { IndividualBackboard.Set(EBlackboardKey.Character_Stat_Energy, value); }
        }

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

            FunDisplay.value = CurrentFun = InitialFunLevel;
            EnergyDisplay.value = CurrentEnergy = InitialEnergyLevel;
        }

        protected virtual void OnEnable() 
        {
        }

        protected virtual void OnDisable() 
        {
        }


        protected float ApplyTraitsTo(EStat targetStat, float currentValue, bool isDecay)
        {
            foreach (var trait in Traits)
            {
                currentValue = trait.Apply(targetStat, currentValue, isDecay);
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

            CurrentFun = Mathf.Clamp01(CurrentFun - ApplyTraitsTo(EStat.Fun, BaseFunDecayRate, true) * Time.deltaTime);
            FunDisplay.value = CurrentFun;

            CurrentEnergy = Mathf.Clamp01(CurrentEnergy - ApplyTraitsTo(EStat.Energy, BaseEnergyDecayRate, true) * Time.deltaTime);
            EnergyDisplay.value = CurrentEnergy;
        }

        protected virtual void OnInteractionFinished(BaseInteraction interaction)
        {
            interaction.UnlockInteraction(this);
            CurrenInteraction = null;
            Debug.Log($"Finished {interaction.DisplayName}");
        }

        public void UpdateIndividualStat(EStat target, float amount)
        {
            var adjustedAmount = ApplyTraitsTo(target, amount, false);
            switch(target)
            {
                case EStat.Energy:
                    CurrentEnergy = Mathf.Clamp01(CurrentEnergy + adjustedAmount);
                    break;
                case EStat.Fun:
                    CurrentFun = Mathf.Clamp01(CurrentFun + adjustedAmount);
                    break;
            }
        }
    }
}

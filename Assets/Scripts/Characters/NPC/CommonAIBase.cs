using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shadee
{
    public enum EStat
    {
        Energy,
        Fun,
    }

    [RequireComponent(typeof(BaseNavigation))]

    public class CommonAIBase : MonoBehaviour
    {
        [Header("Fun")]
        [SerializeField] float InitialEnergyLevel = 0.5f;
        [SerializeField] float BaseFunDecayRate = 0.05f;
        [SerializeField] Slider FunDisplay;
        
        [Header("Energy")]
        [SerializeField] float InitialFunLevel = 0.5f;
        [SerializeField] float BaseEnergyDecayRate = 0.05f;
        [SerializeField] Slider EnergyDisplay;

        protected BaseNavigation Navigation;
        protected BaseInteraction CurrenInteraction = null;
        protected DailyScheduler DailyScheduler;
        protected DateScheduler DateScheduler;
        protected DateMonthScheduler DateMonthScheduler;
        protected ScheduleEventArgs ScheduledInteraction;
        protected bool StartedPerforming = false;
        protected bool ShouldStartTimedInteraction = false;

        public float CurrentFun { get; protected set;}
        public float CurrentEnergy { get; protected set;}

        protected virtual void Awake() 
        {
            FunDisplay.value = CurrentFun = InitialFunLevel;
            EnergyDisplay.value = CurrentEnergy = InitialEnergyLevel;
            Navigation = GetComponent<BaseNavigation>();
            DailyScheduler = GetComponent<DailyScheduler>();
            DateScheduler = GetComponent<DateScheduler>();
            DateMonthScheduler = GetComponent<DateMonthScheduler>();
        }

        protected virtual void Start()
        {
          DailyScheduler.Scheduled += OnScheduled;
          DateScheduler.Scheduled += OnScheduled;
          DateMonthScheduler.Scheduled += OnScheduled;  
        }

        protected virtual void OnScheduled(object sender, ScheduleEventArgs e)
        {
            ShouldStartTimedInteraction = true;
            ScheduledInteraction = e;

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

            CurrentFun = Mathf.Clamp01(CurrentFun - BaseFunDecayRate * Time.deltaTime);
            FunDisplay.value = CurrentFun;

            CurrentEnergy = Mathf.Clamp01(CurrentEnergy - BaseEnergyDecayRate * Time.deltaTime);
            EnergyDisplay.value = CurrentEnergy;
        }

        protected virtual void OnInteractionFinished(BaseInteraction interaction)
        {
            interaction.UnlockInteraction();
            CurrenInteraction = null;
            Debug.Log($"Finished {interaction.DisplayName}");
        }

        public void UpdateIndividualStat(EStat target, float amount)
        {
            switch(target)
            {
                case EStat.Energy:
                    CurrentEnergy += amount;
                    break;
                case EStat.Fun:
                    CurrentFun += amount;
                    break;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Shadee.NPC_Villagers
{
    public enum EInteractionType
    {
        Instantaneous,
        OverTime
    }

    [Serializable]
    public class InteractionStatChange
    {
        public Stat LinkedStat;
        public float Value;
    }
    public abstract class BaseInteraction : MonoBehaviour
    {
        [SerializeField] protected string _DisplayName;
        [SerializeField] protected EInteractionType _InteractionType = EInteractionType.Instantaneous;
        [SerializeField] protected float _Duration = 0.0f;
        [SerializeField, FormerlySerializedAs("StatChanges")] protected InteractionStatChange[] _StatChanges;

        public string DisplayName => _DisplayName;
        public EInteractionType InteractionType => _InteractionType;
        public float Duration => _Duration;
        public InteractionStatChange[] StatChanges => _StatChanges;

        public abstract bool CanPerform();
        public abstract bool LockInteraction(CommonVillager performer);
        public abstract bool Perform(CommonVillager performer, UnityAction<BaseInteraction> onCompleted);
        public abstract bool UnlockInteraction(CommonVillager performer);
        public void ApplyStatChanges(CommonVillager performer, float proportion)
        {
            foreach (var statChange in _StatChanges)
            {
                performer.UpdateIndividualStat(statChange.LinkedStat, statChange.Value * proportion, Trait.ETargetType.Impact);
            }
        }

    }
}

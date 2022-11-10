using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    [CreateAssetMenu(menuName = "AI/Trait", fileName = "Trait")]
    public class Trait : ScriptableObject
    {

        public enum ETargetType
        {
            Score,
            Impact,
            DecayRate,
        }

        public string DisplayName;
        public TraitElement[] Impacts;
        public float Apply(Stat targetStat, Trait.ETargetType targetType, float currentValue)
        {
            foreach (var impact in Impacts)
            {
                currentValue = impact.Apply(targetStat, targetType, currentValue);
            }

            return currentValue;
        }
    }
}

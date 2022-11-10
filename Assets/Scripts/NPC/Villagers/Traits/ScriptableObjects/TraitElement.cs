using System;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    [Serializable]
    public class TraitElement
    {
        public EStat Stat;

        [Header("Scoring Factors")]
        [Range(0.5f, 1.5f)] public float PositiveScale = 1f;
        [Range(0.5f, 1.5f)] public float NegativeScale = 1f;

        [Header("Decay Rate")]
        [Range(0.5f, 1.5f)] public float DecayRateScale = 1f;

        public float Apply(EStat targetStat, float currentValue, bool isDecay)
        {
            if(targetStat == Stat)
            {
                if(isDecay)
                    currentValue *= DecayRateScale;
                else if(currentValue > 0)
                    currentValue *= PositiveScale;
                else if(currentValue < 0)
                    currentValue *= NegativeScale;
            }
            return currentValue;
        }
    }
}
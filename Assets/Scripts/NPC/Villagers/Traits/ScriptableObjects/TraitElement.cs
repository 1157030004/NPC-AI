using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shadee.NPC_Villagers
{
    [Serializable]
    public class TraitElement
    {
        public Stat LinkedStat;


        [Header("Scoring Factors")]
        [Range(0.5f, 1.5f)][FormerlySerializedAs("PositiveScale")]  public float Scoring_PositiveScale = 1f;
        [Range(0.5f, 1.5f)][FormerlySerializedAs("NegativeScale")] public float Scoring_NegativeScale = 1f;

        [Header("Impact Scales")]
        [Range(0.5f, 1.5f)] public float Impact_PositiveScale = 1f;
        [Range(0.5f, 1.5f)] public float Impact_NegativeScale = 1f;

        [Header("Decay Rate")]
        [Range(0.5f, 1.5f)] public float DecayRateScale = 1f;

        public float Apply(Stat targetStat, Trait.ETargetType targetType,  float currentValue)
        {
            if(targetStat == LinkedStat)
            {
                if(targetType == Trait.ETargetType.DecayRate)
                    currentValue *= DecayRateScale;
                else if(targetType == Trait.ETargetType.Impact)
                {
                    if (currentValue > 0)
                        currentValue *= Impact_PositiveScale;
                    else
                        currentValue *= Impact_NegativeScale;
                }
                else
                {
                    if (currentValue > 0)
                        currentValue *= Scoring_PositiveScale;
                    else
                        currentValue *= Scoring_NegativeScale;
                }
            }
            return currentValue;
        }
    }
}
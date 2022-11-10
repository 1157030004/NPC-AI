using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    [CreateAssetMenu(menuName = "AI/Trait", fileName = "Trait")]
    public class Trait : ScriptableObject
    {
        public string DisplayName;
        public TraitElement[] Impacts;
        public float Apply(EStat targetStat, float currentValue, bool isDecay)
        {
            foreach (var impact in Impacts)
            {
                currentValue = impact.Apply(targetStat, currentValue, isDecay);
            }

            return currentValue;
        }
    }
}

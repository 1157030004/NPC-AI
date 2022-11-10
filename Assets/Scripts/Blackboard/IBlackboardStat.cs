using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.Blackboards
{
    public interface IBlackboardStat
    {
        [SerializeField] public string DisplayName { get; }
        [SerializeField] public bool IsVisible { get; }
        [SerializeField] public float InitialValue { get; }
        [SerializeField] public float DecayRate { get; }
    }
}

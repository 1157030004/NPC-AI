using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Shadee.Blackboards;

namespace Shadee.UI
{
    public class StatPanel : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI StatName;
        [SerializeField] protected Slider StatValue;

        protected IBlackboardStat LinkedStat;

        public void Bind(IBlackboardStat stat, float initialValue)
        {
            LinkedStat = stat;
            StatName.text = stat.DisplayName;
            StatValue.SetValueWithoutNotify(initialValue);
        }

        public void OnStatChanged(float newValue)
        {
            StatValue.SetValueWithoutNotify(newValue);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using Shadee.WorldTimes;
using TMPro;
using UnityEngine;

namespace Shadee
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] timeText;
        [SerializeField] private TextMeshProUGUI[] dateText;

        private void OnEnable() 
        {
            WorldTimeManager.OnHourChanged += UpdateTime;
            WorldTimeManager.OnMinuteChanged += UpdateTime;
            WorldTimeManager.OnDayChanged += UpdateDate;
            WorldTimeManager.OnMonthChanged += UpdateDate;
            WorldTimeManager.OnYearChanged += UpdateDate;    
        }

        private void OnDisable() 
        {
            WorldTimeManager.OnHourChanged -= UpdateTime;
            WorldTimeManager.OnMinuteChanged -= UpdateTime;
            WorldTimeManager.OnDayChanged -= UpdateDate;
            WorldTimeManager.OnMonthChanged -= UpdateDate;
            WorldTimeManager.OnYearChanged -= UpdateDate;    
        }

        private void Awake() {
            
        }

        private void UpdateTime()
        {
            for (int i = 0; i < timeText.Length; i++)
            {
                timeText[i].text = WorldTimeManager.Instance.Time;
            }

            UpdateDate();
        }

        private void UpdateDate()
        {
            for (int i = 0; i < dateText.Length; i++)
            {
                dateText[i].text = WorldTimeManager.Instance.Date;
            }
        }
    }
}

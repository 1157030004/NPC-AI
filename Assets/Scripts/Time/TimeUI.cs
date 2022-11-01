using System.Collections;
using System.Collections.Generic;
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
            TimeManager.OnHourChanged += UpdateTime;
            TimeManager.OnMinuteChanged += UpdateTime;
            TimeManager.OnDayChanged += UpdateDate;
            TimeManager.OnMonthChanged += UpdateDate;
            TimeManager.OnYearChanged += UpdateDate;    
        }

        private void OnDisable() 
        {
            TimeManager.OnHourChanged -= UpdateTime;
            TimeManager.OnMinuteChanged -= UpdateTime;
            TimeManager.OnDayChanged -= UpdateDate;
            TimeManager.OnMonthChanged -= UpdateDate;
            TimeManager.OnYearChanged -= UpdateDate;    
        }

        private void Awake() {
            
        }

        private void UpdateTime()
        {
            for (int i = 0; i < timeText.Length; i++)
            {
                timeText[i].text = TimeManager.Instance.Time;
            }

            UpdateDate();
        }

        private void UpdateDate()
        {
            for (int i = 0; i < dateText.Length; i++)
            {
                dateText[i].text = TimeManager.Instance.Date;
            }
        }
    }
}

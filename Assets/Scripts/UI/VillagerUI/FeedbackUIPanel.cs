using Shadee.Blackboards;
using UnityEngine;

namespace Shadee.UI
{
    public class FeedbackUIPanel : MonoBehaviour
    {
        [SerializeField] private GameObject StatPanelPrefab;
        [SerializeField] private Transform StatRoot;

        public StatPanel AddStat(IBlackboardStat linkedStat, float initialValue)
        {
            var newGameObject = Instantiate(StatPanelPrefab, StatRoot);
            newGameObject.name = $"Stat_{linkedStat.DisplayName}";
            var statPanelLogic = newGameObject.GetComponent<StatPanel>();
            statPanelLogic.Bind(linkedStat, initialValue);

            return statPanelLogic;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.Blackboards
{
    public class BlackboardManager : MonoBehaviour
    {
        public static BlackboardManager Instance { get; private set; } = null;

        Dictionary<MonoBehaviour, Blackboard> IndividualBlackboards = new Dictionary<MonoBehaviour, Blackboard>();
        Dictionary<int, Blackboard> SharedBlackboards = new Dictionary<int, Blackboard>();

        private void Awake() 
        {
            if(Instance != null)
            {
                Debug.LogError($"Trying to create second BlackboardManager on {gameObject.name}");
                Destroy(gameObject);
                return;
            }    
            Instance = this;    
        }

        public Blackboard GetIndivualBlackboard(MonoBehaviour requestor)
        {
            if(!IndividualBlackboards.ContainsKey(requestor))
                IndividualBlackboards[requestor] = new Blackboard();

            return IndividualBlackboards[requestor];
        }

        public Blackboard GetSharedBlackboard(int sharedID)
        {
            if(!SharedBlackboards.ContainsKey(sharedID))
                SharedBlackboards[sharedID] = new Blackboard();

            return SharedBlackboards[sharedID];
        }
    }
}

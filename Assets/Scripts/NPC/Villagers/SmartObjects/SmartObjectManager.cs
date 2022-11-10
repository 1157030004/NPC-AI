using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    public class SmartObjectManager : MonoBehaviour
    {
        public static SmartObjectManager Instance { get; private set; } = null;
        public List<SmartObject> RegisteredObjects { get; private set; } = new List<SmartObject>();

        private void Awake() 
        {
            if(Instance != null)
            {
                Debug.LogError($"Trying to create second SmartObjectManager on {gameObject.name}");
                Destroy(gameObject);
                return;
            }    
            Instance = this;
        }

        public void RegisterSmartObject(SmartObject toRegister)
        {
            RegisteredObjects.Add(toRegister);
            Debug.Log($"Registered {toRegister.DisplayName}");
        }

        public void UnregisterSmartObject(SmartObject toUnregister)
        {
            RegisteredObjects.Remove(toUnregister);
        }

    }
}

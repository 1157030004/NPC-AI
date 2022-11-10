using UnityEngine;
using UnityEngine.Events;

namespace Shadee.NPC_Villagers
{
    public class RentASpace : SimpleInteraction
    {
        protected SmartObjectCoffeeShop CoffeeShop;
        protected SmartObjectCoffeeSpace CoffeeSpace;

        private void Awake() 
        {
            CoffeeShop =FindObjectOfType<SmartObjectCoffeeShop>();
            CoffeeSpace = GetComponent<SmartObjectCoffeeSpace>();    
        }

        public override bool CanPerform()
        {
            return base.CanPerform() && CoffeeShop.HasBoughtCoffee;
        }

        public override bool Perform(CommonVillager performer, UnityAction<BaseInteraction> onCompleted)
        {
            CoffeeShop.HasBoughtCoffee = false;
            return base.Perform(performer, onCompleted);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace Shadee.NPC_Villagers
{
    [RequireComponent(typeof(SmartObjectCoffeeShop))]
    public class BuyACoffee : SimpleInteraction
    {
        protected SmartObjectCoffeeShop CoffeeShop;

        private void Awake() 
        {
            CoffeeShop = GetComponent<SmartObjectCoffeeShop>();    
        }

        public override bool CanPerform()
        {
            return base.CanPerform() && !CoffeeShop.HasBoughtCoffee;
        }

        public override bool Perform(CommonVillager performer, UnityAction<BaseInteraction> onCompleted)
        {
            CoffeeShop.BuyCoffee();
            return base.Perform(performer, onCompleted);
        }
    }
}

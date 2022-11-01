using UnityEngine;
using UnityEngine.Events;

namespace Shadee
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

        public override void Perform(CommonAIBase performer, UnityAction<BaseInteraction> onCompleted)
        {
            CoffeeShop.BuyCoffee();
            base.Perform(performer, onCompleted);
        }
    }
}

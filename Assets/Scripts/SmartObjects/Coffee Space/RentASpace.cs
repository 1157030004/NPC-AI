using UnityEngine;
using UnityEngine.Events;

namespace Shadee
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

        public override void Perform(CommonAIBase performer, UnityAction<BaseInteraction> onCompleted)
        {
            CoffeeShop.HasBoughtCoffee = false;
            base.Perform(performer, onCompleted);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.NPC_Villagers
{
    public class SmartObjectCoffeeShop : SmartObject
    {
        public bool HasBoughtCoffee { get; set; } = false;

        public void BuyCoffee()
        {
            HasBoughtCoffee = true;

            Debug.Log($"You {(HasBoughtCoffee ? "have" : "haven't")} bought a coffee");
        }

    }
}

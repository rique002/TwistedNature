using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayableCharacters
{
    public class PlayerInventory : MonoBehaviour
    {
        public DynamicInventory inventory;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out InstanceItemContainer foundItem))
            {
                inventory.AddItem(foundItem.TakeItem());
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayableCharacters
{

    [CreateAssetMenu]
    public class DynamicInventory : ScriptableObject
    {
        public int maxItems = 10;
        public List<ItemInstance> items = new();

        public bool AddItem(ItemInstance itemToAdd)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] == null)
                {
                    items[i] = itemToAdd;
                    return true;
                }
            }

            if (items.Count < maxItems)
            {
                items.Add(itemToAdd);
                return true;
            }

            Debug.Log("No space in the inventory");
            return false;
        }

        public void RemoveItem(ItemInstance itemToRemove)
        {
            items.Remove(itemToRemove);
        }
    }
}

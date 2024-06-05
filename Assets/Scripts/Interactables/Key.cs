using UnityEngine;
using PlayableCharacters;

namespace Interactables
{
    public class Key : Interactable
    {
        public InstanceItemContainer key; 

        private void Awake() {
            key = GetComponent<InstanceItemContainer>();
        }

        public override void Interact()
        {
            Debug.Log("Picked up " + key.item.itemType.name);
            GameObject.Find("Inventory").GetComponent<DynamicInventory>().AddItem(key.item);
            Destroy(gameObject);
        }
    }
}
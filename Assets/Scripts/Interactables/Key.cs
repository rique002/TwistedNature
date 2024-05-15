using UnityEngine;
using PlayableCharacters;

namespace Interactables
{
    public class Key : Interactable
    {
        public int keyId; 

        public override void Interact()
        {
            Debug.Log("Picked up " + keyId);
            Inventory.Instance.AddKey(keyId);
            Destroy(gameObject);
        }
    }
}
using UnityEngine;
using PlayableCharacters;

namespace Interactables
{
    public class Chest : Interactable
    {
        [SerializeField] public int triangleId; 

        public override void Interact()
        {
            Debug.Log("Picked up " + triangleId);
            Inventory.Instance.AddTriangle(triangleId);
            Destroy(gameObject);
        }
    }
}
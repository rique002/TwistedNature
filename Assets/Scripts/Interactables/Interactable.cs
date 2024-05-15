using UnityEngine;

namespace Interactables{
    public class Interactable : MonoBehaviour
    {
        public virtual void Interact()
        {
            Debug.Log("Interacting with " + gameObject.name);
        }
            
    }
}
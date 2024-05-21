using PlayableCharacters;
using UnityEngine; 

namespace Interactables
{
    public class Door : Interactable
    {
        [SerializeField] private Animator animator;
        public int requiredKeyId;
        private bool isOpen = false; 

        

        public override void Interact()
        {
            if (Inventory.Instance.HasKey(requiredKeyId))
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("You need the key with ID " + requiredKeyId + " to open this door.");
            }
        }

        private void OpenDoor()
        {
            if (!isOpen)
            {
                Debug.Log("Opened the door.");
                animator.SetTrigger("Open");
                isOpen = true;
            }
            else
            {
                Debug.Log("Closed the door.");
                animator.SetTrigger("Close");
                isOpen = false;
            }
        }
    }
}
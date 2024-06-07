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
        }

        public void OpenDoor()
        {
            if (!isOpen)
            {
                animator.SetTrigger("Open");
                isOpen = true;
            }
            else
            {
                animator.SetTrigger("Close");
                isOpen = false;
            }
        }
    }
}
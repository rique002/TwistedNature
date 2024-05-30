using UnityEngine;
using System.Collections.Generic;
namespace Interactables{
public class Lever : Interactable
    {       
        [SerializeField] private Animator animator;

        [SerializeField] private bool isOn = false;
        [SerializeField] private List<Door> doors;
        public override void Interact()
        {
            base.Interact();
            if(isOn)
            {
                animator.SetTrigger("Close");
                print("Closed the door.");
                

            }
            else
            {
                animator.SetTrigger("Open");
                print("Opened the door.");
                
            }
            foreach (var door in doors)
                {
                    door.OpenDoor();
                }
            isOn = !isOn;
            
        }
        
    }
}
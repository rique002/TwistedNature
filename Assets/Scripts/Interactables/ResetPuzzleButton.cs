using UnityEngine;
using System.Collections.Generic;
namespace Interactables{
public class ResetPuzzleButton : Interactable
    {
        
        [SerializeField] public List<PressurePlate> pressurePlates;

        [SerializeField] private Door door;
        [SerializeField] public List<PressurePlate> solution;
        
        public override void Interact()
        {
            base.Interact();
            CheckSolution();

        }

        public void CheckSolution()
        {
            bool solved = true;
            foreach (var plate in pressurePlates)
            {
                if (!solution.Contains(plate))
                {
                    if(plate.IsActive())
                    {
                        solved = false;
                        break;
                    }
                }
                else
                {
                    if (!plate.IsActive())
                    {
                        solved = false;
                        break;
                    }
                }
   
            }

            if (solved)
            {
                door.OpenDoor();
            }
            else
            {
                ResetPuzzle();
            }
        }

        public void ResetPuzzle()
        {
            print("Resetting puzzle");
            foreach (var plate in pressurePlates)
            {
                print(plate + " is being deactivated");
                plate.Deactivate();
            }
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
namespace Interactables
{
    public class BarrierPlates : Interactable
    {
        [SerializeField] public List<PressurePlate> pressurePlates;
        [SerializeField] public List<PressurePlate> solution;

        bool toSolve = true;

        private void Update()
        {
            if (toSolve)
            {
                CheckSolution();
            }
        }

        private void OpenBarrier()
        {
            print("Barrier opened.");
            Destroy(gameObject);
        }

        public void CheckSolution()
        {
            bool solved = true;
            foreach (var plate in pressurePlates)
            {
                if (!solution.Contains(plate))
                {
                    if (plate.IsActive())
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
                OpenBarrier();
                print("Puzzle solved");
                toSolve = false;
            }
        }



    }
}
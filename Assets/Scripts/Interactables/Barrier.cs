using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
namespace Interactables{
public class Barrier : Interactable
    {       
        [SerializeField] private List<Totem> totems;
        [SerializeField] private List<String> solution;
        
        private void Update()
        {
            if (totems.Count == 3)
            {
                if (solution[0] == totems[0].GetActiveShape() && solution[1] == totems[1].GetActiveShape() && solution[2] == totems[2].GetActiveShape())
                {
                    OpenBarrier();
                }
            }
        } 

        private void OpenBarrier()
        {
            print("Barrier opened.");
            Destroy(gameObject);
        }
    

        
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace Interactables
{
    public class Totem : Interactable
    {
        [SerializeField] GameObject cube;
        [SerializeField] GameObject sphere;
        [SerializeField] GameObject piramid;


        [SerializeField] private List<Door> doors;

        private void Start()
        {
            cube.SetActive(true);
            sphere.SetActive(false);
            piramid.SetActive(false);
        }
        public override void Interact()
        {
            base.Interact();
            if (cube.activeSelf)
            {
                cube.SetActive(false);
                sphere.SetActive(true);
            }
            else if (sphere.activeSelf)
            {
                sphere.SetActive(false);
                piramid.SetActive(true);
            }
            else if (piramid.activeSelf)
            {
                piramid.SetActive(false);
                cube.SetActive(true);
            }
        }
        public string GetActiveShape()
        {
            if (cube.activeSelf)
            {
                return "Cube";
            }
            else if (sphere.activeSelf)
            {
                return "Sphere";
            }
            else if (piramid.activeSelf)
            {
                return "Piramid";
            }
            return "Cube";
        }
    }
}
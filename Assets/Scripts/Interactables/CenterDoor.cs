using PlayableCharacters;
using UnityEngine; 

namespace Interactables
{
    public class CenterDoor : Interactable
    {

        [SerializeField] private GameObject triangleLeft;
        [SerializeField] private GameObject triangleRight;
        [SerializeField] private GameObject triangleCenter;
        [SerializeField] private Animator animator;
        private bool isOpen = false; 

        

        private Animator triangleLeftAnimator;
        private Animator triangleRightAnimator;
        private Animator triangleCenterAnimator; 


        private bool triangleLeftPlaced = false;
        private bool triangleRightPlaced = false;
        private bool triangleCenterPlaced = false;

        private void Start()
        {
            triangleLeftAnimator = triangleLeft.GetComponent<Animator>();
            triangleRightAnimator = triangleRight.GetComponent<Animator>();
            triangleCenterAnimator = triangleCenter.GetComponent<Animator>();
        }

        

        public override void Interact()
        {
            base.Interact();
            if (Inventory.Instance.HasTriangle(0))
            {
                triangleCenterAnimator.SetTrigger("Place");
                triangleCenterPlaced = true;
            }
            if (Inventory.Instance.HasTriangle(1))
            {
                triangleLeftAnimator.SetTrigger("Place");
                triangleLeftPlaced = true;
            }
            if (Inventory.Instance.HasTriangle(2))
            {
                triangleRightAnimator.SetTrigger("Place");
                triangleRightPlaced = true;
            }
            else
            {
                Debug.Log("You need 3 triangle shaped object to open this door.");
            }
        }

        private void Update()
        {
            if (triangleLeftPlaced && triangleRightPlaced && triangleCenterPlaced)
            {

                if (!isOpen)
                {
                    isOpen = true;
                    Invoke("OpenDoor", 1f);
                }
            }
        }
        private void OpenDoor()
        {
            animator.SetTrigger("Open");
        }
    }
}
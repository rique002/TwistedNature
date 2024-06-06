using System.Collections;
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

        [SerializeField] private GameObject boss;
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
            Invoke("TriggerOpenDoor", 1f);
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
                    Invoke("TriggerOpenDoor", 1f);
                }
            }
        }
       private IEnumerator OpenDoor()
{
    int layerMask = LayerMask.GetMask("Player");

    // Create a sphere in front of the door
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f, layerMask);
    foreach (var hitCollider in hitColliders)
    {
        print("Collided with - " + hitCollider.name);
        // If the player is hit, push them to the X, -Z direction
        Rigidbody playerRigidbody = hitCollider.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            Vector3 forceDirection = new Vector3(1, 0.1f, -1).normalized;
            playerRigidbody.AddForce(forceDirection * 100, ForceMode.Impulse);
            print("Pushed player");
        }
    }

    // Wait for 1 second
    yield return new WaitForSeconds(1f);

    if (boss != null)
    {
        Quaternion rotation = Quaternion.LookRotation(Quaternion.Euler(0, -135, 0) * Vector3.forward);
        Instantiate(boss, new Vector3(transform.position.x+2,transform.position.y,transform.position.z-2), rotation);
    }
}

// Call the coroutine from another method
public void TriggerOpenDoor()
{
    StartCoroutine(OpenDoor());
}
    }
}
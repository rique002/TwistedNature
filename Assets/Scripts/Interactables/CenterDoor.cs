using System;
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
        [SerializeField] private GameObject bossEnemies;

        [SerializeField] private GameObject boss;
        private GameObject bossInstance;

        private bool bossSpawned = false;
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
        }

        private void Update()
        {
            if (triangleLeftPlaced && triangleRightPlaced && triangleCenterPlaced)
            {

                if (!isOpen && !bossSpawned)
                {
                    isOpen = true;
                    Invoke("TriggerOpenDoor", 1f);
                }
            }
        }
        private IEnumerator PushPlayer(Rigidbody playerRigidbody)
        {
            Vector3 forceDirection = new Vector3(10, 0.2f, -10);
            for (int i = 0; i < 50; i++)
            {
                playerRigidbody.AddForce(forceDirection, ForceMode.VelocityChange);
                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator OpenDoor()
        {
            int layerMask = LayerMask.GetMask("Player");

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f, layerMask);
            foreach (var hitCollider in hitColliders)
            {
                print("Collided with - " + hitCollider.name);
                Rigidbody playerRigidbody = hitCollider.GetComponent<Rigidbody>();
                if (playerRigidbody != null)
                {
                    StartCoroutine(PushPlayer(playerRigidbody));
                    print("Pushed player");
                }
            }

            yield return new WaitForSeconds(1f);

            if (boss != null)
            {
                Quaternion rotation = Quaternion.LookRotation(Quaternion.Euler(0, -135, 0) * Vector3.forward);
                bossInstance = Instantiate(boss, new Vector3(transform.position.x + 2, transform.position.y, transform.position.z - 2), rotation);
                Enemy enemy = bossInstance.GetComponent<Enemy>();
                enemy.OnEnemyKilled += (object sender, EventArgs e) =>
                {
                    animator.SetTrigger("Open");
                    Destroy(bossEnemies);
                };
                bossSpawned = true;
                bossEnemies.SetActive(true);
            }
        }

        public void TriggerOpenDoor()
        {
            StartCoroutine(OpenDoor());
        }
    }
}
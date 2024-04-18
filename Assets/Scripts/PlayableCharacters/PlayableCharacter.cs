using System;
using UnityEngine;

namespace PlayableCharacters {
    public abstract class PlayableCharacter<T> : MonoBehaviour, IPlayableCharacter where T : PlayableCharacter<T> {
        [SerializeField] protected GameInput gameInput;
        [SerializeField] protected int healthPoints;
        [SerializeField] protected float attackDamage;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float rotationSpeed;

        protected enum State { 
            Idle,
            Dead,
        }

        public static T Instance { get; private set; }
        public event EventHandler OnPlayableCharacterKilled;

        private void Awake() {
            if (Instance != null) {
                Debug.LogError("There is more than one Instance of " + GetType());
            }

            Instance = (T) this;
        }

        private void Update() {
            HandleMovement();
            HandleInteractions();
        }

        private void HandleMovement() {
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

            float moveDistance = moveSpeed * Time.deltaTime;

            transform.position += moveDirection * moveDistance;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }

        public void TakeDamage(int damage) {
            healthPoints -= damage;

            if (healthPoints < 0) {
                healthPoints = 0;

                OnPlayableCharacterKilled?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetActive(bool active) {
            if (gameObject != null) {
                gameObject.SetActive(active);
            }
        }

        public Transform GetTransform() {
            return transform;
        }

        public void SetPosition(Vector3 position) {
            transform.position = position;
        }

        public void SetForward(Vector3 forward) {
            transform.forward = forward;
        }

        protected abstract void HandleInteractions();
    }
}

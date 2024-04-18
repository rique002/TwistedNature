using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayableCharacters {
    public abstract class PlayableCharacter : MonoBehaviour {
        [SerializeField] protected GameInput gameInput;
        [SerializeField] protected int healthPoints;
        [SerializeField] protected float attackDamage;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float rotationSpeed;
        
        private static Dictionary<Type, PlayableCharacter> instances = new Dictionary<Type, PlayableCharacter>();

        protected enum State { 
            Idle,
            Dead,
        }

        protected State state;

        public event EventHandler OnPlayableCharacterKilled;

        private void Awake() {
            Type type = GetType();

            if (instances.ContainsKey(type)) {
                Debug.LogError("There is more than one instance of " + type);
            } else {
                instances.Add(type, this);
            }

            state = State.Idle;
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
                state = State.Dead;

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

        public bool IsDead() {
            return state == State.Dead;
        }

        protected abstract void HandleInteractions();
    }
}

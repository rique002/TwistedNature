using System;
using UnityEngine;

namespace PlayableCharacters {
    public class PlayableCharacter : MonoBehaviour, IInteractableCharacter {
        [SerializeField] protected GameInput gameInput;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float rotationSpeed;
        [SerializeField] protected float damage;

        public void Update() {
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

            HandleMovement(moveDirection);
            HandleInteractions();
        }

        protected virtual void HandleMovement(Vector3 moveDirection) {
            throw new NotImplementedException();
        }

        protected virtual void HandleInteractions() {
            throw new NotImplementedException();
        }
    }
}
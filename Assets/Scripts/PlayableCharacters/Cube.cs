using System;
using UnityEngine;

namespace PlayableCharacters {
    public class Cube : PlayableCharacter {
        protected override void HandleMovement(Vector3 moveDirection) {
            float moveDistance = moveSpeed * Time.deltaTime;

            transform.position += moveDirection * moveDistance;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }

        protected override void HandleInteractions() {
        }
    }
}
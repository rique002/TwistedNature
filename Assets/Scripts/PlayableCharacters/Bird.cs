using System;
using UnityEngine;

namespace PlayableCharacters
{
    public class Bird : PlayableCharacter
    {
        [SerializeField] PlayerWeaponCollider nozzleCollider;
        [SerializeField] private float flyingSpeed;

        private bool isFlying;

        protected override void Init()
        {
            state = State.Idle;
            isFlying = false;
            nozzleCollider.SetDamage(attackDamage);

            gameInput.OnFlyAction += GameInput_OnFlyAction;
        }

        protected override void HandleMovement()
        {
            float currentMoveSpeed = isFlying ? flyingSpeed : moveSpeed;

            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            playerBody.velocity = new Vector3(inputVector.x * currentMoveSpeed, playerBody.velocity.y, inputVector.y * currentMoveSpeed);

            if (inputVector != Vector2.zero)
            {
                state = State.Mooving;
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.y));
                playerBody.MoveRotation(Quaternion.Slerp(playerBody.rotation, targetRotation, Time.deltaTime * rotationSpeed));
            }
            else
            {
                state = State.Idle;
            }
        }

        protected override void HandleAnimations()
        {
            if (state == State.Idle)
            {
                animator.SetBool("Running", false);
            }
            else if (state == State.Mooving)
            {
                animator.SetBool("Running", true);
            }

            if (isFlying)
            {
                animator.SetBool("Flying", true);
            }
            else
            {
                animator.SetBool("Flying", false);
            }

        }

        protected override void HandleAttack()
        {
            nozzleCollider.StartAttack();
            animator.SetTrigger("Attack");
        }

        private void GameInput_OnFlyAction(object sender, EventArgs e)
        {
            isFlying = !isFlying;
        }

        public void EndAttack()
        {
            nozzleCollider.EndAttack();
        }
    }
}
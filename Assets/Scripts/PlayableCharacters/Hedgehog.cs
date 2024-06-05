using System;
using System.Collections;
using UnityEngine;

namespace PlayableCharacters
{
    public class Hedgehog : PlayableCharacter
    {
        [SerializeField] private PlayerWeaponCollider rightFistCollider;
        [SerializeField] private PlayerWeaponCollider leftFistCollider;

        private bool isDashing = false;
        private bool isJumping = false;

        protected override void Init()
        {
            isDashing = false;
            isJumping = false;

            rightFistCollider.SetDamage(attackDamage);
            leftFistCollider.SetDamage(attackDamage);

            gameInput.OnDashAction += GameInput_OnDashAction;
            gameInput.OnJumpAction += GameInput_OnJumpAction;
        }

        protected override void HandleMovement()
        {
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            playerBody.velocity = new Vector3(inputVector.x * moveSpeed, playerBody.velocity.y, inputVector.y * moveSpeed);

            if (inputVector != Vector2.zero)
            {
                state = State.Mooving;
                if(cameraSwitcher.isMain()){
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.y));
                    playerBody.MoveRotation(Quaternion.Slerp(playerBody.rotation, targetRotation, Time.deltaTime * rotationSpeed));
           
                }
                else{
                    playerBody.velocity = transform.forward * inputVector.y * moveSpeed;
                    if(inputVector.x>0){
                        transform.Rotate(Vector3.up, rotationSpeed*10 * Time.deltaTime);
                    }
                    else if(inputVector.x<0){
                        transform.Rotate(Vector3.up, -rotationSpeed*10 * Time.deltaTime);
                    }
                }
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
        }

        protected override void HandleAttack()
        {
            rightFistCollider.StartAttack();
            leftFistCollider.StartAttack();
            animator.SetTrigger("Attack");
        }

        public override void EndAttack()
        {
            rightFistCollider.EndAttack();
            leftFistCollider.EndAttack();
        }

        private void GameInput_OnDashAction(object sender, EventArgs e)
        {
            if (!isDashing && isActiveAndEnabled)
            {
                StartCoroutine(Dash());
            }
        }

        private IEnumerator Dash()
        {
            isDashing = true;

            Vector3 dashDirection = transform.forward;
            dashDirection.y = 0;
            playerBody.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);
            yield return new WaitForSeconds(dashDuration);

            float yVelocity = playerBody.velocity.y;
            playerBody.velocity = new Vector3(0, yVelocity, 0);

            isDashing = false;

        }

        private void GameInput_OnJumpAction(object sender, EventArgs e)
        {
            Debug.Log(this.name + " is jumping");
            if (!isJumping && isActiveAndEnabled)
            {
                StartCoroutine(Jump());
            }
        }

        private IEnumerator Jump()
        {
            isJumping = true;

            playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            yield return new WaitForSeconds(0.5f);

            isJumping = false;
        }
    }
}
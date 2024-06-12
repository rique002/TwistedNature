using System;
using System.Collections;
using FMOD.Studio;
using UnityEditor;
using UnityEngine;

namespace PlayableCharacters
{
    public class Hedgehog : PlayableCharacter
    {
        [SerializeField] private float dashForce;
        [SerializeField] private float dashDuration;
        [SerializeField] private float jumpForce;
        [SerializeField] private PlayerWeaponCollider rightFistCollider;
        [SerializeField] private PlayerWeaponCollider leftFistCollider;

        private bool isDashing;
        private bool isJumping;
        private EventInstance footSteps;

        protected override void Init()
        {
            isDashing = false;
            isJumping = false;

            rightFistCollider.Init(attackDamage, FMODEvents.Instance.HedgehogHit);
            leftFistCollider.Init(attackDamage, FMODEvents.Instance.HedgehogHit);

            footSteps = AudioManager.Instance.CreateInstance(FMODEvents.Instance.HedgehogFootsteps);

            gameInput.OnDashAction += GameInput_OnDashAction;
            gameInput.OnJumpAction += GameInput_OnJumpAction;
        }

        protected override void HandleMovement()
        {
            if (transform.position.y < -3)
            {
                transform.position = spawnPoint.position;
                isDashing = false;
                isJumping = false;
                state = State.Idle;
                animator.SetBool("Running", false);
                ReceiveDamage(5);
                return;
            }

            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            playerBody.velocity = new Vector3(inputVector.x * moveSpeed, playerBody.velocity.y, inputVector.y * moveSpeed);

            if (inputVector != Vector2.zero)
            {
                state = State.Mooving;
                if (cameraSwitcher.isMain())
                {
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(inputVector.x, 0, inputVector.y));
                    playerBody.MoveRotation(Quaternion.Slerp(playerBody.rotation, targetRotation, Time.deltaTime * rotationSpeed));

                }
                else
                {
                    playerBody.velocity = inputVector.y * moveSpeed * transform.forward;
                    if (inputVector.x > 0)
                    {
                        transform.Rotate(Vector3.up, rotationSpeed * 10 * Time.deltaTime);
                    }
                    else if (inputVector.x < 0)
                    {
                        transform.Rotate(Vector3.up, -rotationSpeed * 10 * Time.deltaTime);
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

        protected override void UpdateSound()
        {
            if (playerBody.velocity.magnitude > 0.1f)
            {
                footSteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
                footSteps.getPlaybackState(out PLAYBACK_STATE playbackState);

                if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
                {
                    footSteps.start();
                }
            }
            else
            {
                footSteps.stop(STOP_MODE.ALLOWFADEOUT);
            }
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
            print(!isJumping + "and" + isActiveAndEnabled);
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

        public override void Deactivate()
        {
            EndAttack();
            isJumping = false;
            isDashing = false;
            animator.SetBool("Running", false);
            gameObject.SetActive(false);
        }

        protected override void PlayDamageSFX()
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.HedgehogDamage, transform.position);
        }
    }
}
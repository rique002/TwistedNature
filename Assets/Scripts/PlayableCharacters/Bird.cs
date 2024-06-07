using System;
using UnityEngine;
using FMOD.Studio;

namespace PlayableCharacters
{
    public class Bird : PlayableCharacter
    {
        [SerializeField] PlayerWeaponCollider nozzleCollider;
        [SerializeField] private float flyingSpeed;

        private bool isFlying;
        private EventInstance footSteps;

        protected override void Init()
        {
            state = State.Idle;
            isFlying = false;
            nozzleCollider.SetDamage(attackDamage);
            //footSteps = AudioManager.Instance.CreateInstance(FMODEvents.Instance.PlayerFootsteps);

            gameInput.OnFlyAction += GameInput_OnFlyAction;
        }

        protected override void HandleMovement()
        {
            if (transform.position.y < -3)
            {
                transform.position = spawnPoint.position;
                isFlying = false;
                state = State.Idle;
                animator.SetBool("Flying", false);
                animator.SetBool("Running", false);
                return;
            }

            float currentMoveSpeed = isFlying ? flyingSpeed : moveSpeed;
            float velocityY = isFlying ? 0 : playerBody.velocity.y;

            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            playerBody.velocity = new Vector3(inputVector.x * currentMoveSpeed, velocityY, inputVector.y * currentMoveSpeed);

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

        public override void EndAttack()
        {
            nozzleCollider.EndAttack();
        }

        public override void Deactivate()
        {
            EndAttack();
            isFlying = false;
            state = State.Idle;
            animator.SetBool("Flying", false);
            animator.SetBool("Running", false);
            gameObject.SetActive(false);
        }

        protected override void UpdateSound()
        {
            if (playerBody.velocity.magnitude > 0.1f && !isFlying)
            {
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
    }
}
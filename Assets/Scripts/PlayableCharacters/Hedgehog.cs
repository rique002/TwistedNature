using UnityEngine;

namespace PlayableCharacters
{
    public class Hedgehog : PlayableCharacter
    {
        [SerializeField] private PlayerWeaponCollider rightFistCollider;
        [SerializeField] private PlayerWeaponCollider leftFistCollider;

        protected override void InitWeapon()
        {
            rightFistCollider.SetDamage(attackDamage);
            leftFistCollider.SetDamage(attackDamage);
        }

        protected override void HandleAnimations()
        {
            if (state == State.Idle)
            {
                animator.SetBool("Running", false);
            }
            else if (state == State.Running)
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

        public void EndAttack()
        {
            rightFistCollider.EndAttack();
            leftFistCollider.EndAttack();
        }
    }
}
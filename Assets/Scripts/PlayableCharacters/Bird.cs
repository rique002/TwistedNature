using System;
using UnityEngine;

namespace PlayableCharacters
{
    public class Bird : PlayableCharacter
    {
        [SerializeField] PlayerWeaponCollider nozzleCollider;

        protected override void Init()
        {
            nozzleCollider.SetDamage(attackDamage);
        }

        protected override void HandleAnimations()
        {
            // Currently no running/flying animations
        }

        protected override void HandleAttack()
        {
            nozzleCollider.StartAttack();
            animator.SetTrigger("Attack");
        }

        public void EndAttack()
        {
            nozzleCollider.EndAttack();
        }
    }
}
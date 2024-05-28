using UnityEngine;

namespace PlayableCharacters
{
    public class Hedgehog : PlayableCharacter
    {
        public override void HandleAnimations()
        {
            Debug.Log("Hedgehog HandleAnimations");
            if (state == State.Idle)
            {
                animator.SetBool("Running", false);
            }
            else if (state == State.Running)
            {
                animator.SetBool("Running", true);
            }
        }
    }
}
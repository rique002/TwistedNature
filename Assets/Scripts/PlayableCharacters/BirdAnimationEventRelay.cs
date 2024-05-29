using PlayableCharacters;
using UnityEngine;

public class BirdAnimationEventRelay : MonoBehaviour
{
    [SerializeField] private Bird bird;

    public void EndAttack()
    {
        bird.EndAttack();
    }
}

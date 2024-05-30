using PlayableCharacters;
using UnityEngine;

public class HedgehogAnimationEventRelay : MonoBehaviour
{
    [SerializeField] private Hedgehog hedgehog;

    public void EndAttack()
    {
        hedgehog.EndAttack();
    }
}

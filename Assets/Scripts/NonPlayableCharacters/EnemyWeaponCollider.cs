using PlayableCharacters;
using UnityEngine;

public class EnemyWeaponCollider : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    private bool isAttacking;
    private float damage;

    void Start()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<PlayableCharacter>(out var player))
            {
                player.ReceiveDamage(damage);
            }
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void StartAttack()
    {
        Debug.Log("Weapon is attacking!");
        isAttacking = true;
    }

    public void EndAttack()
    {
        Debug.Log("Weapon stopped attacking!");
        isAttacking = false;
    }
}

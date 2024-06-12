using FMODUnity;
using UnityEngine;

public class PlayerWeaponCollider : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    private bool isAttacking;
    private float damage;
    private EventReference hitSound;

    void Start()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && enemyLayer == (enemyLayer | (1 << other.gameObject.layer)))
        {
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                AudioManager.Instance.PlayOneShot(hitSound, transform.position, 0.5f);
                enemy.ReceiveDamage(damage);
            }
        }
    }

    public void Init(float damage, EventReference hitSound)
    {
        this.damage = damage;
        this.hitSound = hitSound;
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}

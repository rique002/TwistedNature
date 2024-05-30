using UnityEngine;

public class PlayerWeaponCollider : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    private bool isAttacking;
    private float damage;

    void Start()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking && enemyLayer == (enemyLayer | (1 << other.gameObject.layer)))
        {
            Debug.Log("Enemy hit by " + gameObject.name);
            if (other.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.ReceiveDamage(damage);
            }
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
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

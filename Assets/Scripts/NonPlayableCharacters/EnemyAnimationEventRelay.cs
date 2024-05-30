using UnityEngine;

public class EnemyAnimationEventRelay : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    public void Walk()
    {
        enemy.Walk();
    }

    public void Stop()
    {
        enemy.Stop();
    }

    public void StartCollision()
    {
        enemy.StartCollision();
    }

    public void EndCollision()
    {
        enemy.EndCollision();
    }

    public void EndAttack()
    {
        enemy.EndAttack();
    }
}

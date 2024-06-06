using System.Collections.Generic;
using UnityEngine;
using UI;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float maxHealthPoints;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private float viewDistance = 10.0f;
    [SerializeField] private string enemyName;
    [SerializeField] private float attackDamage;
    [SerializeField] private GameObject model;
    [SerializeField] private EnemyWeaponCollider weaponCollider;
    [SerializeField] private HealthBar healthBar;

    public event EventHandler OnEnemyKilled;

    private Transform player;
    private bool isWalking = false;
    private bool playerDetected = false;
    private bool closeToPlayer = false;
    private Animator animator;
    private State state;
    private float healthPoints;

    private enum State { Idle, Mooving, Attacking, Dead };

    private void Start()
    {
        healthPoints = maxHealthPoints;
        animator = model.GetComponent<Animator>();
        state = State.Idle;
        weaponCollider.SetDamage(attackDamage);
    }

    private void Update()
    {
        if (state == State.Attacking || state == State.Dead) return;

        if (player == null || !player.gameObject.activeInHierarchy)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject potentialPlayer in players)
            {
                if (potentialPlayer.activeInHierarchy)
                {
                    player = potentialPlayer.transform;
                    break;
                }
            }
        }

        closeToPlayer = Vector3.Distance(player.position, transform.position) < 15.0f;

        if (closeToPlayer)
        {
            healthBar.SetName(enemyName);
            healthBar.gameObject.SetActive(true);
            healthBar.SetValue(healthPoints / maxHealthPoints);

        }
        else
        {
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
        }

        playerDetected = PlayerInFieldOfView();

        if (playerDetected)
        {
            Attack();

            if (state != State.Attacking)
            {
                animator.SetBool("Running", true);
                if (isWalking)
                {
                    ChasePlayer();
                }
            }
        }
        else
        {
            animator.SetBool("Running", false);
            state = State.Idle;
        }
    }

    private bool PlayerInFieldOfView()
    {
        Vector3 directionToPlayer = transform.InverseTransformPoint(player.position);
        float angle = Vector3.Angle(-Vector3.right, directionToPlayer);

        Vector3 rayDrawPos = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
        Debug.DrawRay(rayDrawPos, Quaternion.Euler(0, -fieldOfView / 2, 0) * -transform.right * viewDistance, Color.red);
        Debug.DrawRay(rayDrawPos, Quaternion.Euler(0, fieldOfView / 2, 0) * -transform.right * viewDistance, Color.red);

        if (angle < fieldOfView / 2 && directionToPlayer.magnitude < viewDistance)
        {
            Debug.DrawRay(rayDrawPos, transform.TransformDirection(directionToPlayer), Color.blue);
            return true;
        }
        return false;
    }

    private void ChasePlayer()
    {
        state = State.Mooving;
        Vector3 direction = player.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            toRotation *= Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < 2.0f)
        {
            state = State.Attacking;
            animator.SetTrigger("Attack");
        }
    }

    public void StartCollision()
    {
        weaponCollider.StartAttack();
    }

    public void EndCollision()
    {
        weaponCollider.EndAttack();
    }

    public void EndAttack()
    {
        state = State.Mooving;
        weaponCollider.EndAttack();
    }

    public void ReceiveDamage(float damage)
    {
        healthPoints -= damage;
        if (healthPoints < 0.0f)
        {
            healthPoints = 0.0f;
            OnEnemyKilled?.Invoke(this, EventArgs.Empty);
            animator.SetTrigger("Death");
            state = State.Dead;
        }
    }

    public void Walk()
    {
        isWalking = true;
    }

    public void Stop()
    {
        isWalking = false;
    }

    public void Destroy()
    {
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }
}
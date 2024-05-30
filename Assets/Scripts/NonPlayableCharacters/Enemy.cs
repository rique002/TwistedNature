using System.Collections.Generic;
using UnityEngine;
using UI;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private PlayerManager gameManager;
    [SerializeField] private Transform player;
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] protected float maxHealthPoints;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private float viewDistance = 10.0f;
    [SerializeField] private LayerMask viewMask;
    [SerializeField] private string enemyName;
    [SerializeField] private ParticleSystem attackParticles;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDamage;
    [SerializeField] private GameObject model;
    [SerializeField] private EnemyWeaponCollider weaponCollider;

    bool isWalking = false;
    private int waypointIndex = 0;
    private bool playerDetected = false;
    private bool closeToPlayer = false;
    private Animator animator;
    private State state;
    private float healthPoints;

    private enum State { Mooving, Attacking };

    private void Start()
    {
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[waypointIndex].position;
        }
        healthPoints = maxHealthPoints;
        animator = model.GetComponent<Animator>();
        state = State.Mooving;
        weaponCollider.SetDamage(attackDamage);

        gameManager.OnActivePlayerChaged += GameManager_OnActivePlayerChaged;
    }

    private void Update()
    {
        closeToPlayer = Vector3.Distance(player.position, transform.position) < 5.0f;

        if (closeToPlayer)
        {
            healthBar.SetName(enemyName);
            healthBar.gameObject.SetActive(true);
            healthBar.SetValue(healthPoints / maxHealthPoints);
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            screenPosition.y += 200;
            healthBar.transform.position = screenPosition;
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }

        if (state == State.Attacking) return;

        playerDetected = PlayerInFieldOfView();

        if (playerDetected) Attack();

        if (state == State.Mooving && isWalking)
        {
            if (!playerDetected)
            {
                MoveEnemy();
            }
            else
            {
                ChasePlayer();
            }
        }
    }

    private void GameManager_OnActivePlayerChaged(object sender, PlayerManager.OnActivePlayerChangedEventArgs e)
    {
        player = e.playerTransform;
    }

    private bool PlayerInFieldOfView()
    {
        Vector3 directionToPlayer = transform.InverseTransformPoint(player.position);
        float angle = Vector3.Angle(-Vector3.right, directionToPlayer);

        Debug.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfView / 2, 0) * -transform.right * viewDistance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, fieldOfView / 2, 0) * -transform.right * viewDistance, Color.red);

        if (angle < fieldOfView / 2 && directionToPlayer.magnitude < viewDistance)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(directionToPlayer), Color.green);
            return true;
        }
        return false;
    }

    private void MoveEnemy()
    {
        if (waypoints.Count == 0) return;

        Vector3 direction = waypoints[waypointIndex].position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            toRotation *= Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, waypoints[waypointIndex].position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
        }
    }

    private void ChasePlayer()
    {
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
            Destroy(gameObject);
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
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI;
using Interactables;
using UI;

namespace PlayableCharacters
{
    public abstract class PlayableCharacter : MonoBehaviour
    {
        [SerializeField] protected GameInput gameInput;
        [SerializeField] protected float maxHealthPoints;
        [SerializeField] public float attackDamage;
        [SerializeField] public float attackRange;

        [SerializeField] protected float interactionDistance = 1.0f;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float rotationSpeed;
        [SerializeField] private Image cooldownImage;
        [SerializeField] private ParticleSystem attackParticles;
        [SerializeField] private float attackCooldown;

        [SerializeField] private float dashForce;
        [SerializeField] private float dashDuration = 0.2f;

        [SerializeField] private float jumpForce;
        [SerializeField] private Text interactTextUI;

        [SerializeField] private InteractionBar interactionBar;

        [SerializeField] public GameObject projectilePrefab;
        [SerializeField] public float projectileSpeed = 10f;

        private bool isDashing = false;

        private bool isJumping = false;
        private bool isAttackOnCooldown = false;

        private static readonly Dictionary<Type, PlayableCharacter> instances = new();
        private readonly List<StatusEffect> statusEffects = new();

        protected enum State
        {
            Idle,
            Dead,
        }

        public enum StatusEffectType
        {
            Haste,
            Poison,
            Fire,
            // Other status effects...
        }

        public struct StatusEffect
        {
            public StatusEffectType Type { get; private set; }
            public float Duration { get; private set; }

            public StatusEffect(StatusEffectType type, float duration = 0.0f)
            {
                Type = type;
                Duration = duration;
            }

            public void SetDuration(float duration)
            {
                Duration = duration;
            }

            public void UpdateDuration(float elapsedTime)
            {
                Duration -= elapsedTime;
                Debug.Log("Poison Duration: " + Duration);

                if (Duration <= 0.0f)
                {
                    Duration = 0.0f;
                }
            }
        }

        protected State state;
        protected float healthPoints;

        public event EventHandler OnPlayableCharacterKilled;
        public event EventHandler<OnPlayableCharacterHealthChangeArgs> OnPlayableCharacterHealthChange;
        public class OnPlayableCharacterHealthChangeArgs : EventArgs
        {
            public float healthPercentage;
        }

        private void Awake()
        {
            Type type = GetType();

            if (instances.ContainsKey(type))
            {
                Debug.LogError("There is more than one instance of " + type);
            }
            else
            {
                instances.Add(type, this);
            }

            state = State.Idle;
            healthPoints = maxHealthPoints;
        }

        private void Start()
        {
            attackParticles.Stop();
            gameInput.OnAttackAction += GameInput_OnAttackAction;
            gameInput.OnDashAction += GameInput_OnDashAction;
            gameInput.OnInteractAction += GameInput_OnInteractAction;
            gameInput.OnJumpAction += GameInput_OnJumpAction;
        }

        private void Update()
        {
            HandleMovement();
            HandleInteractions();
            HandleStatusEffects();
        }

        private void HandleMovement()
        {
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

            float moveDistance = moveSpeed * Time.deltaTime;

            transform.position += moveDirection * moveDistance;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }

        private void HandleInteractions()
        {
            // Test For Applying Status Effects
            if (Input.GetKey(KeyCode.DownArrow))
            {
                AddStatusEffect(StatusEffectType.Poison, 5.0f);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                AddStatusEffect(StatusEffectType.Fire, 10.0f);
            }

            if(Physics.OverlapSphere(transform.position, interactionDistance, LayerMask.GetMask("Interactable")).Length > 0)
            {
                interactTextUI.gameObject.SetActive(true);
            }
            else
            {
                interactionBar.SetActive(false);
                interactTextUI.gameObject.SetActive(false);
            }
            // if stepping on pressure plate, activate it
            /*if (Physics.OverlapSphere(transform.position,0.2f, LayerMask.GetMask("PressurePlate")).Length > 0)
            {
                PressurePlate pressurePlate = Physics.OverlapSphere(transform.position, 0.2f, LayerMask.GetMask("PressurePlate"))[0].GetComponent<PressurePlate>();
                if (pressurePlate != null)
                {
                    print("Stepping on pressure plate");
                    pressurePlate.Activate();
                }
            }*/
        }

        private void HandleStatusEffects()
        {
            for (int i = statusEffects.Count - 1; i >= 0; i--)
            {
                StatusEffect statusEffect = statusEffects[i];
                statusEffect.UpdateDuration(Time.deltaTime);
                statusEffects[i] = statusEffect;

                if (statusEffect.Duration == 0.0f)
                {
                    statusEffects.RemoveAt(i);
                    continue;
                }

                switch (statusEffect.Type)
                {
                    case StatusEffectType.Poison:
                        float poisonDamagePerSecond = 1.0f;
                        ReceiveDamage(poisonDamagePerSecond * Time.deltaTime);
                        break;
                    case StatusEffectType.Fire:
                        float fireDamagePerSecond = 2.0f;
                        ReceiveDamage(fireDamagePerSecond * Time.deltaTime);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void AddStatusEffect(StatusEffectType statusEffectType, float duration = 0.0f)
        {
            foreach (StatusEffect statusEffect in statusEffects)
            {
                if (statusEffect.Type == statusEffectType)
                {
                    // If yes, set its duration
                    statusEffect.SetDuration(duration);
                    return;
                }
            }

            // If not, add a new status effect
            statusEffects.Add(new StatusEffect(statusEffectType, duration));
        }

        public void ReceiveDamage(float damage)
        {
            healthPoints -= damage;
            OnPlayableCharacterHealthChange?.Invoke(this, new OnPlayableCharacterHealthChangeArgs
            {
                healthPercentage = GetHealthPercentage()
            });

            if (healthPoints < 0.0f)
            {
                healthPoints = 0.0f;
                state = State.Dead;

                OnPlayableCharacterKilled?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GameInput_OnAttackAction(object sender, EventArgs e)
        {
            if (isAttackOnCooldown)
            {
                return;
            }
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, LayerMask.GetMask("Enemy"));
            foreach (Collider enemy in hitEnemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.ReceiveDamage(attackDamage);
                }
            }

            float offset = 1.2f; 
            int numberOfProjectiles = 5;
            float spreadAngle = 45f; 

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float projectileDirectionAngle = -spreadAngle / 2 + spreadAngle / (numberOfProjectiles - 1) * i;
                Quaternion projectileRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + projectileDirectionAngle, 0);
                Vector3 spawnPosition = transform.position + transform.forward * offset;
                spawnPosition.y += 1f;
                GameObject projectile = Instantiate(projectilePrefab, spawnPosition, projectileRotation);
            }

            attackParticles.transform.position = transform.position;
            attackParticles.Play();
            isAttackOnCooldown = true;
            StartCoroutine(StartAttackCooldown());
        }

        private void GameInput_OnInteractAction(object sender, EventArgs e)
        {    
            Collider[] hitInteractables = Physics.OverlapSphere(transform.position,interactionDistance, LayerMask.GetMask("Interactable"));
            if(hitInteractables.Length > 0){
                print("Interacting with " + hitInteractables[0].name);
                Interactable interactable = hitInteractables[0].GetComponent<Interactable>();
                if(interactable != null){
                    interactable.Interact();
                }
            }
        }

        private void GameInput_OnJumpAction(object sender, EventArgs e)
        {
            if (!isJumping)
            {
                StartCoroutine(Jump());
            }
        }

        private IEnumerator Jump()
        {
            isJumping = true;

            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            yield return new WaitForSeconds(0.5f);

            isJumping = false;
        }


        private void GameInput_OnDashAction(object sender, EventArgs e)
        {
            if (!isDashing)
            {
                StartCoroutine(Dash());
            }
        }

        private IEnumerator Dash()
        {
            isDashing = true;

            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 dashDirection = transform.forward;
                dashDirection.y = 0;
                rb.AddForce(dashDirection * dashForce, ForceMode.VelocityChange);
            }

            yield return new WaitForSeconds(dashDuration);

            if (rb != null)
            {

                 float yVelocity = rb.velocity.y;
                rb.velocity = new Vector3(0, yVelocity, 0);
                }

            isDashing = false;

        }
        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        private IEnumerator StartAttackCooldown()
        {
            isAttackOnCooldown = true;
            float cooldownRemaining = attackCooldown;

            while (cooldownRemaining > 0)
            {
                cooldownRemaining -= Time.deltaTime;
                cooldownImage.fillAmount = cooldownRemaining / attackCooldown;
                yield return null;
            }

            cooldownImage.fillAmount = 0;
            isAttackOnCooldown = false;
        }

        public void SetActive(bool active)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(active);
            }
        }

        public float GetHealthPercentage()
        {
            return healthPoints / maxHealthPoints;
        }

        public Transform GetTransform()
        {
            return transform;
        }



        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetForward(Vector3 forward)
        {
            transform.forward = forward;
        }

        public bool IsDead()
        {
            return state == State.Dead;
        }
    }
}

using System;
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
        [SerializeField] protected float attackDamage;
        [SerializeField] protected float attackRange;
        [SerializeField] protected GameInput gameInput;
        [SerializeField] protected float maxHealthPoints;
        [SerializeField] protected float interactionDistance = 1.0f;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float rotationSpeed;
        [SerializeField] protected Image cooldownImage;
        [SerializeField] protected Rigidbody playerBody;
        [SerializeField] protected ParticleSystem attackParticles;
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected GameObject model;
        [SerializeField] protected Text interactTextUI;
        [SerializeField] protected InteractionBar interactionBar;
        [SerializeField] protected CameraSwitcher cameraSwitcher;
        [SerializeField] protected Transform spawnPoint;

        protected State state;
        protected float healthPoints;
        protected bool isAttackOnCooldown = false;
        protected static readonly Dictionary<Type, PlayableCharacter> instances = new();
        protected readonly List<StatusEffect> statusEffects = new();
        protected Animator animator;

        public event EventHandler OnPlayableCharacterKilled;
        public event EventHandler<OnPlayableCharacterHealthChangeArgs> OnPlayableCharacterHealthChange;
        public class OnPlayableCharacterHealthChangeArgs : EventArgs
        {
            public float healthPercentage;
        }

        protected enum State
        {
            Idle,
            Mooving,
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

        protected abstract void Init();
        protected abstract void HandleMovement();
        protected abstract void HandleAnimations();
        protected abstract void HandleAttack();
        public abstract void EndAttack();
        public abstract void Deactivate();

        private void Awake()
        {
            Type type = GetType();

            if (instances.ContainsKey(type))
            {
                instances.Remove(type);
                instances.Add(type, this);
            }
            else
            {
                instances.Add(type, this);
            }

            state = State.Idle;
            healthPoints = maxHealthPoints;
            animator = model.GetComponent<Animator>();
        }

        private void Start()
        {
            Init();
            gameInput.OnAttackAction += GameInput_OnAttackAction;
            gameInput.OnInteractAction += GameInput_OnInteractAction;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleInteractions();
            HandleAnimations();
            HandleStatusEffects();
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

            if (Physics.OverlapSphere(transform.position, interactionDistance, LayerMask.GetMask("Interactable")).Length > 0)
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
            HandleAttack();
        }

        private void GameInput_OnInteractAction(object sender, EventArgs e)
        {
            Collider[] hitInteractables = Physics.OverlapSphere(transform.position, interactionDistance, LayerMask.GetMask("Interactable"));
            if (hitInteractables.Length > 0)
            {
                print("Interacting with " + hitInteractables[0].name);
                Interactable interactable = hitInteractables[0].GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        // private IEnumerator StartAttackCooldown()
        // {
        //     isAttackOnCooldown = true;
        //     float cooldownRemaining = attackCooldown;

        //     while (cooldownRemaining > 0)
        //     {
        //         cooldownRemaining -= Time.deltaTime;
        //         cooldownImage.fillAmount = cooldownRemaining / attackCooldown;
        //         yield return null;
        //     }

        //     cooldownImage.fillAmount = 0;
        //     isAttackOnCooldown = false;
        // }

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
            return instances[GetType()].transform;
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

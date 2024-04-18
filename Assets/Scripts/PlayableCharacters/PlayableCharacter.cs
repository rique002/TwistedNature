using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayableCharacters {
    public abstract class PlayableCharacter : MonoBehaviour {
        [SerializeField] protected GameInput gameInput;
        [SerializeField] protected float maxHealthPoints;
        [SerializeField] protected float attackDamage;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float rotationSpeed;

        private static readonly Dictionary<Type, PlayableCharacter> instances = new();
        private readonly List<StatusEffect> statusEffects = new();

        protected enum State {
            Idle,
            Dead,
        }

        public enum StatusEffectType {
            Haste,
            Poison,
            Fire,
            // Other status effects...
        }

        public struct StatusEffect {
            public StatusEffectType Type { get; private set; }
            public float Duration { get; private set; }

            public StatusEffect(StatusEffectType type, float duration = 0.0f) {
                Type = type;
                Duration = duration;
            }

            public void SetDuration(float duration) {
                Duration = duration;
            }

            public void UpdateDuration(float elapsedTime) {
                Duration -= elapsedTime;
                Debug.Log("Poison Duration: " + Duration);

                if (Duration <= 0.0f) {
                    Duration = 0.0f;
                }
            }
        }

        protected State state;
        protected float healthPoints;

        public event EventHandler OnPlayableCharacterKilled;

        private void Awake() {
            Type type = GetType();

            if (instances.ContainsKey(type)) {
                Debug.LogError("There is more than one instance of " + type);
            } else {
                instances.Add(type, this);
            }

            state = State.Idle;
            healthPoints = maxHealthPoints;
        }

        private void Update() {
            HandleMovement();
            HandleInteractions();
            HandleStatusEffects();
        }

        private void HandleMovement() {
            Vector2 inputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

            float moveDistance = moveSpeed * Time.deltaTime;

            transform.position += moveDirection * moveDistance;
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);
        }

        private void HandleInteractions() { 
            // Test For Applying Status Effects
            if (Input.GetKey(KeyCode.DownArrow)) {
                AddStatusEffect(StatusEffectType.Poison, 5.0f);
            }

            if (Input.GetKey(KeyCode.UpArrow)) {
                AddStatusEffect(StatusEffectType.Fire, 10.0f);
            }

            Debug.Log("Current Health: " + healthPoints);
        }

        private void HandleStatusEffects() {
            for (int i = statusEffects.Count - 1; i >= 0; i--) {
                StatusEffect statusEffect = statusEffects[i];
                statusEffect.UpdateDuration(Time.deltaTime);
                statusEffects[i] = statusEffect;

                if (statusEffect.Duration == 0.0f) {
                    statusEffects.RemoveAt(i);
                    continue;
                }

                switch (statusEffect.Type) {
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

        public void AddStatusEffect(StatusEffectType statusEffectType, float duration = 0.0f) {
            foreach (StatusEffect statusEffect in statusEffects) {
                if (statusEffect.Type == statusEffectType) {
                    // If yes, set its duration
                    statusEffect.SetDuration(duration);
                    return;
                }
            }

            // If not, add a new status effect
            statusEffects.Add(new StatusEffect(statusEffectType, duration));
        }

        public void ReceiveDamage(float damage) {
            healthPoints -= damage;

            if (healthPoints < 0.0f) {
                healthPoints = 0.0f;
                state = State.Dead;

                OnPlayableCharacterKilled?.Invoke(this, EventArgs.Empty);
            }
        }

        public void SetActive(bool active) {
            if (gameObject != null) {
                gameObject.SetActive(active);
            }
        }

        public Transform GetTransform() {
            return transform;
        }

        public void SetPosition(Vector3 position) {
            transform.position = position;
        }

        public void SetForward(Vector3 forward) {
            transform.forward = forward;
        }

        public bool IsDead() {
            return state == State.Dead;
        }
    }
}

using UnityEngine;
using System;
using PlayableCharacters;
using UI;
using Image = UnityEngine.UI.Image;
using System.Collections;

public class PlayerManager : MonoBehaviour {
    [SerializeField] private PlayableCharacter[] playableCharacters;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Image cooldownImage;

    [SerializeField] private float dashForce;

    private ParticleSystem attackParticles;

    private PlayableCharacter activeCharacter;
    private int indexActiveCharacter;

    public event EventHandler OnPlayerGameOver;
    public event EventHandler<OnActivePlayerChangedEventArgs> OnActivePlayerChaged;
    public class OnActivePlayerChangedEventArgs : EventArgs {
        public PlayableCharacter activeCharacter;
    }
    private float attackCooldown;    
    private bool isAttackOnCooldown = false;


    private void Start() {
        foreach (PlayableCharacter playableCharacter in playableCharacters)
        {
            playableCharacter.OnPlayableCharacterHealthChange += PlayerManager_OnPlayableCharacterHealthChange;
            playableCharacter.OnPlayableCharacterKilled += PlayerManager_OnPlayableCharacterKilled;
            playableCharacter.SetActive(false);
        }
        activeCharacter = playableCharacters[0];
        attackParticles = activeCharacter.attackParticles;
        attackCooldown = activeCharacter.attackCooldown;
        indexActiveCharacter = 0;
        activeCharacter.SetActive(true);
        attackParticles.Stop();

        gameInput.OnSwapAction += GameInput_OnSwapAction;
        gameInput.OnAttackAction += GameInput_OnAttackAction;
        gameInput.OnDashAction += GameInput_OnDashAction;
    }
    
    private void PlayerManager_OnPlayableCharacterHealthChange(object sender, PlayableCharacter.OnPlayableCharacterHealthChangeArgs e) {
        healthBar.SetValue(e.healthPercentage);
    }

    private void PlayerManager_OnPlayableCharacterKilled(object sender, EventArgs e) {
        foreach (PlayableCharacter playableCharacter in playableCharacters) {
            if (!playableCharacter.IsDead()) {
                // Swap Character
                GameInput_OnSwapAction(this, EventArgs.Empty);
                return;
            }
        }

        // Game Over
        OnPlayerGameOver?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnSwapAction(object sender, EventArgs e) {
        Transform currentTransform = activeCharacter.GetTransform();
        activeCharacter.SetActive(false);

        do {
            indexActiveCharacter = (indexActiveCharacter + 1) % playableCharacters.Length;
        } while (playableCharacters[indexActiveCharacter].IsDead());

        activeCharacter = playableCharacters[indexActiveCharacter];
        activeCharacter.SetPosition(currentTransform.position);
        activeCharacter.SetForward(currentTransform.forward);
        activeCharacter.SetActive(true);
        
        healthBar.SetValue(activeCharacter.GetHealthPercentage());
        attackParticles = activeCharacter.attackParticles;
        attackCooldown = activeCharacter.attackCooldown;

        OnActivePlayerChaged?.Invoke(this, new OnActivePlayerChangedEventArgs {
            activeCharacter = activeCharacter,
        });
    }

    private void GameInput_OnAttackAction(object sender, EventArgs e) 
    {
        if (isAttackOnCooldown)
        {
            return;
        }
        Collider[] hitEnemies = Physics.OverlapSphere(activeCharacter.transform.position, activeCharacter.attackRange, LayerMask.GetMask("Enemy"));
        foreach (Collider enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.ReceiveDamage(activeCharacter.attackDamage);
            }
        }
        attackParticles.transform.position = activeCharacter.transform.position;
        attackParticles.Play();
        StartCoroutine(StartAttackCooldown());
    }

    private void GameInput_OnDashAction(object sender, EventArgs e){
        Rigidbody rb = activeCharacter.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dashDirection = activeCharacter.transform.forward;
            rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        }
    }

    void OnDrawGizmos()
{
    // Draw a red wire sphere representing the attack range
    Gizmos.color = Color.red;
    if (activeCharacter != null)
    {
        Gizmos.DrawWireSphere(activeCharacter.transform.position, activeCharacter.attackRange);
    }
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


}

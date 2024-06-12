using UnityEngine;
using System;
using PlayableCharacters;
using UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayableCharacter[] playableCharacters;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameOverScreen gameOverScreen;

    [SerializeField] private WonScreen wonScreen;
    private PlayableCharacter activeCharacter;
    private int indexActiveCharacter;

    public bool canChange = false;

    public event EventHandler<OnActivePlayerChangedEventArgs> OnActivePlayerChaged;
    public class OnActivePlayerChangedEventArgs : EventArgs
    {
        public Transform playerTransform;
    }

    public void AddCharacter()
    {
        canChange = true;
    }


    private void Start()
    {
        gameOverScreen.Hide();
        wonScreen.Hide();

        foreach (PlayableCharacter playableCharacter in playableCharacters)
        {
            playableCharacter.OnPlayableCharacterHealthChange += PlayerManager_OnPlayableCharacterHealthChange;
            playableCharacter.OnPlayableCharacterKilled += PlayerManager_OnPlayableCharacterKilled;
            playableCharacter.SetActive(false);
        }

        gameInput.OnSwapAction += GameInput_OnSwapAction;
        gameInput.OnSkipAction += GameInput_OnSkipAction;
        activeCharacter = playableCharacters[0];
        indexActiveCharacter = 0;
        activeCharacter.SetActive(true);
    }

    private void FixedUpdate()
    {
        transform.position = activeCharacter.GetTransform().position;
    }

    private void PlayerManager_OnPlayableCharacterHealthChange(object sender, PlayableCharacter.OnPlayableCharacterHealthChangeArgs e)
    {
        healthBar.SetValue(e.healthPercentage);
    }

    private void PlayerManager_OnPlayableCharacterKilled(object sender, EventArgs e)
    {
        foreach (PlayableCharacter playableCharacter in playableCharacters)
        {
            if (!playableCharacter.IsDead() && canChange)
            {
                // Swap Character
                GameInput_OnSwapAction(this, EventArgs.Empty);
                return;
            }
        }

        activeCharacter.SetActive(false);

        // Game Over
        gameOverScreen.Show();
        AudioManager.Instance.StopBackgroundMusic();
        StartCoroutine(AudioManager.Instance.PlayTimedShot(FMODEvents.Instance.GameOverMusic, transform.position, 3.5f));
    }

    private void GameInput_OnSwapAction(object sender, EventArgs e)
    {

        if (!canChange)
        {
            return;
        }
        Transform currentTransform = activeCharacter.GetTransform();
        activeCharacter.Deactivate();

        do
        {
            indexActiveCharacter = (indexActiveCharacter + 1) % playableCharacters.Length;
        } while (playableCharacters[indexActiveCharacter].IsDead());

        activeCharacter = playableCharacters[indexActiveCharacter];
        activeCharacter.SetPosition(currentTransform.position);
        activeCharacter.SetForward(currentTransform.forward);
        activeCharacter.SetActive(true);
        activeCharacter.EndAttack();

        healthBar.SetValue(activeCharacter.GetHealthPercentage());

        OnActivePlayerChaged?.Invoke(this, new OnActivePlayerChangedEventArgs
        {
            playerTransform = activeCharacter.GetTransform(),
        });
    }

    private void GameInput_OnSkipAction(object sender, EventArgs e)
    {
        {
            Tutorial tutorial = FindObjectOfType<Tutorial>();
            if (tutorial != null)
            {
                tutorial.skipTutorial();
            }
        }
    }
}
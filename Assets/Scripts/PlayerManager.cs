using UnityEngine;
using System;
using PlayableCharacters;
using UI;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayableCharacter[] playableCharacters;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Inventory inventory;

    private PlayableCharacter activeCharacter;
    private int indexActiveCharacter;

    private List<int> keys = new List<int>();

    public event EventHandler OnPlayerGameOver;
    public event EventHandler<OnActivePlayerChangedEventArgs> OnActivePlayerChaged;
    public class OnActivePlayerChangedEventArgs : EventArgs
    {
        public PlayableCharacter activeCharacter;
    }


    private void Start()
    {
        foreach (PlayableCharacter playableCharacter in playableCharacters)
        {
            playableCharacter.OnPlayableCharacterHealthChange += PlayerManager_OnPlayableCharacterHealthChange;
            playableCharacter.OnPlayableCharacterKilled += PlayerManager_OnPlayableCharacterKilled;
            playableCharacter.SetActive(false);
        }
        gameInput.OnSwapAction += GameInput_OnSwapAction;
        activeCharacter = playableCharacters[0];
        indexActiveCharacter = 0;
        activeCharacter.SetActive(true);
    }

    private void PlayerManager_OnPlayableCharacterHealthChange(object sender, PlayableCharacter.OnPlayableCharacterHealthChangeArgs e)
    {
        healthBar.SetValue(e.healthPercentage);
    }

    private void PlayerManager_OnPlayableCharacterKilled(object sender, EventArgs e)
    {
        foreach (PlayableCharacter playableCharacter in playableCharacters)
        {
            if (!playableCharacter.IsDead())
            {
                // Swap Character
                GameInput_OnSwapAction(this, EventArgs.Empty);
                return;
            }
        }

        // Game Over
        OnPlayerGameOver?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnSwapAction(object sender, EventArgs e)
    {
        Transform currentTransform = activeCharacter.GetTransform();
        activeCharacter.SetActive(false);

        do
        {
            indexActiveCharacter = (indexActiveCharacter + 1) % playableCharacters.Length;
        } while (playableCharacters[indexActiveCharacter].IsDead());

        activeCharacter = playableCharacters[indexActiveCharacter];
        activeCharacter.SetPosition(currentTransform.position);
        activeCharacter.SetForward(currentTransform.forward);
        activeCharacter.SetActive(true);

        healthBar.SetValue(activeCharacter.GetHealthPercentage());

        OnActivePlayerChaged?.Invoke(this, new OnActivePlayerChangedEventArgs
        {
            activeCharacter = activeCharacter,
        });
    }
}
using UnityEngine;
using PlayableCharacters;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField] private GameObject[] playableCharactersGameObjects;
    [SerializeField] private GameInput gameInput;

    private IPlayableCharacter[] playableCharacters;
    private IPlayableCharacter activeCharacter;
    private int indexActiveCharcter;

    public event EventHandler<OnActivePlayerChangedEventArgs> OnActivePlayerChaged;
    public class OnActivePlayerChangedEventArgs : EventArgs {
        public IPlayableCharacter activeCharacter;
    }

    private void Start() {
        playableCharacters = new IPlayableCharacter[playableCharactersGameObjects.Length];

        for (int i = 0; i < playableCharactersGameObjects.Length; i++) {
            IPlayableCharacter playableCharacter = playableCharactersGameObjects[i].GetComponent<IPlayableCharacter>();
            
            if (playableCharacter == null) {
                Debug.LogError("Game Object " + playableCharacter + " does not have a component that implements IPlayableCharacter");
            }

            playableCharacters[i] = playableCharacter;
            playableCharacters[i].SetActive(false);
        }

        activeCharacter = playableCharacters[0];
        indexActiveCharcter = 0;
        activeCharacter.SetActive(true);

        gameInput.OnSwapAction += GameInput_OnSwapAction;
    }

    private void GameInput_OnSwapAction(object sender, System.EventArgs e) {
        Transform currentTransform = activeCharacter.GetTransform();
        activeCharacter.SetActive(false);
        
        indexActiveCharcter = (indexActiveCharcter + 1) % playableCharacters.Length;
        activeCharacter = playableCharacters[indexActiveCharcter];

        activeCharacter.SetPosition(currentTransform.position);
        activeCharacter.SetForward(currentTransform.forward);
        activeCharacter.SetActive(true);

        OnActivePlayerChaged?.Invoke(this, new OnActivePlayerChangedEventArgs {
            activeCharacter = activeCharacter,
        });
    }
}

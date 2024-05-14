using UnityEngine;
using PlayableCharacters;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected float interactionDistance = 3f;

    [SerializeField] protected KeyCode interactionKey = KeyCode.E;

    [SerializeField] protected string interactionPrompt = "Press E to interact";

    [SerializeField] protected PlayableCharacter playableCharacter;

    public virtual void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, playableCharacter.gameObject.transform.position) <=
            interactionDistance)
        {
            Debug.Log(interactionPrompt);


            if (Input.GetKeyDown(interactionKey))
            {
                Interact();
            }
        }
    }
}
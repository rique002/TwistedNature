using Interactables;
using UnityEngine;
using UI;
public class NPC : Interactable
{
    [SerializeField] private string message;

    [SerializeField] private string answer1;

    [SerializeField] private string answer2;
    [SerializeField] private InteractionBar uiInteractText;

    public override void Interact()
    {
        base.Interact();
        uiInteractText.SetActive(true);
        uiInteractText.SetText(message);
        uiInteractText.SetName(gameObject.name); 
        uiInteractText.SetAnswerText(answer1, answer2);   
        uiInteractText.SetNPC(true); 
    }

}
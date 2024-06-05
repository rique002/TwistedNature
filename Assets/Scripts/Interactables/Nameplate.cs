using Interactables;
using UnityEngine;
using UI;
public class Nameplate : Interactable
{
    [SerializeField] private string message;
    [SerializeField] private InteractionBar uiInteractText;

    public override void Interact()
    {
        base.Interact();
        uiInteractText.SetActive(true);
        uiInteractText.SetText(message);
        uiInteractText.SetName("Board");
        uiInteractText.SetNPC(false);
    }

}
using UnityEngine;

public class CustomInteractable : Interactable
{
    public override void Interact()
    {
        base.Interact(); // Call the base Interact method if needed

        // Implement custom interaction behavior here
        Debug.Log("Custom interaction with " + gameObject.name);
    }
}


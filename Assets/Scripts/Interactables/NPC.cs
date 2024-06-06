using Interactables;
using UnityEngine;
using UI;
using System.Collections;
public class NPC : Interactable
{
    [SerializeField] private string message;

    [SerializeField] private string answer1;

    [SerializeField] private string answer2;

    [SerializeField] private string finalMessage;
    
    [SerializeField] private InteractionBar uiInteractText;

    [SerializeField] private PlayerManager playerManager;

    public override void Interact()
    {
        base.Interact();
        uiInteractText.SetActive(true);
        uiInteractText.SetText(message);
        uiInteractText.SetName(gameObject.name); 
        uiInteractText.SetAnswerText(answer1, answer2);   
        uiInteractText.SetNPC(true); 
    }

    public void Update(){
        if(uiInteractText.answer1Clicked){
            Answer1();
        }
    }

        public void Answer1()
    {
        uiInteractText.SetText(finalMessage);
        uiInteractText.SetAnswerText("", "");
        uiInteractText.SetNPC(false);
        
        StartCoroutine(DestroyAfterDelay(2f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerManager.addCharacter();
        Destroy(gameObject);
        
    }

}
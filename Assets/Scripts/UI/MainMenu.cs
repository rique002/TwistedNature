using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] Animator seaAnim;
    [SerializeField] Animator cloudAnim;

    [SerializeField] Animator rainAnim;

    [SerializeField] Animator animator;
    [SerializeField] GameObject thunder;

    public void Play()
    {
        animator.SetTrigger("hide");
        seaAnim.SetTrigger("move");
        cloudAnim.SetTrigger("move");
        rainAnim.SetTrigger("move");
        StartCoroutine(ActivateThunderAfterDelay(27f));
        //SceneManager.LoadScene("GameScene");
    }
    private IEnumerator ActivateThunderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        thunder.SetActive(true);
    }

   public void Quit()
    {
        Application.Quit();
    }
}

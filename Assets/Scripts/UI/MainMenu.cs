using System.Collections;
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
    }

    private IEnumerator ActivateThunderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        thunder.SetActive(true);

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(7f);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

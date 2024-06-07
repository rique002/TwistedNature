using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

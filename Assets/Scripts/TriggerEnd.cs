using UnityEngine;

public class TriggerEnd : MonoBehaviour
{
    [SerializeField] private WonScreen endScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            endScreen.Show();
            Time.timeScale = 0.0f;
        }
    }
}
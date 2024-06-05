using UnityEngine;

public class TriggerLeave : MonoBehaviour
{
    [SerializeField] private CameraSwitcher cameraSwitcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cameraSwitcher.toMain();
        }
    }
}
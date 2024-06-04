using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    [SerializeField] private CameraSwitcher cameraSwitcher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cameraSwitcher.toFP();
        }
    }
}
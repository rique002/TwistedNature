using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerManager gameManager;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;
    [SerializeField] private Vector3 offset;

    private Vector3 currentVelocity = Vector3.zero;

    private void Start()
    {
        transform.position = target.position + offset;
        gameManager.OnActivePlayerChaged += GameManager_OnActivePlayerChaged;
    }

    private void LateUpdate()
    {
        Vector3 followPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref currentVelocity, smoothTime);
    }

    private void GameManager_OnActivePlayerChaged(object sender, PlayerManager.OnActivePlayerChangedEventArgs e)
    {
        target = e.playerTransform;
    }
}
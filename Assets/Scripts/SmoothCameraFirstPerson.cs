using UnityEngine;

public class SmoothCameraFirstPerson : MonoBehaviour {
    [SerializeField] private PlayerManager gameManager;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;

    
    
    private Vector3 currentVelocity = Vector3.zero;

    private void Start() {
        gameManager.OnActivePlayerChaged += GameManager_OnActivePlayerChaged;
    }

    private void LateUpdate() {
        // Position the camera at the player's position
        Vector3 followPosition = new Vector3(target.position.x, target.position.y + 1.0f, target.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref currentVelocity, smoothTime);

        // Rotate the camera based on the player's rotation
        transform.rotation = target.rotation;
    }

    private void GameManager_OnActivePlayerChaged(object sender, PlayerManager.OnActivePlayerChangedEventArgs e) {
        target = e.playerTransform;
    }
}
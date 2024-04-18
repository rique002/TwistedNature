using System;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime;

    private Vector3 offset;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake() {
        offset = transform.position - target.position;
    }

    private void Start() {
        gameManager.OnActivePlayerChaged += GameManager_OnActivePlayerChaged;
    }

    private void LateUpdate() {
        Vector3 followPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, followPosition, ref currentVelocity, smoothTime);
    }

    private void GameManager_OnActivePlayerChaged(object sender, GameManager.OnActivePlayerChangedEventArgs e) {
        target = e.activeCharacter.GetTransform();
    }
}
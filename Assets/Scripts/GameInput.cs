using System;
using UnityEngine;

public class GameInput : MonoBehaviour {
    private PlayerInputActions playerInputActions;

    public event EventHandler OnSwapAction;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Swap.performed += Swap_performed;
    }

    private void Swap_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSwapAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}
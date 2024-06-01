using System;
using UnityEngine;

public class GameInput : MonoBehaviour {
    private PlayerInputActions playerInputActions;

    public event EventHandler OnSwapAction;
    public event EventHandler OnAttackAction;
    public event EventHandler OnDashAction;

    public event EventHandler OnJumpAction;

    public event EventHandler OnInteractAction;

    public event EventHandler OnSkipAction;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Swap.performed += Swap_performed;
        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Dash.performed += Dash_performed;
        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Skip.performed += Skip_performed;

    }

    private void Swap_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSwapAction?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    private void Dash_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void Skip_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSkipAction?.Invoke(this, EventArgs.Empty);
    }
    
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}
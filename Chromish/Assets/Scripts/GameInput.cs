using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    //public event EventHandler OnInteractAction;
    //public event EventHandler OnAttackAction;


    public static Action attackInputPressed;
    public static Action attackInputReleased;
    public static Action reloadInput;
    public static Action shopInput;

    public static Action jumpInput;
    public static Action sprintInput;
    public static Action sprintInputCanceled;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Reload.performed += Reload_performed;
        playerInputActions.Player.Attack.canceled += Attack_canceled;

        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Sprint.performed += Sprint_performed;
        playerInputActions.Player.Sprint.canceled += Sprint_canceled;
    }

    /*private void OnEnable() {
        playerInputActions.Player.Enable();

        playerInputActions.Player.Attack.performed += Attack_performed;
        playerInputActions.Player.Reload.performed += Reload_performed;
        playerInputActions.Player.Attack.canceled += Attack_canceled;

        playerInputActions.Player.Jump.performed += Jump_performed;
        playerInputActions.Player.Sprint.performed += Sprint_performed;
        playerInputActions.Player.Sprint.canceled += Sprint_canceled;
    }*/

    private void OnDisable() {
        

        playerInputActions.Player.Attack.performed -= Attack_performed;
        playerInputActions.Player.Reload.performed -= Reload_performed;
        playerInputActions.Player.Attack.canceled  -= Attack_canceled;

        playerInputActions.Player.Jump.performed -= Jump_performed;
        playerInputActions.Player.Sprint.performed -= Sprint_performed;
        playerInputActions.Player.Sprint.canceled -= Sprint_canceled;
        playerInputActions.Player.Disable();
    }

    private void OnDestroy() {
        playerInputActions.Player.Attack.performed -= Attack_performed;
        playerInputActions.Player.Reload.performed -= Reload_performed;
        playerInputActions.Player.Attack.canceled -= Attack_canceled;

        playerInputActions.Player.Jump.performed -= Jump_performed;
        playerInputActions.Player.Sprint.performed -= Sprint_performed;
        playerInputActions.Player.Sprint.canceled -= Sprint_canceled;
        playerInputActions.Player.Disable();
    }

    private void Sprint_canceled(InputAction.CallbackContext obj) {
        sprintInputCanceled?.Invoke();
    }

    private void Sprint_performed(InputAction.CallbackContext obj) {
        sprintInput?.Invoke();
    }

    private void Jump_performed(InputAction.CallbackContext obj) {
        jumpInput?.Invoke();
    }

    private void Attack_canceled(InputAction.CallbackContext obj) {
        attackInputReleased?.Invoke();
    }

    private void Reload_performed(InputAction.CallbackContext obj) {
        reloadInput?.Invoke();
    }

    private void Attack_performed(InputAction.CallbackContext obj) {
        attackInputPressed?.Invoke();
    }


    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}

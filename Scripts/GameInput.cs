using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions _playerInputActions;

    public event EventHandler OnPlayerAttack;
    public event EventHandler OnPlayerShift;
    public event EventHandler OnPlayerShiftReleased;
    //public event EventHandler OnPlayerDash;
    //=================================================================================================================
    private void Awake()
    {
        Instance = this;
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

        _playerInputActions.Combat.Attack.started += PlayerAttack_started;
        _playerInputActions.Player.Shift.started += Shift_started;
        _playerInputActions.Player.Shift.canceled += Shift_canceled;
        //_playerInputActions.Player.Dash.performed += Dash_performed;
    }
    //=================================================================================================================
    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
    public void DisableMovement()
    {
        _playerInputActions.Disable();
    }
    //=================================================================================================================
    private void Shift_canceled(InputAction.CallbackContext obj)
    {
        OnPlayerShiftReleased?.Invoke(this, EventArgs.Empty);
    }
    private void Shift_started(InputAction.CallbackContext obj)
    {
        OnPlayerShift?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        OnPlayerAttack?.Invoke(this, EventArgs.Empty);
    }
    //private void Dash_performed(InputAction.CallbackContext context)
    //{
    //    OnPlayerDash?.Invoke(this, EventArgs.Empty);
    //}
}


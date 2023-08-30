using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private InputManager _inputManager;

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager?.Disable();
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        var direction = context.ReadValue<Vector2>().normalized;
        Run(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        Stop();
    }

    #endregion
}

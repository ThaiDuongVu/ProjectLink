using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        var direction = context.ReadValue<Vector2>().normalized;
        Run(direction);
        SetLookDirection(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);

        Stop();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Animator.SetBool(RunAnimationBool, CurrentSpeed > 0f);
    }

    #endregion
}

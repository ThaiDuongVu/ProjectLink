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

        var direction = context.ReadValue<Vector2>();
        Run(direction);
        SetLookDirection(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);

        Stop();
    }

    #endregion

    #region Movement Methods

    public override void Run(Vector2 direction)
    {
        base.Run(direction);

        Animator.SetBool(RunAnimationBool, true);
    }

    public override void Stop()
    {
        base.Stop();

        Animator.SetBool(RunAnimationBool, false);
    }

    #endregion
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : CharacterMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    private Player _player;

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

    protected override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
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

    private void JumpOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;
    }

    #endregion

    public override void Run(Vector2 direction)
    {
        base.Run(direction);

        if (direction.x == 0f) return;
        _player.SetFlip(direction.x < 0f);

        Animator.SetBool(RunAnimationBool, true);
    }

    public override void Stop()
    {
        base.Stop();

        Animator.SetBool(RunAnimationBool, false);
    }
}

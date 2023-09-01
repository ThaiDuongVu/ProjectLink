using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : CharacterMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    [Header("Jump References")]
    [SerializeField] private Transform jumpPoint;
    [SerializeField] private ParticleSystem jumpMuzzlePrefab;
    [SerializeField] private SpriteRenderer jumpBar;
    [SerializeField] private Color jumpEnabledColor;
    [SerializeField] private Color jumpDisabledColor;

    private static readonly int JumpAnimationTrigger = Animator.StringToHash("jump");

    private Player _player;

    private InputManager _inputManager;

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        // Handle player jump input
        _inputManager.Player.Jump.performed += JumpOnPerformed;

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

    protected override void Update()
    {
        base.Update();

        HandleJumpBar();
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

        Jump();
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

    public override bool Jump()
    {
        if (!base.Jump()) return false;

        Animator.SetTrigger(JumpAnimationTrigger);
        Instantiate(jumpMuzzlePrefab, jumpPoint.position, Quaternion.identity);

        CameraShaker.Instance.Shake(CameraShakeMode.Micro);

        return true;
    }

    private void HandleJumpBar()
    {
        jumpBar.transform.localScale = JumpTimer == null 
                                        ? Vector2.one
                                        : new Vector2(JumpTimer.Progress / JumpTimer.MaxProgress, 1f);
        jumpBar.color = CanJump ? jumpEnabledColor : jumpDisabledColor;
    }
}

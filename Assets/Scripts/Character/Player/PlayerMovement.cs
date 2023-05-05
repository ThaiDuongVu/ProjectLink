using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : CharacterMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    [Header("Dash Stats")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashInterpolationRatio;
    [SerializeField] private float dashStoppingDistance;
    [SerializeField] private int dashRate;

    [Header("Dash References")]
    [SerializeField] private ParticleSystem dashMuzzlePrefab;
    [SerializeField] private Transform dashTrail;

    private bool _isDashing;
    private Vector2 _dashPosition;
    private bool _canDash = true;
    private Timer _dashTimer;
    private static readonly int DashAnimationBool = Animator.StringToHash("isDashing");

    private Player _player;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        // Handle player dash input
        _inputManager.Player.Dash.performed += DashOnPerformed;
        _inputManager.Player.Dash.canceled += DashOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    protected override void Awake()
    {
        base.Awake();

        _player = GetComponent<Player>();
    }

    protected override void Start()
    {
        base.Start();

        dashTrail.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (_isDashing)
        {
            transform.position = Vector2.Lerp(transform.position, _dashPosition, dashInterpolationRatio);
            if (Vector2.Distance(transform.position, _dashPosition) <= dashStoppingDistance) EndDash();
        }

        if (!_canDash && _dashTimer.IsReached()) _canDash = true;
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;
        if (TutorialController.Instance.IsTutorialDisplayed) return;
        if (_player.MovementDisabled) return;

        var direction = context.ReadValue<Vector2>().normalized * (PlayerPrefs.GetInt("InvertMovement") == 0 ? 1f : -1f);
        Run(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        Stop();
    }

    private void DashOnPerformed(InputAction.CallbackContext context)
    {
        if (GameController.Instance.State != GameState.InProgress) return;
        if (TutorialController.Instance.IsTutorialDisplayed) return;
        InputTypeController.Instance.CheckInputType(context);
        if (_player.MovementDisabled) return;

        Dash(CurrentDirection);
    }

    private void DashOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
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

    public override void StopImmediate()
    {
        base.StopImmediate();
        Animator.SetBool(RunAnimationBool, false);
    }

    #endregion

    #region Dash Methods

    private void Dash(Vector2 direction)
    {
        if (!_canDash) return;

        var hitPoint = Physics2D.Raycast(transform.position, direction, dashDistance, LayerMask.GetMask("Obstacles")).point;

        // Set dash position based on hit obstacle
        if (hitPoint == Vector2.zero) _dashPosition = (Vector2)transform.position + direction * dashDistance;
        else _dashPosition = hitPoint - direction * 0.5f;

        // Update dash state
        _isDashing = true;
        _dashTimer = new Timer(1f / dashRate);
        _canDash = false;

        // Update dash behavior
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        Animator.SetBool(DashAnimationBool, true);
        Collider.enabled = false;

        // Play dash effects
        dashTrail.gameObject.SetActive(true);
        // Instantiate(dashMuzzlePrefab, transform.position, Quaternion.identity).transform.up = -direction;
        CameraShaker.Instance.Shake(CameraShakeMode.Micro);
    }

    private void EndDash()
    {
        // Update dash state
        _dashPosition = transform.position;
        _isDashing = false;

        // Update dash behavior
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Animator.SetBool(DashAnimationBool, false);
        Collider.enabled = true;

        // Stop dash effects
        dashTrail.gameObject.SetActive(false);
    }

    #endregion
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    [Header("Virtual Cursor")]
    [SerializeField] private VirtualCursor virtualCursorPrefab;
    private VirtualCursor _virtualCursor;

    [Header("Weapon")]
    [SerializeField] private Transform weaponHolder;
    public Weapon CurrentWeapon { get; private set; }

    private bool _isExecuting;
    private Zombie _executionTarget;
    private Vector2 _executionPosition;

    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerCombo _playerCombo;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player fire input
        _inputManager.Player.Fire.performed += FireOnPerformed;
        _inputManager.Player.Fire.canceled += FireOnCanceled;

        // Handle player execute input
        _inputManager.Player.Execute.performed += ExecuteOnPerformed;
        _inputManager.Player.Execute.canceled += ExecuteOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    protected override void Awake()
    {
        base.Awake();

        _virtualCursor = Instantiate(virtualCursorPrefab, Vector2.zero, Quaternion.identity);

        _player = GetComponent<Player>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerCombo = GetComponent<PlayerCombo>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        _virtualCursor.SetColor(CurrentWeapon ? CurrentWeapon.CrosshairColor : Color.white);
        _virtualCursor.SetAimAssist(CurrentWeapon?.CurrentDamageTarget != null);

        if (_executionTarget) transform.position = Vector2.Lerp(transform.position, _executionPosition, 0.5f);
        else _playerMovement.SetLookDirection((_virtualCursor.transform.position - transform.position).normalized);
    }

    #endregion

    #region Input Handlers

    private void FireOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance?.State != GameState.InProgress) return;
        if (TutorialController.Instance.IsTutorialDisplayed) return;
        if (_player.CombatDisabled) return;

        CurrentWeapon?.Fire();
    }

    private void FireOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (_player.CombatDisabled) return;

        CurrentWeapon?.CancelFire();
    }

    private void ExecuteOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance?.State != GameState.InProgress) return;
        if (TutorialController.Instance.IsTutorialDisplayed) return;
        if (_player.CombatDisabled) return;

        Execute(CurrentWeapon.CurrentZombieTarget);
    }

    private void ExecuteOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
    }

    #endregion

    #region Weapon Methods

    public void EquipWeapon(Weapon weapon)
    {
        CurrentWeapon = weapon;
        CurrentWeapon.transform.SetParent(weaponHolder);
        CurrentWeapon.transform.localPosition = Vector2.zero;
    }

    public void UnequipCurrentWeapon()
    {
        CurrentWeapon.transform.SetParent(null);
        CurrentWeapon = null;
    }

    #endregion

    #region Execution Methods

    private void Execute(Zombie target)
    {
        if (!target) return;
        if (!target.IsStagger) return;

        // Set up execution target & position
        _executionTarget = target;
        var direction = (target.transform.position - transform.position).normalized;
        _executionPosition = target.transform.position - direction;

        // Disable movement & combat & damage
        _player.MovementDisabled = _player.CombatDisabled = _player.IsInvulnerable = true;

        // Pick an execution move to perform
        var allExecutionMoves = Resources.LoadAll<ExecutionMove>("Player/ExecutionMoves");
        var executionMove = allExecutionMoves[Random.Range(0, allExecutionMoves.Length)];

        // Perform said move
        Animator.SetTrigger(executionMove.animationTrigger);
        _playerCombo.Pause();

        // Update effects to focus on player
        PostProcessingController.Instance.SetVignetteIntensity(0.8f);
        PostProcessingController.Instance.SetVignetteCenter(
            Camera.main.WorldToViewportPoint((transform.position + target.transform.position) / 2f));

        // Finish the execution on animation hit frame
        Invoke(nameof(EndExecution), executionMove.hitFrame / (1f / Time.deltaTime));
    }

    private void EndExecution()
    {
        // Enable movement & combat & damage
        _player.MovementDisabled = _player.CombatDisabled = _player.IsInvulnerable = false;
        _playerCombo.Add();
        _playerCombo.Unpause();

        // Play some effects
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
        EffectsController.Instance.SpawnPopText(_executionTarget.transform.position, CurrentWeapon.hitColor, "Executed");

        // Destroy & reset target
        _executionTarget.Die();
        _executionTarget = null;

        // Reset player focus effects
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity);
        PostProcessingController.Instance.SetVignetteCenter(Camera.main.WorldToViewportPoint(Vector2.zero));
    }

    #endregion
}

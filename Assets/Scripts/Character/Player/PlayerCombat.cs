using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    [Header("Dash Stats")]
    [SerializeField] private float dashRange;
    [SerializeField] private int dashRate;
    [SerializeField] private float dashInterpolationRatio;
    [SerializeField] private float dashEpsilon;

    [Header("Dash References")]
    [SerializeField] private ParticleSystem dashMuzzlePrefab;
    [SerializeField] private DashLine dashLinePrefab;

    [Header("Combat Stats")]
    [SerializeField] private float damagePerDash;

    [Header("Arrow & Color References")]
    [SerializeField] private Transform arrow;
    [SerializeField] private Color regularColor;
    [SerializeField] private Color aimColor;

    private SpriteRenderer _arrowSprite;

    private bool _isDashing;
    private Vector2 _dashPosition;
    private static readonly int DashAnimationBool = Animator.StringToHash("isDashing");
    private bool _canDash = true;
    private Timer _dashTimer;

    private Vector2 _aimHitPoint;
    private bool _aimHitMap;
    private IDamageable _aimTarget;

    private Player _player;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

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

        _arrowSprite = arrow.GetComponentInChildren<SpriteRenderer>();
        _player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();

        Aim();

        if (_isDashing)
        {
            transform.position = Vector2.Lerp(transform.position, _dashPosition, dashInterpolationRatio);
            if (Vector2.Distance(transform.position, _dashPosition) <= dashEpsilon) EndDash();
        }

        if (!_canDash && _dashTimer.IsReached()) _canDash = true;
        arrow.localScale = _canDash ? Vector2.one : Vector2.one * _dashTimer.Progress / _dashTimer.MaxProgress;
    }

    #endregion

    #region Input Handlers

    private void DashOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        Dash();
    }

    private void DashOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
    }

    #endregion

    private void Aim()
    {
        // Set default color & hit point
        _aimHitMap = false;
        _aimTarget = null;
        _arrowSprite.color = regularColor;

        var hit = Physics2D.Raycast(transform.position, transform.up, dashRange);
        if (!hit) return;

        // Set color & target & hit point
        _aimHitMap = hit.transform.CompareTag("Map");
        _aimTarget = hit.transform.GetComponent<IDamageable>();
        _aimHitPoint = hit.point;
        if (_aimTarget != null) _arrowSprite.color = aimColor;
    }

    #region Dash Methods

    private void Dash()
    {
        if (!_canDash) return;

        // Update dash position & state
        _dashPosition = _aimHitMap
                        ? _aimHitPoint - (Vector2)transform.up * 0.5f
                        : transform.position + transform.up * dashRange;
        _isDashing = true;

        // Update animation & physics
        Animator.SetBool(DashAnimationBool, true);
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        Collider.enabled = false;
        StartCoroutine(_player.StartInvulnerability());

        // Deal damage to target (if applicable)
        if (_aimTarget != null)
        {
            var damageMessage = _aimTarget.CurrentHealth < damagePerDash ? _aimTarget.CurrentHealth.ToString() : damagePerDash.ToString();
            _aimTarget.TakeDamage(damagePerDash, transform.up, _aimHitPoint);
            EffectsController.Instance.SpawnPopText(_aimHitPoint, aimColor, damageMessage);
        }

        // Play dash effects
        Instantiate(dashMuzzlePrefab, transform.position, Quaternion.identity).transform.up = -transform.up;
        var dashLine = Instantiate(dashLinePrefab, _dashPosition, Quaternion.identity);
        dashLine.SetDirection(transform.up);
        dashLine.SetLength((_dashPosition - (Vector2)transform.position).magnitude);
        dashLine.SetColor(_aimTarget == null ? regularColor : aimColor);
        Destroy(dashLine.gameObject, 0.5f);

        // Lightly shake the camera
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        // Handle shake rate
        _canDash = false;
        _dashTimer = new Timer(1f / dashRate);
    }

    private void EndDash()
    {
        // Update dash state
        _isDashing = false;

        // Update animation & physics
        Animator.SetBool(DashAnimationBool, false);
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Collider.enabled = true;
        StartCoroutine(_player.EndInvulnerability(0.2f));
    }

    #endregion
}

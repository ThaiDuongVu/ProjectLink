using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    [Header("Dash Stats")]
    [SerializeField] private float dashRange;
    [SerializeField] private float dashInterpolationRatio;
    [SerializeField] private float dashEpsilon;
    [SerializeField] private int dashRate;

    [Header("Dash References")]
    [SerializeField] private ParticleSystem dashMuzzlePrefab;
    [SerializeField] private GameObject dashLinePrefab;

    private bool _isDashing;
    private Vector2 _dashPosition;
    private static readonly int DashAnimationBool = Animator.StringToHash("isDashing");
    private bool _canDash = true;
    private Timer _dashTimer;

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

    protected override void Update()
    {
        base.Update();

        if (_isDashing)
        {
            transform.position = Vector2.Lerp(transform.position, _dashPosition, dashInterpolationRatio);
            if (Vector2.Distance(transform.position, _dashPosition) <= dashEpsilon) EndDash();
        }

        if (!_canDash && _dashTimer.IsReached()) _canDash = true;
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

    private void Dash()
    {
        if (!_canDash) return;

        _dashPosition = transform.position + transform.up * dashRange;
        _isDashing = true;

        Animator.SetBool(DashAnimationBool, true);
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        Instantiate(dashMuzzlePrefab, transform.position, Quaternion.identity).transform.up = -transform.up;
        var dashLine = Instantiate(dashLinePrefab, transform.position, Quaternion.identity);
        dashLine.transform.right = transform.up;
        Destroy(dashLine, 1f);

        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        _canDash = false;
        _dashTimer = new Timer(1f / dashRate);
    }

    private void EndDash()
    {
        _isDashing = false;

        Animator.SetBool(DashAnimationBool, false);
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}

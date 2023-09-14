using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    [Header("References")]
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem muzzle;

    private const float ArrowInterpolatoinRatio = 0.6f;

    private Player _player;
    private PlayerMovement _playerMovement;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player fire input
        _inputManager.Player.Fire.performed += FireOnPerformed;

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
        _playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void Update()
    {
        base.Update();

        arrow.up = Vector2.Lerp(arrow.up, _playerMovement.CurrentDirection, ArrowInterpolatoinRatio);
    }

    #endregion

    #region Input Handlers

    private void FireOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        Fire();
    }

    #endregion

    private void Fire()
    {
        muzzle.Play();
    }
}

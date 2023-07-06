using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    [Header("Stats")]
    [SerializeField] private float range;
    [SerializeField] private float pushForce;

    [Header("References")]
    [SerializeField] private Transform crosshair;

    [Header("Color References")]
    [SerializeField] private Color crosshairRegularColor;
    [SerializeField] private Color crosshairTargetColor;

    private SpriteRenderer _crosshairSprite;

    private static readonly int PushAnimationTrigger = Animator.StringToHash("push");

    private Item _targetItem;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle hold/push input
        _inputManager.Player.Hold.performed += HoldOnPerformed;
        _inputManager.Player.Hold.canceled += HoldOnCanceled;

        _inputManager.Player.Push.performed += PushOnPerformed;
        _inputManager.Player.Push.canceled += PushOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    protected override void Awake()
    {
        base.Awake();

        _crosshairSprite = crosshair.GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        CheckTarget();
    }

    #endregion

    #region Input Handlers

    private void HoldOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;
    }

    private void HoldOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
    }

    private void PushOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        Push(_targetItem);
    }

    private void PushOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
    }

    #endregion

    private void CheckTarget()
    {
        // Raycast forward to acquire item target
        var hit = Physics2D.Raycast(transform.position, transform.up, range, LayerMask.GetMask("Items"));
        if (!hit)
        {
            // _crosshairSprite.color = crosshairRegularColor;
            crosshair.gameObject.SetActive(true);
            if (_targetItem)
            {
                _targetItem.SetHighlight(false);
                _targetItem = null;
            }
            return;
        }

        if (!_targetItem) _targetItem = hit.transform.GetComponent<Item>();

        _targetItem.SetHighlight(true, transform);
        // _crosshairSprite.color = crosshairTargetColor;
        crosshair.gameObject.SetActive(false);
    }

    #region Hold Methods

    private void Hold(Item item)
    {

    }

    private void ReleaseCurrentItem()
    {

    }

    #endregion

    #region Push Methods

    private void Push(Item item)
    {
        if (item)
        {
            item.AddForce(transform.up, pushForce);
        }

        Animator.SetTrigger(PushAnimationTrigger);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    #endregion
}

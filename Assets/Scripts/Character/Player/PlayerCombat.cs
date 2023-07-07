using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : CharacterCombat
{
    [Header("Stats")]
    [SerializeField] private float range;
    [SerializeField] private float pushForce;

    [Header("References")]
    [SerializeField] private Transform crosshair;
    [SerializeField] private ParticleSystem muzzle;

    private static readonly int PushAnimationTrigger = Animator.StringToHash("push");

    private Item _targetItem;
    private Item _heldItem;

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

    protected override void Update()
    {
        base.Update();

        if (!_heldItem) CheckTarget();
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
            muzzle.Play();
        }

        Animator.SetTrigger(PushAnimationTrigger);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    #endregion
}

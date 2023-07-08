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
    [SerializeField] private Transform itemHolder;

    [Header("Colors")]
    [SerializeField] private Color holdTextColor;
    [SerializeField] private Color pushTextColor;

    private static readonly int PushAnimationTrigger = Animator.StringToHash("push");
    private string[] _pushTexts = new string[] { "Bam", "Boom", "Tada" };

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

        Hold(_targetItem);
    }

    private void HoldOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);

        ReleaseHeldItem();
    }

    private void PushOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        Push(_heldItem ?? _targetItem);
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
            if (_targetItem)
            {
                _targetItem.SetHighlight(false);
                _targetItem.SetPusher();
                _targetItem = null;
            }
            crosshair.gameObject.SetActive(true);
            return;
        }

        if (!_targetItem) _targetItem = hit.transform.GetComponent<Item>();

        _targetItem.SetHighlight(true);
        _targetItem.SetPusher(transform);
        crosshair.gameObject.SetActive(false);
    }

    #region Hold Methods

    private void Hold(Item item)
    {
        if (!item) return;

        _heldItem = item;
        _heldItem.SetHolder(itemHolder);

        EffectsController.Instance.SpawnPopText(crosshair.position, holdTextColor, _heldItem.name);
    }

    private void ReleaseHeldItem()
    {
        if (!_heldItem) return;

        _heldItem.SetHolder();
        _heldItem = null;
    }

    #endregion

    #region Push Methods

    private void Push(Item item)
    {
        // Release currently held item first (if applicable)
        ReleaseHeldItem();

        // Add force (and effects) to item
        if (item)
        {
            item.AddForce(transform.up, pushForce);

            EffectsController.Instance.SpawnPopText(crosshair.position, pushTextColor, _pushTexts[Random.Range(0, _pushTexts.Length)]);
            muzzle.Play();
        }

        Animator.SetTrigger(PushAnimationTrigger);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    #endregion
}

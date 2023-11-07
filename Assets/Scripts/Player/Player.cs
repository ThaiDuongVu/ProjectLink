using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer chain;
    [SerializeField] private Transform pin;

    private Block[] _blocks;
    private int _activeBlockIndex = 1;
    private int _inactiveBlockIndex => _activeBlockIndex == 0 ? 1 : 0;
    public Block ActiveBlock => _blocks[_activeBlockIndex];
    public Block InactiveBlock => _blocks[_inactiveBlockIndex];

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle movement input
        _inputManager.Player.Move.performed += MoveOnPerformed;
        _inputManager.Player.Move.canceled += MoveOnCanceled;

        // Handle fire input
        _inputManager.Player.Fire.performed += FireOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Awake()
    {
        _blocks = GetComponentsInChildren<Block>();
    }

    private void Start()
    {
        SetActiveBlock(0);

        _blocks[0].SetConnectedBody(_blocks[1].GetRigidbody);
        _blocks[1].SetConnectedBody(_blocks[0].GetRigidbody);
    }

    private void Update()
    {
        chain.SetPosition(0, _blocks[0].transform.position);
        chain.SetPosition(1, _blocks[1].transform.position);
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        var direction = context.ReadValue<Vector2>().normalized;
        ActiveBlock.Swing(direction);
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        ActiveBlock.StopSwing();
    }

    private void FireOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        SwapActiveBlock();
    }

    #endregion

    private void SetActiveBlock(int index)
    {
        _activeBlockIndex = index;
        _blocks[_activeBlockIndex].IsActive = true;
        _blocks[_inactiveBlockIndex].IsActive = false;

        pin.position = InactiveBlock.transform.position;
    }

    private void SwapActiveBlock()
    {
        SetActiveBlock(_activeBlockIndex == 0 ? 1 : 0);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LineRenderer chain;
    [SerializeField] private GameObject pin;
    [SerializeField] private GameObject arrow;

    private Block[] _blocks;
    private int _activeBlockIndex = 1;
    private int _inactiveBlockIndex => _activeBlockIndex == 0 ? 1 : 0;
    public Block ActiveBlock => _blocks[_activeBlockIndex];
    public Block InactiveBlock => _blocks[_inactiveBlockIndex];

    private Vector2 _arrowTargetDirection;

    public int CollectedStars { get; set; }

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

        // Handle undo input
        _inputManager.Player.Undo.performed += UndoOnPerformed;

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
        // Set chain positions
        chain.positionCount = 2;
        chain.SetPositions(new Vector3[] { _blocks[0].transform.position, _blocks[1].transform.position });

        SetArrowPosition(ActiveBlock.transform.position);
        SetArrowDirection(Vector2.Lerp(arrow.transform.up, _arrowTargetDirection, 0.5f));
    }

    #endregion

    #region Input Handlers

    private void MoveOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;
        var direction = context.ReadValue<Vector2>().normalized;

        ActiveBlock.Swing(direction);
        arrow.SetActive(true);
        _arrowTargetDirection = direction;
    }

    private void MoveOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        ActiveBlock.StopSwing();
        arrow.SetActive(false);
    }

    private void FireOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        SwapActiveBlock();
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    private void UndoOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);
        if (GameController.Instance.State != GameState.InProgress) return;

        Undo();
    }

    #endregion

    private void SetArrowPosition(Vector2 position)
    {
        arrow.transform.position = position;
    }

    private void SetArrowDirection(Vector2 direction)
    {
        arrow.transform.up = direction;
    }

    private void SetPinPosition(Vector2 position)
    {
        pin.SetActive(false);
        pin.transform.position = position;
        pin.SetActive(true);
    }

    private void SetActiveBlock(int index)
    {
        // Set index and block states
        _activeBlockIndex = index;
        _blocks[_activeBlockIndex].IsActive = true;
        _blocks[_inactiveBlockIndex].IsActive = false;

        SetPinPosition(InactiveBlock.transform.position);
    }

    public void SwapActiveBlock()
    {
        if (InactiveBlock.IsInPortal) return;
        SetActiveBlock(_activeBlockIndex == 0 ? 1 : 0);
    }

    public void CheckWinCondition()
    {
        // If both blocks entered portal then complete level
        if (_blocks[0].IsInPortal && _blocks[1].IsInPortal)
        {
            _blocks[0].Exit();
            _blocks[1].Exit();
            chain.gameObject.SetActive(false);
            StartCoroutine(GameController.Instance.CompleteLevel(CollectedStars));
        }
        // Else swap to the other block
        else
        {
            SwapActiveBlock();
            pin.SetActive(false);
        }
    }

    public void Undo()
    {
        if (!InactiveBlock.PopPosition() || !ActiveBlock.PopPosition()) return;

        SwapActiveBlock();

        EffectsController.Instance.SpawnPopText(InactiveBlock.transform.position, "Undo", Color.white);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualCursor : MonoBehaviour
{
    [Header("Cursor Stats")]
    public float sensitivity;

    [Header("Cursor References")]
    [SerializeField] private SpriteRenderer sprite;

    [Header("Aim Assist")]
    public float regularScale;
    public float assistScale;
    private float _currentAimScale;

    private Vector2 _direction;
    private Vector2 _minPosition;
    private Vector2 _maxPosition;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player aim input
        _inputManager.Player.Aim.performed += AimOnPerformed;
        _inputManager.Player.Aim.canceled += AimOnCanceled;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Start()
    {
        SetAimAssist(false);

        _minPosition = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f));
        _maxPosition = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f));
    }

    private void Update()
    {
        transform.Translate(_direction * sensitivity * _currentAimScale * Time.fixedDeltaTime);
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, _minPosition.x, _maxPosition.x),
            Mathf.Clamp(transform.position.y, _minPosition.y, _maxPosition.y)
        );
    }

    #endregion

    #region Input Handlers

    private void AimOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);
        if (GameController.Instance?.State != GameState.InProgress) return;
        if (TutorialController.Instance.IsTutorialDisplayed) return;

        _direction = Vector2.ClampMagnitude(context.ReadValue<Vector2>(), 1f);
    }

    private void AimOnCanceled(InputAction.CallbackContext context)
    {
        InputTypeController.Instance?.CheckInputType(context);

        _direction = Vector2.zero;
    }

    #endregion

    public void SetColor(Color color)
    {
        sprite.color = color;
    }

    public void SetAimAssist(bool aimAssist)
    {
        _currentAimScale = aimAssist ? assistScale : regularScale;
    }
}

using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    public bool RunDisabled { get; set; } = false;

    private bool _isRunning;
    private float _currentSpeed;
    public Vector2 CurrentDirection { get; protected set; } = Vector2.up;

    private bool _lookDirectionSet;
    public Vector2 LookDirection { get; protected set; } = Vector2.up;

    protected Rigidbody2D Rigidbody;
    protected Animator Animator;
    protected Collider2D Collider;

    private Character _character;

    #region Unity Events

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Collider = GetComponent<Collider2D>();

        _character = GetComponent<Character>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (_isRunning) Accelerate();
        else Decelerate();

        // rigidbody.MovePosition(rigidbody.position + CurrentDirection * _currentSpeed * Time.fixedDeltaTime);
        if (_currentSpeed > 0f) Rigidbody.velocity = CurrentDirection * _currentSpeed;
    }

    protected virtual void Update()
    {
        if (_lookDirectionSet && LookDirection != Vector2.zero) transform.up = Vector2.Lerp(transform.up, LookDirection, 0.2f);
        ScaleAnimationSpeed();
    }

    #endregion

    #region Movement Methods

    private void Accelerate()
    {
        if (_currentSpeed < speed) _currentSpeed += acceleration * Time.fixedDeltaTime;
        else if (_currentSpeed > speed) _currentSpeed = speed;
    }

    private void Decelerate()
    {
        if (_currentSpeed > 0f) _currentSpeed -= deceleration * Time.fixedDeltaTime;
        else if (_currentSpeed < 0f) _currentSpeed = 0f;
    }

    public virtual void Run(Vector2 direction)
    {
        if (RunDisabled) return;

        CurrentDirection = direction;
        _isRunning = true;
    }

    public virtual void Stop()
    {
        _isRunning = false;
    }

    public virtual void StopImmediate()
    {
        _isRunning = false;

        _currentSpeed = 0f;
        Rigidbody.velocity = Vector2.zero;
    }

    #endregion

    protected virtual void ScaleAnimationSpeed()
    {
        Animator.speed = _isRunning ? _currentSpeed / speed : 1f;
    }

    #region Look Direction Methods

    public virtual void SetLookDirection(Vector2 direction)
    {
        LookDirection = direction;
        _lookDirectionSet = true;
    }

    public virtual void UnsetLookDirection()
    {
        _lookDirectionSet = false;
        LookDirection = Vector2.up;
    }

    #endregion

    public IEnumerator TemporarilyDisableRun(float duration)
    {
        RunDisabled = true;
        yield return new WaitForSeconds(duration);
        RunDisabled = false;
    }
}

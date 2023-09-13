using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    protected bool IsRunning;
    protected float CurrentSpeed;
    public Vector2 CurrentDirection { get; protected set; } = Vector2.zero;

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
        if (IsRunning) Accelerate();
        else Decelerate();

        if (CurrentSpeed > 0f) Rigidbody.velocity = CurrentDirection * CurrentSpeed;
    }

    protected virtual void Update()
    {
        ScaleAnimationSpeed();
    }

    #endregion

    #region Movement Methods

    private void Accelerate()
    {
        if (CurrentSpeed < speed) CurrentSpeed += acceleration * Time.fixedDeltaTime;
        else if (CurrentSpeed > speed) CurrentSpeed = speed;
    }

    private void Decelerate()
    {
        if (CurrentSpeed > 0f) CurrentSpeed -= deceleration * Time.fixedDeltaTime;
        else if (CurrentSpeed < 0f) CurrentSpeed = 0f;
    }

    public virtual void Run(Vector2 direction)
    {
        if (direction.x == 0f) return;

        IsRunning = true;
        CurrentDirection = direction;
    }

    public virtual void Move(Vector2 direction)
    {
        IsRunning = true;
        CurrentDirection = direction;
    }

    public virtual void Stop()
    {
        IsRunning = false;
    }

    public virtual void StopImmediate()
    {
        IsRunning = false;
        CurrentSpeed = 0f;

        Rigidbody.velocity = Vector2.zero;
    }

    #endregion

    protected virtual void ScaleAnimationSpeed()
    {
        Animator.speed = IsRunning ? CurrentSpeed / speed : 1f;
    }
}

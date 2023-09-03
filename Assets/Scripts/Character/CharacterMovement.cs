using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [Header("Jump Stats")]
    [SerializeField] private float jumpForce;
    [SerializeField] private int jumpRate;
    [SerializeField] public int maxAirJumps;
    protected bool CanJump = true;
    protected Timer JumpTimer;
    public int AirJumpsLeft { get; set; }

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

        if (CurrentSpeed > 0f) Rigidbody.velocity = new Vector2(CurrentDirection.x * CurrentSpeed, Rigidbody.velocity.y);
    }

    protected virtual void Update()
    {
        ScaleAnimationSpeed();

        if (!CanJump && JumpTimer.IsReached()) CanJump = true;

        if (_character.IsGrounded && AirJumpsLeft < maxAirJumps) AirJumpsLeft = maxAirJumps;
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
        // Convert direction to either left or right
        CurrentDirection = direction.x > 0f ? Vector2.right : Vector2.left;
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

    public virtual bool Jump()
    {
        if (!CanJump) return false;
        if (AirJumpsLeft <= 0) return false;

        Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, 0f);
        Rigidbody?.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        CanJump = false;
        JumpTimer = new Timer(1f / jumpRate);
        AirJumpsLeft--;

        return true;
    }
}

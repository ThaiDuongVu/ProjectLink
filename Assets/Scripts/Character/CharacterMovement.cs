using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float moveForce;

    protected bool IsRunning;
    protected float CurrentSpeed;
    public Vector2 CurrentDirection { get; protected set; } = Vector2.up;

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
        if (IsRunning) Rigidbody.AddForce(CurrentDirection * moveForce, ForceMode2D.Force);
    }

    protected virtual void Update()
    {

    }

    #endregion

    #region Movement Methods

    public virtual void Run(Vector2 direction)
    {
        IsRunning = true;
        CurrentDirection = direction;

        if (direction.x < 0f) _character.SetFlip(true);
        else if (direction.x > 0f) _character.SetFlip(false);
    }

    public virtual void Stop()
    {
        IsRunning = false;
    }

    public virtual void StopImmediate()
    {
        Stop();

        CurrentSpeed = 0f;
        Rigidbody.velocity = Vector2.zero;
    }

    #endregion
}

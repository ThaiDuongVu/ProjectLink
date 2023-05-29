using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    public float speed;

    [Header("References")]
    [SerializeField] private Transform sprite;

    private Vector2 _currentDirection;
    public virtual Vector2 CurrentDirection
    {
        get => _currentDirection;
        set
        {
            _currentDirection = value;
            sprite.up = value;
        }
    }

    private Rigidbody2D _rigidbody;

    #region Unity Events

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();
    }

    #endregion

    protected virtual void HandleMovement()
    {
        if (CurrentDirection != Vector2.zero) _rigidbody.velocity = CurrentDirection * speed;
    }
}

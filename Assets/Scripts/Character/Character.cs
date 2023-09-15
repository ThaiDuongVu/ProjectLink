using UnityEngine;

public class Character : MonoBehaviour, IDamageable
{
    [Header("Health Stats")]
    public float baseHealth;
    private float _health;
    public virtual float Health
    {
        get => _health;
        set
        {
            _health = value;
            if (value <= 0f) Die();
        }
    }

    [Header("References")]
    [SerializeField] private SpriteRenderer mainSprite;
    [SerializeField] private ParticleSystem bloodSplashPrefab;
    [SerializeField] private ParticleSystem deathExplosionPrefab;

    [Header("Boundaries Stats")]
    [SerializeField] private bool isBounded;
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody;
    protected Collider2D Collider;

    #region Unity Events

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        Health = baseHealth;
    }

    protected virtual void Update()
    {
        HandleBoundaries();
    }

    #endregion

    #region Interface Implementations

    public virtual float BaseHealth => baseHealth;
    public virtual float CurrentHealth => Health;

    public virtual void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint)
    {
        Health -= damage;
        Instantiate(bloodSplashPrefab, contactPoint, Quaternion.identity).transform.up = direction;
    }

    public virtual void Die()
    {
        Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    #endregion

    public virtual void SetFlip(bool isFlipped)
    {
        mainSprite.flipX = isFlipped;
    }

    private void HandleBoundaries()
    {
        if (!isBounded) return;
        var position = transform.position;

        if (position.x < minPosition.x) transform.position = new Vector2(maxPosition.x, position.y);
        else if (position.x > maxPosition.x) transform.position = new Vector2(minPosition.x, position.y);

        if (position.y < minPosition.y) transform.position = new Vector2(position.x, maxPosition.y);
        else if (position.y > maxPosition.y) transform.position = new Vector2(position.x, minPosition.y);
    }
}

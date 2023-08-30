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

    public bool IsFalling => Rigidbody?.velocity.y < -0.2f;

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
        Destroy(gameObject);
        Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
    }

    #endregion

    public virtual void SetFlip(bool isFlipped)
    {
        mainSprite.flipX = isFlipped;
    }
}

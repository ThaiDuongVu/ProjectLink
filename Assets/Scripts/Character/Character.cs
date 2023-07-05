using UnityEngine;

public class Character : MonoBehaviour, IDamageable, IKnockbackable
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
    [SerializeField] private Transform rig;
    [SerializeField] private ParticleSystem bloodSplashPrefab;
    [SerializeField] private ParticleSystem deathExplosionPrefab;

    protected Animator Animator;
    protected Rigidbody2D Rigidbody;

    private CharacterMovement _characterMovement;

    #region Unity Events

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();

        _characterMovement = GetComponent<CharacterMovement>();
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

    public virtual void Knockback(Vector2 direction, float force)
    {
        _characterMovement?.StopImmediate();
        Rigidbody?.AddForce(direction * force, ForceMode2D.Impulse);
    }

    #endregion
}

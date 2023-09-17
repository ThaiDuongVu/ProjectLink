using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float baseKnockbackForce;

    [Header("References")]
    [SerializeField] private Color damageColor;
    [SerializeField] private ParticleSystem explosionPrefab;

    private Rigidbody2D _rigidbody;

    #region Unity Events

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    #endregion

    public void Fly(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        _rigidbody.AddTorque(force);
    }

    private void Explode()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Do not damage or knockback player or cat aka no friendly fire
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Cat"))
        {
            Explode();
            return;
        }

        var contactPoint = other.GetContact(0).point;
        var direction = _rigidbody.velocity.normalized;

        // Deal damage
        if (other.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(baseDamage, direction, contactPoint, damageColor);
        }

        // Deal knockback
        if (other.transform.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            knockbackable.Knockback(direction, baseKnockbackForce);
        }

        Explode();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border")) Destroy(gameObject);
    }
}

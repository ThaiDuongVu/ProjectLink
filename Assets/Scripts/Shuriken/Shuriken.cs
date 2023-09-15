using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float baseDamage;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        if (other.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            var contactPoint = other.GetContact(0).point;
            var direction = _rigidbody.velocity.normalized;

            damageable.TakeDamage(baseDamage, direction, contactPoint);
            EffectsController.Instance.SpawnPopText(contactPoint, baseDamage.ToString(), damageColor);
        }

        Destroy(gameObject);
    }
}

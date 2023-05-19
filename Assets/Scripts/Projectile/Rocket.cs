using UnityEngine;

public class Rocket : Projectile
{
    [Header("Fireball Stats")]
    public float damage;
    public float damageRadius;
    public float knockbackForce;

    [Header("Fireball References")]
    [SerializeField] private ParticleSystem explosion;

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // var damageable = other.transform.GetComponent<IDamageable>();
        // var knockbackable = other.transform.GetComponent<IKnockbackable>();

        // damageable?.TakeDamage(damage, CurrentDirection, other.contacts[0].point);
        // knockbackable?.Knockback(CurrentDirection, knockbackForce);

        Explode();
    }
}

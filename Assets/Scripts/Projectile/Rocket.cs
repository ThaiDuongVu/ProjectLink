using UnityEngine;

public class Rocket : Projectile
{
    [Header("Stats")]
    public float radius;
    public float maxDamage;
    public float maxForce;

    [Header("Rocket References")]
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Color damageColor;

    private void Explode()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit == GetComponent<Collider2D>()) continue;

            // Get damageable and knockbackable from overlap hit
            var hitDamageable = hit.transform.GetComponentInParent<IDamageable>();
            var hitKnockbackable = hit.transform.GetComponentInParent<IKnockbackable>();

            // Get hit position, distance & direction
            var hitPosition = hit.ClosestPoint(transform.position);
            var distance = (hitPosition - (Vector2)transform.position).magnitude;
            var direction = (hitPosition - (Vector2)transform.position).normalized;

            // Deal damage based on distance
            if (hitDamageable != null)
            {
                var damage = maxDamage - (distance / radius) * maxDamage;
                hitDamageable.TakeDamage(damage, direction, hitPosition);
                EffectsController.Instance.SpawnPopText(hitPosition, damageColor, ((int)damage).ToString());
            }

            // Deal knockback based on distance
            if (hitKnockbackable != null)
            {
                var force = maxForce - (distance / radius) * maxForce;
                hitKnockbackable.Knockback(direction, force);
            }
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        GameController.Instance.PlaySlowMotionEffect();
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

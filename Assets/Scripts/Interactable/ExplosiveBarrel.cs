using UnityEngine;

public class ExplosiveBarrel : Interactable, IDamageable
{
    [Header("Stats")]
    public float radius;
    public float maxDamage;
    public float maxForce;
    public float explosionDelay;

    [Header("References")]
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Color damageColor;

    private static readonly int ExplodeAnimationBool = Animator.StringToHash("isExploding");

    #region Interface Implementations

    public float BaseHealth { get; private set; } = 100f;
    public float CurrentHealth { get; private set; } = 100f;

    public void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint)
    {
        Animator.SetBool(ExplodeAnimationBool, true);
        Invoke(nameof(Die), explosionDelay);
    }

    public void Die()
    {
        Explode();
    }

    #endregion

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
}

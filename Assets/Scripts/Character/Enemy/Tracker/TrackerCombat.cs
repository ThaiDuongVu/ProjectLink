using UnityEngine;

public class TrackerCombat : EnemyCombat
{
    [Header("Tracker Stats")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float baseKnockbackForce;

    [Header("References")]
    [SerializeField] private Color damageColor;

    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");

    private Tracker _tracker;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _tracker = GetComponent<Tracker>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Do not damage or knockback player or other zombies
        if (other.transform.CompareTag("Player") || other.transform.CompareTag("Enemy")) return;

        var contactPoint = other.GetContact(0).point;
        var direction = (other.transform.position - transform.position).normalized;

        // Deal damage
        if (other.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(baseDamage, direction, contactPoint, damageColor);
            _tracker.TempStop();
        }

        // Deal knockback
        if (other.transform.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            knockbackable.Knockback(direction, baseKnockbackForce);
            Animator.SetTrigger(AttackAnimationTrigger);
        }
    }
}

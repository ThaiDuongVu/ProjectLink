using UnityEngine;

public class TrackerCombat : EnemyCombat
{
    [Header("Tracker Stats")]
    [SerializeField] private float baseDamage;
    [SerializeField] private float baseKnockbackForce;

    [Header("References")]
    [SerializeField] private Color damageColor;

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
        var contactPoint = other.GetContact(0).point;
        var direction = (other.transform.position - transform.position).normalized;

        if (other.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(baseDamage, direction, contactPoint);
            EffectsController.Instance.SpawnPopText(contactPoint, baseDamage.ToString(), damageColor);
            _tracker.TempStop();
        }

        if (other.transform.TryGetComponent<IKnockbackable>(out var knockbackable))
        {
            knockbackable.Knockback(direction, baseKnockbackForce);
        }
    }
}

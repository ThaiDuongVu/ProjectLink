using UnityEngine;

public class TrackerCombat : EnemyCombat
{
    [Header("Tracker Stats")]
    [SerializeField] private float baseDamage;

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
        if (other.transform.TryGetComponent<IDamageable>(out var damageable))
        {
            var contactPoint = other.GetContact(0).point;
            var direction = Rigidbody.velocity.normalized;

            damageable.TakeDamage(baseDamage, direction, contactPoint);
            EffectsController.Instance.SpawnPopText(contactPoint, baseDamage.ToString(), damageColor);

            _tracker.TempStop();
        }
    }
}

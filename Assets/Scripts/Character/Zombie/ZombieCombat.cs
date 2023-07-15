using UnityEngine;

public class ZombieCombat : CharacterCombat
{
    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float knockForce;
    [SerializeField] private Color damageTextColor;

    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");

    private ZombieMovement _zombieMovement;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _zombieMovement = GetComponent<ZombieMovement>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var player = other.transform.GetComponent<Player>();
            var direction = Rigidbody.velocity.normalized;
            var contactPoint = other.GetContact(0).point;

            _zombieMovement.Stop();

            player.TakeDamage(damage, direction, contactPoint);
            player.Knockback(direction, knockForce);

            CameraShaker.Instance.Shake(CameraShakeMode.Light);
            EffectsController.Instance.SpawnPopText(contactPoint, damageTextColor, damage.ToString());
            Animator.SetTrigger(AttackAnimationTrigger);
        }
    }
}

using UnityEngine;

public class RookieCombat : ZombieCombat
{
    [Header("Rookie Combat Stats")]
    [SerializeField] private float damage;
    [SerializeField] private Color damageColor;

    [SerializeField] private float knockbackForce;

    private static readonly int PushAnimationTrigger = Animator.StringToHash("push");

    private Rookie _rookie;
    private RookieMovement _rookieMovement;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rookie = GetComponent<Rookie>();
        _rookieMovement = GetComponent<RookieMovement>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player") || _rookie.IsStagger) return;

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(damage, _rookieMovement.CurrentDirection, other.GetContact(0).point);
        player.Knockback(_rookieMovement.CurrentDirection, knockbackForce);

        _rookieMovement.Stop();
        Animator.SetTrigger(PushAnimationTrigger);

        EffectsController.Instance.SpawnPopText(other.GetContact(0).point, damageColor, damage.ToString());
    }
}

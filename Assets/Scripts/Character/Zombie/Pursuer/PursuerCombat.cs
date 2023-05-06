using UnityEngine;

public class PursuerCombat : ZombieCombat
{
    [Header("Pursuer Combat Stats")]
    [SerializeField] private float damage;
    [SerializeField] private Color damageColor;

    [SerializeField] private float knockbackForce;

    private static readonly int PushAnimationTrigger = Animator.StringToHash("push");

    private Pursuer _pursuer;
    private PursuerMovement _pursuerMovement;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _pursuer = GetComponent<Pursuer>();
        _pursuerMovement = GetComponent<PursuerMovement>();
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Player") || _pursuer.IsStagger) return;

        var player = other.transform.GetComponent<Player>();
        player.TakeDamage(damage, _pursuerMovement.CurrentDirection, other.GetContact(0).point);
        player.Knockback(_pursuerMovement.CurrentDirection, knockbackForce);

        _pursuerMovement.Stop();
        Animator.SetTrigger(PushAnimationTrigger);

        EffectsController.Instance.SpawnPopText(other.GetContact(0).point, damageColor, damage.ToString());
    }
}

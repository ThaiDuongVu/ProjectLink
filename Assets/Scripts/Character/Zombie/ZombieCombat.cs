using UnityEngine;

public class ZombieCombat : CharacterCombat
{
    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float knockForce;

    private static readonly int AttackAnimationTrigger = Animator.StringToHash("attack");

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            var player = other.transform.GetComponent<Player>();
            var direction = Rigidbody.velocity.normalized;

            player.TakeDamage(damage, direction, other.GetContact(0).point);
            player.Knockback(direction, knockForce);

            Animator.SetTrigger(AttackAnimationTrigger);
        }
    }
}

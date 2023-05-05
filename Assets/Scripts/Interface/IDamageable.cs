using UnityEngine;
public interface IDamageable
{
    float BaseHealth { get; }
    float CurrentHealth { get; }
    void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint);
    void Die();
}
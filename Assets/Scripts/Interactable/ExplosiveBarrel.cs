using UnityEngine;

public class ExplosiveBarrel : Interactable, IDamageable
{
    #region Interface Implementations

    public float BaseHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint)
    {

    }

    public void Die()
    {
        
    }

    #endregion
}

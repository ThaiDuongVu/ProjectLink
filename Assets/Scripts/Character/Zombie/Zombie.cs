using UnityEngine;

public class Zombie : Character
{
    [Header("Stagger Stats")]
    public float staggerHealthThreshold;
    public float staggerDuration;

    public bool IsStagger { get; protected set; }
    private static readonly int StaggerAnimationBool = Animator.StringToHash("isStagger");

    #region Interface Implementations

    public override void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint)
    {
        base.TakeDamage(damage, direction, contactPoint);

        if (CurrentHealth <= staggerHealthThreshold) EnableStagger();
    }

    public override void Die()
    {
        base.Die();

        SpawnAllCollectibles();
    }

    #endregion

    public void SpawnAllCollectibles()
    {
        foreach (var spawner in GetComponentsInChildren<CollectibleSpawner>()) spawner.Spawn();
    }

    public virtual void EnableStagger()
    {
        IsStagger = true;
        Animator.SetBool(StaggerAnimationBool, true);

        Invoke(nameof(DisableStagger), staggerDuration);
    }

    public virtual void DisableStagger()
    {
        IsStagger = false;
        Animator.SetBool(StaggerAnimationBool, false);
    }
}

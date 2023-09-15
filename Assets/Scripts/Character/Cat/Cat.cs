using UnityEngine;
using UnityEngine.UI;

public class Cat : Character
{
    [Header("UI References")]
    [SerializeField] private Image healthBar;
    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            healthBar.transform.localScale = new Vector2(value / baseHealth, 1f);
        }
    }

    #region Interface Implementations

    public override void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint)
    {
        base.TakeDamage(damage, direction, contactPoint);

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
    }

    public override void Die()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
        GameController.Instance.PlaySlowMotionEffect();

        base.Die();
    }

    #endregion
}

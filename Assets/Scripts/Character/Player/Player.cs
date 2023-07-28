using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Character
{
    public bool IsInvulnerable { get; set; }

    [Header("UI References")]
    [SerializeField] private Image healthBar;
    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            healthBar.fillAmount = value / baseHealth;
        }
    }

    [SerializeField] private TMP_Text coinText;
    private int _collectedCoins;
    public int CollectedCoins
    {
        get => _collectedCoins;
        set
        {
            _collectedCoins = value;
            coinText.SetText(value.ToString());
        }
    }

    public override void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint)
    {
        if (IsInvulnerable) return;

        base.TakeDamage(damage, direction, contactPoint);
    }

    public override void Die()
    {
        GameController.Instance.PlaySlowMotionEffect();
        GameController.Instance.StartCoroutine(GameController.Instance.GameOver("You died", 1f));

        base.Die();
    }

    #region Invulnerability Methods

    public IEnumerator StartInvulnerability(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        IsInvulnerable = true;
    }

    public IEnumerator EndInvulnerability(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        IsInvulnerable = false;
    }

    #endregion
}

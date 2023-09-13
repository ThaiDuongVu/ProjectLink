using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Character
{
    [Header("UI References")]
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

    #region Interface Implementations

    public override void TakeDamage(float damage, Vector2 direction, Vector2 contactPoint) { }

    public override void Die() { }

    #endregion

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            other.GetComponent<Collectible>().OnCollected(this);
        }
    }
}

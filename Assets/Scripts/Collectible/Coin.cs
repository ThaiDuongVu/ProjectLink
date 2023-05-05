using UnityEngine;

public class Coin : Collectible
{
    [Header("Stats")]
    [SerializeField] private int amount;

    [Header("References")]
    [SerializeField] private Color collectColor;
    [SerializeField] private ParticleSystem collectSplashPrefab;

    public override bool OnCollected(Player player)
    {
        if (!base.OnCollected(player)) return false;

        player.CollectedCoins += amount * player.ComboMultiplier;

        var position = transform.position;
        Instantiate(collectSplashPrefab, position, Quaternion.identity);
        EffectsController.Instance.SpawnPopText(position, collectColor, amount.ToString());
        return true;
    }
}

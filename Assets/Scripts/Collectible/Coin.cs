using UnityEngine;

public class Coin : Collectible
{
    [SerializeField] private int amount;
    [SerializeField] private Color collectColor;
    [SerializeField] private ParticleSystem splashPrefab;

    public override bool OnCollected(Player player)
    {
        if (!base.OnCollected(player)) return false;

        player.CollectedCoins += amount;
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        EffectsController.Instance.SpawnPopText(transform.position, amount.ToString(), collectColor);

        return true;
    }
}

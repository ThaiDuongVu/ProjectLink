using UnityEngine;

public class Heart : Collectible
{
    [Header("Stats")]
    [SerializeField] private int amount;

    [Header("References")]
    [SerializeField] private Color collectColor;
    [SerializeField] private ParticleSystem collectSplashPrefab;

    public override bool OnCollected(Player player)
    {
        if (player.Health > player.baseHealth - amount) return false;
        if (!base.OnCollected(player)) return false;

        player.Health += amount;

        var position = transform.position;
        Instantiate(collectSplashPrefab, position, Quaternion.identity);
        EffectsController.Instance.SpawnPopText(position, collectColor, amount.ToString());
        return true;
    }
}

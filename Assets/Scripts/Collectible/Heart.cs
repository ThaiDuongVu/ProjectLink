using UnityEngine;

public class Heart : Collectible
{
    [SerializeField] private float amount;
    [SerializeField] private Color collectColor;
    [SerializeField] private ParticleSystem splashPrefab;

    public override bool OnCollected(Player player)
    {
        if (player.Health > player.baseHealth - amount) return false;
        if (!base.OnCollected(player)) return false;

        player.Health += amount;
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        EffectsController.Instance.SpawnPopText(transform.position, amount.ToString(), collectColor);

        return true;
    }
}

using UnityEngine;

public class JumpToken : Collectible
{
    [SerializeField] private int amount;
    [SerializeField] private Color collectColor;
    [SerializeField] private ParticleSystem splashPrefab;

    public override bool OnCollected(Player player)
    {
        var playerMovement = player.GetComponent<PlayerMovement>();

        if (playerMovement.AirJumpsLeft >= playerMovement.maxAirJumps) return false;
        if (!base.OnCollected(player)) return false;

        playerMovement.AirJumpsLeft += amount;
        Instantiate(splashPrefab, transform.position, Quaternion.identity);
        EffectsController.Instance.SpawnPopText(transform.position, amount.ToString(), collectColor);

        return true;
    }
}

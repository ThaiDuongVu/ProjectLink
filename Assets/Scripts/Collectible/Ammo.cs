using UnityEngine;

public class Ammo : Collectible
{
    [Header("Stats")]
    [SerializeField] private int amount;

    [Header("References")]
    [SerializeField] private Color collectColor;
    [SerializeField] private ParticleSystem collectSplashPrefab;

    public override bool OnCollected(Player player)
    {
        var currentWeapon = player.PlayerCombat.CurrentWeapon;
        if (currentWeapon.CurrentAmmo > currentWeapon.maxAmmo - amount) return false;
        if (!base.OnCollected(player)) return false;

        currentWeapon.CurrentAmmo += amount;

        var position = transform.position;
        Instantiate(collectSplashPrefab, position, Quaternion.identity);
        EffectsController.Instance.SpawnPopText(position, collectColor, amount.ToString());
        return true;
    }
}

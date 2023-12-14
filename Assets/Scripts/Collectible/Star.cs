using UnityEngine;

public class Star : Collectible
{
    public override void OnCollected(Player player)
    {
        player.CollectedStars++;
        GameController.Instance.PlaySlowMotionEffect();

        base.OnCollected(player);
    }
}

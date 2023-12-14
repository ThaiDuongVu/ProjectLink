using UnityEngine;

public class Star : Collectible
{
    public override void OnCollected(Player player)
    {
        GameController.Instance.PlaySlowMotionEffect();
        base.OnCollected(player);
    }
}

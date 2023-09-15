using UnityEngine;

public class TrackerMovement : EnemyMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    public override void Run(Vector2 direction)
    {
        base.Run(direction);
        Animator.SetBool(RunAnimationBool, true);
    }

    public override void Stop()
    {
        base.Stop();
        Animator.SetBool(RunAnimationBool, false);
    }
}

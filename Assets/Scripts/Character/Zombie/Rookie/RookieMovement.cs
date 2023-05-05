using UnityEngine;

public class RookieMovement : ZombieMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    private Rookie _rookie;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rookie = GetComponent<Rookie>();
    }

    #endregion

    public override void Run(Vector2 direction)
    {
        base.Run(direction);

        SetLookDirection(direction);
        Animator.SetBool(RunAnimationBool, true);
    }

    public override void Stop()
    {
        base.Stop();

        Animator.SetBool(RunAnimationBool, false);
    }
}

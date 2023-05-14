using UnityEngine;

public class ThrowerMovement : ZombieMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    private Thrower _thrower;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _thrower = GetComponent<Thrower>();
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

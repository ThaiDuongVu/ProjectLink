using UnityEngine;

public class PursuerMovement : ZombieMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    private Pursuer _pursuer;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _pursuer = GetComponent<Pursuer>();
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

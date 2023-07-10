using UnityEngine;

public class ZombieMovement : CharacterMovement
{
    private static readonly int RunAnimationBool = Animator.StringToHash("isRunning");

    #region Unity Events

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #endregion

    #region Movement Methods

    public override void Run(Vector2 direction)
    {
        base.Run(direction);

        Animator.SetBool(RunAnimationBool, true);
        SetLookDirection(CurrentDirection);
    }

    public override void Stop()
    {
        base.Stop();

        Animator.SetBool(RunAnimationBool, false);
    }

    #endregion
}

using UnityEngine;

public class Player : Character
{
    private static readonly int FallAnimationBool = Animator.StringToHash("isFalling");

    #region Unity Event

    protected override void Update()
    {
        Animator.SetBool(FallAnimationBool, IsFalling);
    }

    #endregion
}

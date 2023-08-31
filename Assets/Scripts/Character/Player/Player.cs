using UnityEngine;

public class Player : Character
{
    private static readonly int FallAnimationBool = Animator.StringToHash("isFalling");

    [Header("Effects References")]
    [SerializeField] private ParticleSystem groundTrail;
    [SerializeField] private ParticleSystem airTrail;
    private bool _tempGrounded;

    #region Unity Event

    protected override void Start()
    {
        _tempGrounded = !IsGrounded;
    }

    protected override void Update()
    {
        base.Update();

        Animator.SetBool(FallAnimationBool, IsFalling);

        if (IsGrounded != _tempGrounded)
        {
            if (IsGrounded)
            {
                groundTrail.Play();
                airTrail.Stop();
            }
            else
            {
                groundTrail.Stop();
                airTrail.Play();
            }

            _tempGrounded = IsGrounded;
        }
    }

    #endregion
}

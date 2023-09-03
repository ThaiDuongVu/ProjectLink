using System.Diagnostics;
using UnityEngine;

public class Player : Character
{
    private static readonly int FallAnimationBool = Animator.StringToHash("isFalling");

    [Header("Position References")]
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;

    [Header("Effects References")]
    [SerializeField] private ParticleSystem groundTrail;
    [SerializeField] private ParticleSystem airTrail;
    private bool _tempGrounded;

    #region Unity Event

    protected override void Start()
    {
        base.Start();

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

        HandlePlayerBorder();
    }

    #endregion

    private void HandlePlayerBorder()
    {
        var position = transform.position;

        if (position.x < minPosition.x) transform.position = new Vector2(maxPosition.x, position.y);
        else if (position.x > maxPosition.x) transform.position = new Vector2(minPosition.x, position.y);

        if (position.y < minPosition.y) transform.position = new Vector2(position.x, maxPosition.y);
        else if (position.y > maxPosition.y) transform.position = new Vector2(position.x, minPosition.y);
    }
}

using UnityEngine;

public class Grenade : Projectile
{
    [Header("Stats")]
    public int maxBounces;

    public override Vector2 CurrentDirection
    {
        get => base.CurrentDirection;
        set
        {
            // TODO: Set rigidbody force direction
        }
    }

    private int _currentBounce;

    private new Rigidbody2D rigidbody;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
    }

    #endregion

    protected override void HandleMovement()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }
}

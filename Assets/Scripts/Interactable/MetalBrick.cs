using UnityEngine;

public class MetalBrick : Interactable, IMetal, ISwitchActivator
{
    private Rigidbody2D _rigidbody;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    #endregion

    public void Attract(Transform target, float force)
    {
        _rigidbody.AddForce((target.position - transform.position).normalized * force);
    }
}

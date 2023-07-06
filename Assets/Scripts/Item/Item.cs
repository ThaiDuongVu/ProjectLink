using UnityEngine;

public class Item : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    #region Unity Events

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    #endregion

    public virtual void AddForce(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }
}

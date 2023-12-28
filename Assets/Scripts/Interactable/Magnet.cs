using System.Collections.Generic;
using UnityEngine;

public class Magnet : Interactable
{
    public float attractForce;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _fieldSprite;

    private List<IMetal> _targets = new();

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponents<BoxCollider2D>()[1];
        _fieldSprite = GetComponentsInChildren<SpriteRenderer>()[1];
    }

    protected override void Start()
    {
        base.Start();

        _boxCollider.size = new Vector2(_fieldSprite.size.x, _fieldSprite.size.y);
        _boxCollider.offset = new Vector2(0f, -0.5f * (_boxCollider.size.y - 1f));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        foreach (var target in _targets) target.Attract(transform, attractForce);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IMetal>(out var metal)) _targets.Add(metal);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var metal = other.GetComponent<IMetal>();
        if (metal != null && _targets.Contains(metal)) _targets.Remove(metal);
    }
}

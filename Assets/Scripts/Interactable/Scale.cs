using UnityEngine;

public class Scale : Interactable
{
    [SerializeField] private SpriteRenderer blockLeft;
    [SerializeField] private SpriteRenderer blockRight;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    private SpriteRenderer[] _sprites;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        _boxCollider.size = new Vector2(blockLeft.size.x + blockRight.size.x + 1f, 1f);
    }

    #endregion

    public void SetCollision(bool value)
    {
        _boxCollider.enabled = value;
        foreach (var sprite in _sprites) sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value ? 1f : 0.1f);
    }
}

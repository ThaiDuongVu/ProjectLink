using UnityEngine;

public class Scale : Interactable
{
    [SerializeField] private SpriteRenderer blockLeft;
    [SerializeField] private SpriteRenderer blockRight;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected override void Start()
    {
        base.Start();

        _boxCollider.size = new Vector2(blockLeft.size.x + blockRight.size.x + 1f, 1f);
    }

    #endregion
}

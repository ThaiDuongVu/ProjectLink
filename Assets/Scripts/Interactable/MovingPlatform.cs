using System.Collections;
using UnityEngine;

public class MovingPlatform : Interactable
{
    [SerializeField] private float speed;
    [SerializeField] private float stopDelay;
    [SerializeField] private Vector2[] positions = new Vector2[2];

    private int _fromIndex;
    private int _toIndex;
    private Vector2 FromPosition => positions[_fromIndex];
    private Vector2 ToPosition => positions[_toIndex];

    public bool IsMoving { get; set; } = true;
    public bool IsActive { get; set; } = true;

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _sprite;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        _fromIndex = 0;
        _toIndex = 1;

        _boxCollider.size = _sprite.size;
    }

    protected override void Update()
    {
        base.Update();

        if (Vector2.Distance(transform.localPosition, ToPosition) <= 0.1f)
        {
            transform.localPosition = ToPosition;

            if (_fromIndex < positions.Length - 1) _fromIndex++;
            else _fromIndex = 0;
            if (_toIndex < positions.Length - 1) _toIndex++;
            else _toIndex = 0;

            StopMoving();
            Invoke(nameof(StartMoving), stopDelay);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!IsActive) return;

        if (IsMoving)
            _rigidbody.MovePosition(_rigidbody.position + speed * Time.fixedDeltaTime * (ToPosition - FromPosition).normalized);
    }

    #endregion

    public void StartMoving()
    {
        IsMoving = true;
    }

    public void StopMoving()
    {
        IsMoving = false;
    }

    public void SetActive(bool value)
    {
        IsActive = value;
    }

    public void ToggleActive()
    {
        IsActive = !IsActive;
    }

    public void SetCollision(bool value)
    {
        _boxCollider.enabled = value;
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, value ? 1f : 0.1f);
    }
}

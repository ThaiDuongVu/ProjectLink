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

    private bool _isMoving = true;

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
            if (_fromIndex < positions.Length - 1) _fromIndex++;
            else _fromIndex = 0;
            if (_toIndex < positions.Length - 1) _toIndex++;
            else _toIndex = 0;

            StartCoroutine(TemporarilyStop(stopDelay));
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isMoving)
            _rigidbody.MovePosition(_rigidbody.position + speed * Time.fixedDeltaTime * (ToPosition - FromPosition).normalized);
    }

    #endregion

    private IEnumerator TemporarilyStop(float duration)
    {
        _isMoving = false;
        yield return new WaitForSeconds(duration);
        _isMoving = true;
    }
}

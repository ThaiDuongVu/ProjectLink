using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Stats")]
    public new string name;

    [Header("References")]
    [SerializeField] private Transform indicator;
    private Transform _currentPusher;
    private Transform _currentHolder;

    private static readonly int HighlightAnimationBool = Animator.StringToHash("isHighlighted");
    private const float HoldInterpolationRatio = 0.5f;

    protected Rigidbody2D Rigidbody;
    protected Animator Animator;
    protected CircleCollider2D CircleCollider;

    #region Unity Events

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        CircleCollider = GetComponent<CircleCollider2D>();
    }

    protected virtual void Update()
    {
        if (_currentPusher) indicator.up = _currentPusher.up;
        if (_currentHolder) Rigidbody.MovePosition(
            Vector2.Lerp(transform.position, _currentHolder.position + _currentHolder.up * CircleCollider.radius, HoldInterpolationRatio)
        );
    }

    #endregion

    #region Holder & Pusher Methods

    public virtual void SetHolder(Transform holder = null)
    {
        _currentHolder = holder;
    }

    public virtual void SetPusher(Transform pusher = null)
    {
        _currentPusher = pusher;
    }

    #endregion

    public virtual void AddForce(Vector2 direction, float force)
    {
        Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public virtual void SetHighlight(bool isHighlighted)
    {
        Animator.SetBool(HighlightAnimationBool, isHighlighted);
    }
}

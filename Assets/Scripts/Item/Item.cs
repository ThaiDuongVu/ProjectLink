using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform indicator;
    private Transform _currentPusher;
    private Transform _currentHolder;

    private static readonly int HighlightAnimationBool = Animator.StringToHash("isHighlighted");

    protected Rigidbody2D Rigidbody;
    protected Animator Animator;

    #region Unity Events

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (_currentPusher) indicator.up = _currentPusher.up;
        if (_currentHolder) indicator.up = _currentHolder.up;
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

using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform indicator;
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
        if (_currentHolder) indicator.up = _currentHolder.up;
    }

    #endregion

    public virtual void AddForce(Vector2 direction, float force)
    {
        Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public virtual void SetHighlight(bool isHighlighted, Transform holder = null)
    {
        Animator.SetBool(HighlightAnimationBool, isHighlighted);
        _currentHolder = holder;
    }
}

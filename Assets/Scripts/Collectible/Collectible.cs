using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Transform _collectTarget;
    [Header("Collect Stats")]
    public float collectInterpolationRatio;
    public float collectDelay;

    public bool IsCollected { get; set; }

    private Animator _animator;
    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");

    private Collider2D _collider;

    #region Unity Events

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (_collectTarget) transform.position = Vector2.Lerp(transform.position, _collectTarget.position, collectInterpolationRatio);
    }

    #endregion

    public virtual bool OnCollected(Player player)
    {
        if (IsCollected) return false;

        _collectTarget = player.transform;
        _animator.SetTrigger(CollectAnimationTrigger);
        _collider.enabled = false;

        IsCollected = true;
        Destroy(gameObject, 1f);

        return true;
    }
}

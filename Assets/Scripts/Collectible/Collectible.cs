using UnityEngine;

public class Collectible : MonoBehaviour
{
    public bool IsCollected { get; set; }

    private Transform _collectTarget;
    private const float CollectInterpolationRatio = 0.25f;

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
        if (_collectTarget) transform.position = Vector2.Lerp(transform.position, _collectTarget.position, CollectInterpolationRatio);
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

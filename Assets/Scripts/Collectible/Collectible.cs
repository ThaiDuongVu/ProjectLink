using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float expireTime;
    public float levelPointsAwardedOnCollected;

    private const float CollectDelay = 0.5f;
    private const float CollectInterpolationRatio = 0.2f;
    private bool _canCollect;
    private bool _isCollected;

    private Transform _target;

    private Animator _animator;
    private static readonly int CollectAnimationTrigger = Animator.StringToHash("collect");

    #region Unity Events

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke(nameof(EnableCollect), CollectDelay);
    }

    private void FixedUpdate()
    {
        if (_isCollected && _target) transform.position = Vector2.Lerp(transform.position, _target.position, CollectInterpolationRatio);
    }

    #endregion

    private void EnableCollect()
    {
        _canCollect = true;

        Invoke(nameof(Expire), expireTime);
    }

    public virtual bool OnCollected(Player player)
    {
        if (!_canCollect) return false;
        if (_isCollected) return false;

        _target = player.transform;
        _animator.SetTrigger(CollectAnimationTrigger);

        Level.Instance.CurrentLevelPoints += levelPointsAwardedOnCollected * player.ComboMultiplier;

        _isCollected = true;
        Invoke(nameof(SelfDestruct), 1f);

        return true;
    }

    private void Expire()
    {
        _animator.SetTrigger(CollectAnimationTrigger);
        Invoke(nameof(SelfDestruct), 1f);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}

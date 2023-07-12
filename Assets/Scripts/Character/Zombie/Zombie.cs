using UnityEngine;

public class Zombie : Character
{
    [Header("Zombie AI Stats")]
    public float trackRate;

    private Player _player;

    private ZombiePathfinder _zombiePathfinder;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _player = FindObjectOfType<Player>();
        _zombiePathfinder = GetComponent<ZombiePathfinder>();
    }

    protected override void Start()
    {
        base.Start();

        InvokeRepeating(nameof(TrackPlayer), 0f, trackRate);
    }

    #endregion

    #region Interface Implementations

    public override void Die()
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Micro);
        GameController.Instance.PlaySlowMotionEffect();

        base.Die();
    }

    #endregion

    private void TrackPlayer()
    {
        _zombiePathfinder.Track(_player.transform);
    }
}

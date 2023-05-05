using UnityEngine;

public class Rookie : Zombie
{
    [Header("Rookie Stats")]
    [SerializeField] private float trackRate;

    private Player _player;

    private RookiePathfinder _rookiePathfinder;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _rookiePathfinder = GetComponent<RookiePathfinder>();
        _player = FindObjectOfType<Player>();
    }

    protected override void Start()
    {
        base.Start();

        InvokeRepeating(nameof(TrackPlayer), 0f, trackRate);
    }

    #endregion

    public override void EnableStagger()
    {
        base.EnableStagger();

        _rookiePathfinder.Stop();
    }

    private void TrackPlayer()
    {
        if (!_player || IsStagger)
        {
            _rookiePathfinder.Stop();
            return;
        }
        _rookiePathfinder.Track(_player.transform);
    }
}

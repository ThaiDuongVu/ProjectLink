using UnityEngine;

public class Pursuer : Zombie
{
    [Header("Pursuer Stats")]
    [SerializeField] private float trackRate;

    private Player _player;

    private PursuerPathfinder _pursuerPathfinder;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _pursuerPathfinder = GetComponent<PursuerPathfinder>();
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

        _pursuerPathfinder.Stop();
    }

    private void TrackPlayer()
    {
        if (!_player || IsStagger)
        {
            _pursuerPathfinder.Stop();
            return;
        }
        _pursuerPathfinder.Track(_player.transform);
    }
}

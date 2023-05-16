using System.Collections;
using UnityEngine;

public class Thrower : Zombie
{
    [Header("Thrower Stats")]
    [SerializeField] private float trackRate;
    [SerializeField] private float throwRate;

    private Player _player;

    private ThrowerMovement _throwerMovement;
    private ThrowerCombat _throwerCombat;
    private ThrowerPathfinder _throwerPathfinder;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _player = FindObjectOfType<Player>();

        _throwerMovement = GetComponent<ThrowerMovement>();
        _throwerCombat = GetComponent<ThrowerCombat>();
        _throwerPathfinder = GetComponent<ThrowerPathfinder>();
    }

    protected override void Start()
    {
        base.Start();

        InvokeRepeating(nameof(TrackPlayer), 0f, trackRate);
        InvokeRepeating(nameof(Throw), throwRate, throwRate);
    }

    protected override void Update()
    {
        base.Update();

        if (CanSeePlayer()) _throwerMovement.SetLookDirection((_player.transform.position - transform.position).normalized);
    }

    #endregion

    public override void EnableStagger()
    {
        base.EnableStagger();

        _throwerPathfinder.Stop();
    }

    private void TrackPlayer()
    {
        if (!_player || IsStagger || CanSeePlayer())
        {
            StopTracking();
            return;
        }
        _throwerPathfinder.Track(_player.transform);
    }

    private void StopTracking()
    {
        _throwerPathfinder.Stop();
    }

    private bool CanSeePlayer()
    {
        var direction = (_player.transform.position - transform.position).normalized;
        var hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity);

        if (!hit) return false;
        return (hit.transform.GetComponent<Player>() != null);
    }

    private void Throw()
    {
        if (!CanSeePlayer()) return;
        StartCoroutine(ThrowProcess());
    }

    private IEnumerator ThrowProcess()
    {
        yield return new WaitForSeconds(1f);

        // TODO: Play throw animation
        yield return new WaitForSeconds(0.5f);
        _throwerCombat.Throw(_throwerMovement.LookDirection);
    }
}

using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class CharacterPathfinder : MonoBehaviour
{
    [Header("Pathfinding Stats")]
    public float stoppingDistance;

    [Header("Events")]
    public UnityEvent onPathFound;
    public UnityEvent onPathReached;

    private CharacterMovement _characterMovement;
    private Vector2 _targetPosition;

    private bool _isTracking;

    private Path _path;
    private Seeker _seeker;

    private int _currentWaypoint;
    public Vector2 Direction { get; set; }
    private const float NextWaypointDistance = 0.1f;

    #region Unity Events

    protected virtual void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _seeker = GetComponent<Seeker>();
    }

    protected virtual void Update()
    {
        if (_path == null || !_isTracking) return;

        // Reached pathfinding destination, stopping...
        if (_currentWaypoint >= _path.vectorPath.Count || Vector2.Distance(transform.position, _targetPosition) <= stoppingDistance)
        {
            onPathReached.Invoke();
            Stop();
            return;
        }

        // Travel to current waypoint
        Direction = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
        _characterMovement.Run(Direction);

        // If waypoint reached then proceed to the next waypoint
        if (Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]) < NextWaypointDistance) _currentWaypoint++;
    }

    #endregion

    #region Tracking Methods

    public virtual bool Track(Vector2 position)
    {
        _targetPosition = position;
        _isTracking = false;

        _seeker.StartPath(transform.position, _targetPosition, path =>
        {
            if (path.error)
            {
                Debug.LogError(path.errorLog);
                Stop();
                return;
            }

            _isTracking = true;
            _path = path;
            _currentWaypoint = 0;

            onPathFound.Invoke();
        });

        return _isTracking;
    }

    public virtual bool Track(Transform target)
    {
        return Track(target.position);
    }

    public virtual void Stop()
    {
        _characterMovement.Stop();
        _isTracking = false;

        _path = null;
        _currentWaypoint = 0;
    }

    #endregion
}
using UnityEngine;

public class Tracker : Enemy
{
    [Header("Tracker Stats")]
    [SerializeField] private float trackRate;
    [SerializeField] private float rotationInterpolationRatio;
    [SerializeField] private float damage;

    private Cat _cat;
    private TrackerMovement _trackerMovement;
    private TrackerPathfinder _trackerPathfinder;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _cat = FindObjectOfType<Cat>();
        _trackerMovement = GetComponent<TrackerMovement>();
        _trackerPathfinder = GetComponent<TrackerPathfinder>();
    }

    protected override void Start()
    {
        base.Start();

        InvokeRepeating(nameof(TrackTarget), 0f, trackRate);
    }

    protected override void Update()
    {
        base.Update();

        transform.right = Vector2.Lerp(transform.right, _trackerPathfinder.Direction, rotationInterpolationRatio);
    }

    #endregion

    private void TrackTarget()
    {
        if (!_cat) return;

        _trackerPathfinder.Track(_cat.transform);
    }

    private void StopTracking()
    {
        _trackerPathfinder.Stop();
    }

    private void Explode()
    {
        Die();
        CameraShaker.Instance.Shake(CameraShakeMode.Normal);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Cat"))
        {
            var cat = other.transform.GetComponent<Cat>();
            cat.TakeDamage(damage, _trackerPathfinder.Direction, other.GetContact(0).point);
            Explode();
        }

        // var damageable = other.transform.GetComponent<IDamageable>();
        // if (damageable != null) damageable.TakeDamage(damage, _trackerPathfinder.Direction, other.GetContact(0).point);
    }
}

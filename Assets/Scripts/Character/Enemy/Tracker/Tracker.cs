using UnityEngine;

public class Tracker : Enemy
{
    [Header("Tracker Stats")]
    [SerializeField] private float trackRate;

    private Cat _cat;

    private TrackerPathfinder _trackerPathfinder;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _cat = FindObjectOfType<Cat>();
        _trackerPathfinder = GetComponent<TrackerPathfinder>();
    }

    protected override void Start()
    {
        base.Start();

        InvokeRepeating(nameof(TrackTarget), 0f, trackRate);
    }

    #endregion

    private void TrackTarget()
    {
        _trackerPathfinder.Track(_cat.transform);
    }

    public void TempStop()
    {
        CancelInvoke();
        _trackerPathfinder.Stop();

        InvokeRepeating(nameof(TrackTarget), trackRate, trackRate);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class ForceButton : Interactable
{
    private bool _isOn;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            if (value != _isOn)
            {
                if (value)
                {
                    onEvent.Invoke();
                }
                else
                {
                    offEvent.Invoke();
                }
            }
            _isOn = value;
        }
    }

    [SerializeField] private Transform knob;

    [Header("References")]
    [SerializeField] private ParticleSystem sparkPrefab;
    [SerializeField] private LineRenderer connectLine;
    [SerializeField] private Transform[] connectedObjects;

    [Header("Events")]
    public UnityEvent onEvent;
    public UnityEvent offEvent;

    #region Unity Events

    protected override void Start()
    {
        base.Start();

        offEvent.Invoke();
    }

    protected override void Update()
    {
        base.Update();

        IsOn = knob.localPosition.y <= 0.3f;
        HandleConnectLine();
    }

    #endregion

    private void HandleConnectLine()
    {
        connectLine.positionCount = connectedObjects.Length * 2;
        var j = 0;
        for (int i = 0; i < connectLine.positionCount; i += 2)
        {
            connectLine.SetPosition(i, transform.position);
            connectLine.SetPosition(i + 1, connectedObjects[j].position);
            j++;
        }
    }
}

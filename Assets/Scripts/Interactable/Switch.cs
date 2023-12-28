using UnityEngine;
using UnityEngine.Events;

public class Switch : Interactable
{
    private bool _isOn;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;

            if (value)
            {
                onEvent.Invoke();
            }
            else
            {
                offEvent.Invoke();
            }
            _animator.SetBool(SwitchAnimationBool, value);
        }
    }

    private Animator _animator;
    private static readonly int SwitchAnimationBool = Animator.StringToHash("isOn");

    [Header("Stats")]
    [SerializeField] private bool onByDefault;

    [Header("References")]
    [SerializeField] private ParticleSystem sparkPrefab;
    [SerializeField] private LineRenderer connectLine;
    [SerializeField] private Transform[] connectedObjects;

    [Header("Audio References")]
    [SerializeField] private AudioSource toggleAudio;

    [Header("Events")]
    public UnityEvent onEvent;
    public UnityEvent offEvent;

    #region Unity Event

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        IsOn = onByDefault;
    }

    protected override void Update()
    {
        base.Update();

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

    public void Toggle()
    {
        IsOn = !IsOn;

        Instantiate(sparkPrefab, transform.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Micro);

        toggleAudio.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ISwitchActivator>() != null)
        {
            Toggle();
        }
    }
}

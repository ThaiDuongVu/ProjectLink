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

    [SerializeField] private ParticleSystem sparkPrefab;

    public UnityEvent onEvent;
    public UnityEvent offEvent;

    [SerializeField] private LineRenderer connectLine;
    [SerializeField] private Transform connectedObject;

    #region Unity Event

    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        IsOn = false;
    }

    protected override void Update()
    {
        base.Update();

        if (connectedObject) connectLine.SetPositions(new Vector3[] { transform.position, connectedObject.position });
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block"))
        {
            IsOn = !IsOn;

            Instantiate(sparkPrefab, transform.position, Quaternion.identity);
            CameraShaker.Instance.Shake(CameraShakeMode.Micro);
        }
    }
}

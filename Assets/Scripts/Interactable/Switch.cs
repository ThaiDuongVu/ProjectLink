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

        IsOn = false;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Block")) 
        {
            IsOn = !IsOn;
            CameraShaker.Instance.Shake(CameraShakeMode.Micro);
        }
    }
}

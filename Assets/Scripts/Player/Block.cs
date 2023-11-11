using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Block : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float swingForce;

    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;

    private bool _isSwinging;
    private Vector2 _swingDirection;

    private static readonly int SleepAnimationBool = Animator.StringToHash("isSleeping");
    private static readonly int SwingAnimationBool = Animator.StringToHash("isSwinging");

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

            if (!value) _rigidbody.velocity = Vector2.zero;
            _rigidbody.isKinematic = !value;
            _light.enabled = value;
            _animator.SetBool(SleepAnimationBool, !value);
        }
    }

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private DistanceJoint2D _distanceJoint;

    private Light2D _light;

    #region Unity Events

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _distanceJoint = GetComponent<DistanceJoint2D>();

        _light = GetComponentInChildren<Light2D>();
    }

    private void FixedUpdate()
    {
        if (_isSwinging) _rigidbody.AddForce(_swingDirection * swingForce, ForceMode2D.Force);
    }

    #endregion

    public void SetConnectedBody(Rigidbody2D body)
    {
        _distanceJoint.connectedBody = body;
    }

    public Rigidbody2D GetRigidbody => _rigidbody;

    #region Swing Methods

    public void Swing(Vector2 direction)
    {
        _isSwinging = true;
        _swingDirection = direction;

        _animator.SetBool(SwingAnimationBool, true);
        if (direction.x < 0f) SetFlip(true);
        else if (direction.x > 0f) SetFlip(false);
    }

    public void StopSwing()
    {
        _isSwinging = false;
        _animator.SetBool(SwingAnimationBool, false);
    }

    #endregion

    public void SetFlip(bool value)
    {
        sprite.flipX = value;
    }
}

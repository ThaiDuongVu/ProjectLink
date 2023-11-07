using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Block : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float swingForce;
    private bool _isSwinging;
    private Vector2 _swingDirection;

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
    }

    public void StopSwing()
    {
        _isSwinging = false;
    }

    #endregion
}

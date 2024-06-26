using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Block : MonoBehaviour, ISwitchActivator
{
    [Header("Stats")]
    [SerializeField] private float swingForce;
    public BlockType type;

    [Header("References")]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private ParticleSystem sparkPrefab;

    [SerializeField] private string[] collisionReactionTexts;
    [SerializeField] private string[] portalReactionTexts;

    private bool _isSwinging;
    private Vector2 _swingDirection;

    private static readonly int SleepAnimationBool = Animator.StringToHash("isSleeping");
    private static readonly int SwingAnimationBool = Animator.StringToHash("isSwinging");
    private static readonly int ExitAnimationTrigger = Animator.StringToHash("exit");

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;

            if (!value)
            {
                _rigidbody.velocity = Vector2.zero;
                _positionStack.Push(transform.position);
                Instantiate(sparkPrefab, transform.position, Quaternion.identity);
            }

            _rigidbody.isKinematic = !value;
            _light.enabled = value;
            _animator.SetBool(SleepAnimationBool, !value);
        }
    }

    private bool _isInPortal;
    public bool IsInPortal
    {
        get => _isInPortal;
        set
        {
            _isInPortal = value;
        }
    }
    private Portal _targetPortal;
    private const float EnterPortalInterpolationRatio = 0.4f;

    private Stack<Vector2> _positionStack = new();

    private Player _player;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private DistanceJoint2D _distanceJoint;

    private Light2D _light;

    #region Unity Events

    private void Awake()
    {
        _player = GetComponentInParent<Player>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _distanceJoint = GetComponent<DistanceJoint2D>();

        _light = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        // _positionStack.Push(transform.position);
    }

    private void Update()
    {
        if (IsInPortal)
            transform.position = Vector2.Lerp(transform.position, _targetPortal.transform.position, EnterPortalInterpolationRatio);
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

    public void EnterPortal(Portal portal)
    {
        if (!portal) return;
        if (portal.type != type) return;

        _targetPortal = portal;
        IsInPortal = true;
        IsActive = false;

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.PlaySlowMotionEffect();
        EffectsController.Instance.SpawnSpeechBubble(transform, Vector2.zero, portalReactionTexts[Random.Range(0, portalReactionTexts.Length)]);

        _player.CheckWinCondition();
        _player.playerPortalAudio.Play();
    }

    public void Exit()
    {
        _animator.SetTrigger(ExitAnimationTrigger);
    }

    public void Damage(Enemy enemy)
    {
        enemy.Die();
        EffectsController.Instance.SpawnSpeechBubble(transform, Vector2.zero, portalReactionTexts[Random.Range(0, portalReactionTexts.Length)]);
    }

    public bool PopPosition()
    {
        if (_positionStack.Count == 0) return false;
        if (IsInPortal) return false;

        transform.position = _positionStack.Pop();
        return true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_rigidbody.velocity.magnitude >= 6f && other.transform.CompareTag("Map"))
        {
            EffectsController.Instance.SpawnSpeechBubble(
                transform,
                Vector2.zero,
                collisionReactionTexts[Random.Range(0, collisionReactionTexts.Length)]
            );
            CameraShaker.Instance.Shake(CameraShakeMode.Micro);
            _player.playerCollisionAudio.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            EnterPortal(other.GetComponent<Portal>());
        }
        else if (other.CompareTag("Collectible"))
        {
            other.GetComponent<Collectible>().OnCollected(_player);
            _player.playerStarAudio.Play();
        }
    }
}

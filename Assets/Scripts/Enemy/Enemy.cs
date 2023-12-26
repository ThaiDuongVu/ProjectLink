using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType type;
    [SerializeField] private ParticleSystem explosionPrefab;

    [Header("Movement References")]
    [SerializeField] private float speed;
    [SerializeField] private float stopDelay;
    [SerializeField] private Vector2[] positions = new Vector2[2];

    private int _fromIndex;
    private int _toIndex;
    private Vector2 FromPosition => positions[_fromIndex];
    private Vector2 ToPosition => positions[_toIndex];

    public bool IsMoving { get; set; } = true;
    public bool IsActive { get; set; } = true;

    private Rigidbody2D _rigidbody;

    #region Unity Events

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _fromIndex = 0;
        _toIndex = 1;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.localPosition, ToPosition) <= 0.1f)
        {
            if (_fromIndex < positions.Length - 1) _fromIndex++;
            else _fromIndex = 0;
            if (_toIndex < positions.Length - 1) _toIndex++;
            else _toIndex = 0;

            StopMoving();
            Invoke(nameof(StartMoving), stopDelay);
        }
    }

    private void FixedUpdate()
    {
        if (!IsActive) return;

        if (IsMoving)
            _rigidbody.MovePosition(_rigidbody.position + speed * Time.fixedDeltaTime * (ToPosition - FromPosition).normalized);
    }

    #endregion

    public void StartMoving()
    {
        IsMoving = true;
    }

    public void StopMoving()
    {
        IsMoving = false;
    }

    public void SetActive(bool value)
    {
        IsActive = value;
    }

    public void ToggleActive()
    {
        IsActive = !IsActive;
    }

    public void Die()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        GameController.Instance.explosionAudio.Play();
        GameController.Instance.PlaySlowMotionEffect();

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Block"))
        {
            var block = other.transform.GetComponent<Block>();
            var player = block.GetComponentInParent<Player>();

            if (type == EnemyType.Red)
            {
                player.Die();
                return;
            }

            if ((type == EnemyType.Blue && block.type == BlockType.Blue) || (type == EnemyType.Pink && block.type == BlockType.Pink))
            {
                block.Damage(this);
            }
            else
            {
                player.Die();
                return;
            }
        }
    }
}

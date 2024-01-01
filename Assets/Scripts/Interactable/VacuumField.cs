using UnityEngine;

public class VacuumField : Interactable
{
    private BoxCollider2D _boxCollider;
    private SpriteRenderer _sprite;

    [SerializeField] private ParticleSystem sparkPrefab;

    [Header("Audio References")]
    [SerializeField] private AudioSource enterAudio;
    [SerializeField] private AudioSource exitAudio;

    #region Unity Events

    protected override void Awake()
    {
        base.Awake();

        _boxCollider = GetComponent<BoxCollider2D>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        _boxCollider.size = _sprite.size;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rigidbody = other.GetComponent<Rigidbody2D>();
        if (!rigidbody || rigidbody.gravityScale == 0f) return;

        // rigidbody.velocity /= 2f;
        rigidbody.gravityScale = 0f;
        enterAudio.Play();

        Instantiate(sparkPrefab, rigidbody.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Micro);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var rigidbody = other.GetComponent<Rigidbody2D>();
        if (!rigidbody || rigidbody.gravityScale > 0f) return;

        rigidbody.gravityScale = 3f;
    }
}

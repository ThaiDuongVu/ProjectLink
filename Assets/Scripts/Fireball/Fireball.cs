using UnityEngine;

public class Fireball : MonoBehaviour
{
    [Header("Stats")]
    public float speed;
    public float damage;

    [Header("References")]
    [SerializeField] private ParticleSystem explosion;

    public Vector2 CurrentDirection { get; set; } = Vector2.up;

    private Rigidbody2D _rigidbody;

    #region Unity Events

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (CurrentDirection != Vector2.zero) _rigidbody.velocity = CurrentDirection * speed;
    }

    #endregion

    private void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Light);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Explode();
    }
}

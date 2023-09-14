using UnityEngine;

public class Shuriken : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage;

    [Header("References")]
    [SerializeField] private ParticleSystem explosionPrefab;

    private Rigidbody2D _rigidbody;

    #region Unity Events

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    #endregion

    public void Fly(Vector2 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
        _rigidbody.AddTorque(force);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        CameraShaker.Instance.Shake(CameraShakeMode.Light);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}

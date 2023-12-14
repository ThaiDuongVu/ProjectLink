using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private ParticleSystem sparkPrefab;
    [SerializeField] private Color collectColor;

    public virtual void OnCollected(Player player)
    {
        EffectsController.Instance.SpawnPopText(transform.position, "collected", collectColor);
        Instantiate(sparkPrefab, transform.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Micro);

        Destroy(gameObject);
    }
}

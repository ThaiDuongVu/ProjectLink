using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private ParticleSystem sparkPrefab;
    [SerializeField] private Color collectColor;

    [SerializeField] private Transform attachedObject;
    [SerializeField] private Vector3 attachedOffset;

    #region Unity Events

    private void Update()
    {
        if (attachedObject) transform.position = attachedObject.position + attachedOffset;
    }

    #endregion

    public virtual void OnCollected(Player player)
    {
        EffectsController.Instance.SpawnPopText(transform.position, "+1", collectColor);
        Instantiate(sparkPrefab, transform.position, Quaternion.identity);
        CameraShaker.Instance.Shake(CameraShakeMode.Micro);

        Destroy(gameObject);
    }
}

using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [SerializeField] private Collectible collectiblePrefab;
    [SerializeField] private Vector2Int amountRange;

    private const float InitForce = 5f;

    public void Spawn()
    {
        for (var i = 0; i < Random.Range(amountRange.x, amountRange.y + 1); i++)
        {
            Instantiate(collectiblePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().AddForce(
                new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * InitForce,
                ForceMode2D.Impulse
            );
        }
    }
}

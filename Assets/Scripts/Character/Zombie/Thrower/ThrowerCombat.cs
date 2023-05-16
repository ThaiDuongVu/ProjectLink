using UnityEngine;

public class ThrowerCombat : ZombieCombat
{
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Fireball fireballPrefab;

    public void Throw(Vector2 direction)
    {
        Instantiate(fireballPrefab, throwPoint.position, Quaternion.identity).CurrentDirection = direction;
    }
}

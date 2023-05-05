using System.Collections;
using UnityEngine;
using Pathfinding;

public class ZombieWaveController : MonoBehaviour
{
    [Header("Wave References")]
    [SerializeField] private bool startWaveOnAwake;
    [SerializeField] private float waveDuration;
    [SerializeField] private float waveInterval;
    private int _currentWave;

    [Header("Spawn References")]
    [SerializeField] private Vector2 spawnIntervalRange;

    [Header("Zombie References")]
    [SerializeField] private Zombie[] zombiePrefabs;
    [SerializeField] private Transform zombiesParent;
    [SerializeField] private Transform zombieHolder;

    [Header("Population Control")]
    [SerializeField] private int maxZombiesPopulation;

    private bool _isInWave;

    private Timer _waveTimer;
    private Timer _spawnTimer;

    [Header("Player References")]
    [SerializeField] private float playerAvoidRadius;
    private Player _player;

    #region Unity Events

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        _waveTimer = new Timer(waveDuration);
        _spawnTimer = new Timer(Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));

        if (startWaveOnAwake) StartWave();
    }

    private void Update()
    {
        if (!_isInWave || GameController.Instance.State != GameState.InProgress) return;

        if (_spawnTimer.IsReached())
        {
            StartCoroutine(Spawn());
            _spawnTimer = new Timer(Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));
        }

        if (_waveTimer.IsReached())
        {
            EndWave();

            _waveTimer = new Timer(waveDuration);
            Invoke(nameof(StartWave), waveInterval);
        }
    }

    #endregion

    public void StartWave()
    {
        _isInWave = true;
        _currentWave++;
    }

    public void EndWave()
    {
        _isInWave = false;
    }

    private IEnumerator Spawn()
    {
        if (FindObjectsOfType<Zombie>().Length >= maxZombiesPopulation) yield break;

        // Regenerate position if too close to player
        var position = GenerateSpawnPosition();
        var playerPosition = _player.transform.position;
        while (Vector2.Distance(position, playerPosition) <= playerAvoidRadius) position = GenerateSpawnPosition();

        // Spawn zombie
        var holder = Instantiate(zombieHolder, position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Length)], position, Quaternion.identity).transform.SetParent(zombiesParent);
        Destroy(holder.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach (var zombie in FindObjectsOfType<Zombie>()) zombie.Die();
    }

    private Vector3 GenerateSpawnPosition()
    {
        GraphNode node = null;
        while (node is not { Walkable: true })
        {
            var nodeIndex = Random.Range(0, Level.Instance.GetPathfinderNodeCount());
            node = Level.Instance.GetPathfinder().data.gridGraph.nodes[nodeIndex];
        }

        return (Vector3)node.position;
    }
}

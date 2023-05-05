using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level : MonoBehaviour
{
    #region Singleton

    private static Level _levelInstance;

    public static Level Instance
    {
        get
        {
            if (_levelInstance == null) _levelInstance = FindObjectOfType<Level>();
            return _levelInstance;
        }
    }

    #endregion

    private AstarPath _astarPath;

    [SerializeField] private TMP_Text fpsText;

    [Header("Level Progress References")]
    private bool _levelCompleted;
    public float levelPointThreshold;
    [SerializeField] private Image levelProgressBar;
    private float _currentLevelPoints;
    public float CurrentLevelPoints
    {
        get => _currentLevelPoints;
        set
        {
            _currentLevelPoints = value;
            levelProgressBar.transform.localScale = new Vector2(value / levelPointThreshold, 1f);

            if (value >= levelPointThreshold & !_levelCompleted)
            {
                _zombieWaveController.DestroyAllEnemies();
                _zombieWaveController.EndWave();
                _zombieWaveController.CancelInvoke();

                _portal.gameObject.SetActive(true);

                CameraShaker.Instance.Shake(CameraShakeMode.Light);
                GameController.Instance.PlaySlowMotionEffect();
                _levelCompleted = true;
            }
        }
    }

    private ZombieWaveController _zombieWaveController;
    private Portal _portal;
    private Player _player;

    #region Unity Events

    private void Awake()
    {
        _astarPath = GetComponent<AstarPath>();
        _zombieWaveController = GetComponent<ZombieWaveController>();
        _portal = GetComponentInChildren<Portal>();
        _player = GetComponentInChildren<Player>();
    }

    private void Start()
    {
        CurrentLevelPoints = 0;
        _portal.gameObject.SetActive(false);

        // TODO: Equip random weapon
    }

    private void Update()
    {
        fpsText.SetText(((int)(1f / Time.deltaTime)).ToString());
    }

    #endregion

    public AstarPath GetPathfinder() => _astarPath;

    public int GetPathfinderNodeCount() => _astarPath.data.gridGraph.nodes.Length;

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);

        var tempData = new TempSaveData(_player.CollectedCoins, _player.Health);
        SaveLoadController.SaveTempData(tempData);

        SceneLoader.Instance.Restart();
    }
}

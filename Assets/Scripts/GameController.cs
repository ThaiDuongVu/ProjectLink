using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    #region Singleton

    private static GameController _gameControllerInstance;

    public static GameController Instance
    {
        get
        {
            if (_gameControllerInstance == null) _gameControllerInstance = FindObjectOfType<GameController>();
            return _gameControllerInstance;
        }
    }

    #endregion

    public GameState State { get; private set; } = GameState.InProgress;
    public const float TargetFrameRate = 60f;

    [Header("Menus")]
    [SerializeField] private Canvas mainUI;
    private Menu _pauseMenu;
    private Menu _gameOverMenu;
    private Menu _levelCompleteMenu;

    private InputManager _inputManager;

    #region Unity Event

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle game pause input
        _inputManager.Game.Escape.performed += EscapeOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager?.Disable();
    }

    private void Awake()
    {
        _pauseMenu = mainUI.GetComponentsInChildren<Menu>()[0];
        _gameOverMenu = mainUI.GetComponentsInChildren<Menu>()[1];
        _levelCompleteMenu = mainUI.GetComponentsInChildren<Menu>()[2];
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = ((int)TargetFrameRate);

        PostProcessingController.Instance.SetDepthOfField(false);
        PostProcessingController.Instance.SetChromaticAberration(false);
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity);

        mainUI.gameObject.SetActive(true);
        SetCursorEnabled(false);
        SetTimeScale();
    }

    #endregion

    #region Input Handlers

    private void EscapeOnPerformed(InputAction.CallbackContext context)
    {
        InputTypeController.Instance.CheckInputType(context);

        if (State == GameState.InProgress) Pause();
    }

    #endregion

    private static void SetCursorEnabled(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    #region Game State Methods

    public void Pause()
    {
        SetGameState(GameState.NotInProgress);
        _pauseMenu.SetActive(true);

        PostProcessingController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    public void Resume()
    {
        SetGameState(GameState.InProgress);
        _pauseMenu.SetActive(false);

        PostProcessingController.Instance.SetDepthOfField(false);
        SetCursorEnabled(false);
    }

    public IEnumerator GameOver(string message = "", float delay = 0.5f)
    {
        yield return new WaitForSeconds(delay);

        State = GameState.NotInProgress;
        _gameOverMenu.SetActive(true, message);

        PostProcessingController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    public IEnumerator CompleteLevel(float delay = 1f)
    {
        yield return new WaitForSeconds(delay);

        State = GameState.NotInProgress;
        _levelCompleteMenu.SetActive(true);

        PostProcessingController.Instance.SetDepthOfField(true);
        SetCursorEnabled(true);
    }

    #endregion

    private static IEnumerator SlowMotionEffect(float scale, float duration)
    {
        // Slow down
        SetTimeScale(scale);
        PostProcessingController.Instance.SetChromaticAberration(true);
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity + 0.1f);

        yield return new WaitForSeconds(duration);

        // Back to normal
        SetTimeScale();
        PostProcessingController.Instance.SetChromaticAberration(false);
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity);
    }

    public void PlaySlowMotionEffect(float scale = 0.5f, float duration = 0.25f)
    {
        StartCoroutine(SlowMotionEffect(scale, duration));
    }

    public void SetGameState(GameState state)
    {
        State = state;
        if (state == GameState.NotInProgress) SetTimeScale(0f);
        else if (state == GameState.InProgress) SetTimeScale();
    }

    public static void SetTimeScale(float scale = 1f)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.01666667f * Time.timeScale;
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    public GameState State { get; set; }

    [Header("Menu References")]
    [SerializeField] private SimpleMenu pauseMenu;
    [SerializeField] private SimpleMenu levelCompletedMenu;
    [SerializeField] private SimpleMenu gameOverMenu;

    [Header("UI References")]
    [SerializeField] private RatingDisplay starRatingDisplay;

    private InputManager _inputManager;

    #region Unity Events

    private void OnEnable()
    {
        _inputManager = new InputManager();

        // Handle player pause/resume input
        _inputManager.Game.Escape.performed += EscapeOnPerformed;

        _inputManager.Enable();
    }

    private void OnDisable()
    {
        _inputManager.Disable();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        SetCursorEnabled(false);
        SetTimeScale(1f);
    }

    #endregion

    #region Input Handlers

    private void EscapeOnPerformed(InputAction.CallbackContext context)
    {
        if (State == GameState.InProgress) Pause();
        else if (State == GameState.Paused) Resume();
    }

    #endregion

    #region Pause/Resume Methods

    public void Pause()
    {
        SetTimeScale(0f);
        pauseMenu.SetActive(true);

        SetGameState(GameState.Paused);
        PostProcessingController.Instance.SetDepthOfField(true);
    }

    public void Resume()
    {
        SetTimeScale(1f);
        pauseMenu.SetActive(false);

        SetGameState(GameState.InProgress);
        PostProcessingController.Instance.SetDepthOfField(false);
    }

    #endregion

    public IEnumerator CompleteLevel(int rating)
    {
        yield return new WaitForSeconds(0.5f);

        levelCompletedMenu.SetActive(true);
        starRatingDisplay.UpdateRating(rating);

        var levelName = SceneManager.GetActiveScene().name;
        // Save level rating
        PlayerPrefs.SetInt(levelName, rating);
        // Unlock next level
        var nextLevelIndex = Convert.ToInt32(levelName[5..]) + 1;
        PlayerPrefs.SetInt($"Level{(nextLevelIndex < 10 ? "0" : "")}{nextLevelIndex}", 0);

        SetGameState(GameState.Over);
        PostProcessingController.Instance.SetDepthOfField(true);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);

        gameOverMenu.SetActive(true);

        SetGameState(GameState.Over);
        PostProcessingController.Instance.SetDepthOfField(true);
    }

    public void SetGameState(GameState state)
    {
        State = state;
    }

    public static void SetCursorEnabled(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }

    public static void SetTimeScale(float scale = 1f)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.01666667f * Time.timeScale;
    }


    #region Slow Motion Methods

    private static IEnumerator SlowMotionEffect(float scale, float duration)
    {
        // Slow down
        SetTimeScale(scale);
        PostProcessingController.Instance.SetChromaticAberration(true);
        // PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity + 0.1f);

        yield return new WaitForSecondsRealtime(duration);

        // Back to normal
        SetTimeScale();
        PostProcessingController.Instance.SetChromaticAberration(false);
        // PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity);
    }

    public void PlaySlowMotionEffect(float scale = 0.5f, float duration = 0.2f)
    {
        StartCoroutine(SlowMotionEffect(scale, duration));
    }

    #endregion
}

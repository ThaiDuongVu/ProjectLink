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
            if (_gameControllerInstance == null) _gameControllerInstance = FindFirstObjectByType<GameController>();
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

    [Header("Audio References")]
    [SerializeField] private AudioSource pauseAudio;
    [SerializeField] private AudioSource resumeAudio;
    [SerializeField] private AudioSource levelCompleteAudio;
    [SerializeField] private AudioSource gameOverAudio;

    public AudioSource explosionAudio; 

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
        // else if (State == GameState.Paused) Resume();
    }

    #endregion

    #region Pause/Resume Methods

    public void Pause()
    {
        SetTimeScale(0f);
        SetCursorEnabled(true);
        pauseMenu.SetActive(true);

        SetGameState(GameState.Paused);
        PostProcessingController.Instance.SetDepthOfField(true);

        pauseAudio.Play();
    }

    public void Resume()
    {
        SetTimeScale(1f);
        SetCursorEnabled(false);
        pauseMenu.SetActive(false);

        SetGameState(GameState.InProgress);
        PostProcessingController.Instance.SetDepthOfField(false);

        resumeAudio.Play();
    }

    #endregion

    public IEnumerator CompleteLevel(int rating)
    {
        yield return new WaitForSeconds(0.5f);

        SetCursorEnabled(true);
        levelCompletedMenu.SetActive(true);

        if (SceneManager.GetActiveScene().name.Equals("Tutorial"))
        {
            starRatingDisplay.UpdateRating(3);
            TutorialController.Instance.gameObject.SetActive(false);
        }
        else
        {
            starRatingDisplay.UpdateRating(rating);

            // Save highest level rating
            var levelName = SceneManager.GetActiveScene().name;
            if (PlayerPrefs.GetInt(levelName) < rating) PlayerPrefs.SetInt(levelName, rating);

            // Unlock next level (if not already unlocked)
            var nextLevelIndex = Convert.ToInt32(levelName[5..]) + 1;
            var nextLevelName = $"Level{(nextLevelIndex < 10 ? "0" : "")}{nextLevelIndex}";
            if (PlayerPrefs.GetInt(nextLevelName, -1) < 0) PlayerPrefs.SetInt(nextLevelName, 0);
        }

        SetGameState(GameState.Over);
        PostProcessingController.Instance.SetDepthOfField(true);

        levelCompleteAudio.Play();
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.5f);

        SetCursorEnabled(true);
        gameOverMenu.SetActive(true);

        SetGameState(GameState.Over);
        PostProcessingController.Instance.SetDepthOfField(true);

        gameOverAudio.Play();
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
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity + 0.05f);

        yield return new WaitForSecondsRealtime(duration);

        // Back to normal
        SetTimeScale();
        PostProcessingController.Instance.SetChromaticAberration(false);
        PostProcessingController.Instance.SetVignetteIntensity(PostProcessingController.DefaultVignetteIntensity);
    }

    public void PlaySlowMotionEffect(float scale = 0.5f, float duration = 0.2f)
    {
        StartCoroutine(SlowMotionEffect(scale, duration));
    }

    #endregion
}
